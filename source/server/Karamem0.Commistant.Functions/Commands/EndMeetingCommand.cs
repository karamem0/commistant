//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Commands
{

    public class EndMeetingCommand : Command
    {

        private readonly IDateTimeService dateTimeService;

        private readonly IConnectorClientService connectorClientService;

        private readonly IQrCodeService qrCodeService;

        private readonly ILogger logger;

        public EndMeetingCommand(
            IDateTimeService dateTimeService,
            IConnectorClientService connectorClientService,
            IQrCodeService qrCodeService,
            ILogger<EndMeetingCommand> logger
        ) : base()
        {
            this.dateTimeService = dateTimeService;
            this.connectorClientService = connectorClientService;
            this.qrCodeService = qrCodeService;
            this.logger = logger;
        }

        public override async Task ExecuteAsync(
            ConversationProperty property,
            ConversationReference reference,
            CancellationToken cancellationToken = default
        )
        {
            if (property.InMeeting is not true)
            {
                return;
            }
            if (property.EndMeetingSchedule < 0)
            {
                return;
            }
            if (property.EndMeetingSended is true)
            {
                return;
            }
            var endTime = property.ScheduledEndTime;
            if (endTime is null)
            {
                return;
            }
            var currentTime = this.dateTimeService.GetCurrentDateTime();
            var timeSpan = (DateTime)endTime - currentTime;
            if (timeSpan.TotalMinutes < 0)
            {
                return;
            }
            if ((int)timeSpan.TotalMinutes > property.EndMeetingSchedule)
            {
                return;
            }
            try
            {
                this.logger.EndMeetingMessageNotifying(reference, property);
                var card = new AdaptiveCard("1.3");
                if (string.IsNullOrEmpty(property.EndMeetingMessage) is not true)
                {
                    card.Body.Add(new AdaptiveTextBlock()
                    {
                        Text = property.EndMeetingMessage,
                        Wrap = true
                    });
                }
                if (string.IsNullOrEmpty(property.EndMeetingUrl) is not true)
                {
                    var bytes = await this.qrCodeService.CreateAsync(property.EndMeetingUrl);
                    var base64 = WebEncoders.Base64UrlEncode(bytes);
                    card.Body.Add(new AdaptiveImage()
                    {
                        AltText = property.InMeetingUrl,
                        Size = AdaptiveImageSize.Stretch,
                        Url = new Uri($"data:image/png;base64,{base64}")
                    });
                }
                var activity = MessageFactory.Attachment(new Attachment()
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = JsonConvert.DeserializeObject(card.ToJson())
                });
                activity.From = reference.Bot;
                activity.Recipient = reference.User;
                activity.Conversation = reference.Conversation;
                _ = await this.connectorClientService.SendActivityAsync(
                    new Uri(reference.ServiceUrl),
                    (Activity)activity,
                    cancellationToken: cancellationToken
                );
            }
            finally
            {
                this.logger.EndMeetingMessageNotified(reference, property);
            }
            property.EndMeetingSended = true;
        }

    }

}
