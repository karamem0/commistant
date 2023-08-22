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

    public class StartMeetingCommand : Command
    {

        private readonly IDateTimeService dateTimeService;

        private readonly IConnectorClientService connectorClientService;

        private readonly IQrCodeService qrCodeService;

        private readonly ILogger logger;

        public StartMeetingCommand(
            IDateTimeService dateTimeService,
            IConnectorClientService connectorClientService,
            IQrCodeService qrCodeService,
            ILogger<StartMeetingCommand> logger
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
            if (property.StartMeetingSchedule < 0)
            {
                return;
            }
            if (property.StartMeetingSended is true)
            {
                return;
            }
            var startTime = property.ScheduledStartTime;
            if (startTime is null)
            {
                return;
            }
            var currentTime = this.dateTimeService.GetCurrentDateTime();
            var timeSpan = currentTime - (DateTime)startTime;
            if (timeSpan.TotalMinutes < 0)
            {
                return;
            }
            if (property.StartMeetingSchedule > (int)timeSpan.TotalMinutes)
            {
                return;
            }
            try
            {
                this.logger.StartMeetingMessageNotifying(reference, property);
                var card = new AdaptiveCard("1.3");
                if (string.IsNullOrEmpty(property.StartMeetingMessage) is not true)
                {
                    card.Body.Add(new AdaptiveTextBlock()
                    {
                        Text = property.StartMeetingMessage,
                        Wrap = true
                    });
                }
                if (string.IsNullOrEmpty(property.StartMeetingUrl) is not true)
                {
                    var bytes = await this.qrCodeService.CreateAsync(property.StartMeetingUrl);
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
                this.logger.StartMeetingMessageNotified(reference, property);
            }
            property.StartMeetingSended = true;
        }

    }

}
