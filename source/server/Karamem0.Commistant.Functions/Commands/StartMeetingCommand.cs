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
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
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

        private readonly QrCodeService qrCodeService;

        private readonly ILogger logger;

        public StartMeetingCommand(
            ServiceClientCredentials credentials,
            QrCodeService qrCodeService,
            ILogger<StartMeetingCommand> logger) : base(credentials)
        {
            this.qrCodeService = qrCodeService;
            this.logger = logger;
        }

        public override async Task ExecuteAsync(
            ConversationProperty property,
            ConversationReference reference,
            CancellationToken cancellationToken
        )
        {
            if (property.StartMeetingSchedule < 0)
            {
                return;
            }
            if (property.StartMeetingSended is true)
            {
                return;
            }
            var endTime = property.ScheduledEndTime;
            if (endTime is null)
            {
                return;
            }
            var currentTime = DateTime.UtcNow;
            var timeSpan = (DateTime)endTime - currentTime;
            if (timeSpan.TotalMinutes < 0)
            {
                return;
            }
            if ((int)timeSpan.TotalMinutes > property.StartMeetingSchedule)
            {
                return;
            }
            try
            {
                this.logger.StartMeetingMessageNotifying(reference, property);
                var card = new AdaptiveCard("1.3");
                var client = new ConnectorClient(new Uri(reference.ServiceUrl), this.credentials);
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
                _ = await client.Conversations.SendToConversationAsync(
                    (Activity)activity,
                    cancellationToken: cancellationToken);
            }
            finally
            {
                this.logger.StartMeetingMessageNotified(reference, property);
            }
            property.StartMeetingSended = true;
        }

    }

}
