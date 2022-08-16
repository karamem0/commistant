//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Commands
{

    public class InMeetingCommand : Command
    {

        private readonly QrCodeService qrCodeService;

        private readonly ILogger logger;

        public InMeetingCommand(
            ServiceClientCredentials credentials,
            QrCodeService qrCodeService,
            ILogger<InMeetingCommand> logger) : base(credentials)
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
            if (property.InMeetingSchedule <= 0)
            {
                return;
            }
            var startTime = property.ScheduledStartTime;
            if (startTime is null)
            {
                return;
            }
            var currentTime = DateTime.UtcNow;
            var timeSpan = currentTime - (DateTime)startTime;
            if (timeSpan.TotalMinutes < 0)
            {
                return;
            }
            if ((int)timeSpan.TotalMinutes % property.InMeetingSchedule > 0)
            {
                return;
            }
            var client = new ConnectorClient(new Uri(reference.ServiceUrl), this.credentials);
            if (string.IsNullOrEmpty(property.InMeetingMessage) is not true)
            {
                this.logger.InMeetingMessageSending(reference, property.InMeetingMessage);
                var activity = Activity.CreateMessageActivity();
                activity.From = reference.Bot;
                activity.Recipient = reference.User;
                activity.Conversation = reference.Conversation;
                activity.Text = property.InMeetingMessage;
                _ = await client.Conversations.SendToConversationAsync(
                    (Activity)activity,
                    cancellationToken: cancellationToken);
            }
            if (string.IsNullOrEmpty(property.InMeetingUrl) is not true)
            {
                this.logger.InMeetingUrlSending(reference, property.InMeetingUrl);
                var bytes = await this.qrCodeService.CreateAsync(property.InMeetingUrl);
                var base64 = Convert.ToBase64String(bytes);
                var activity = Activity.CreateMessageActivity();
                activity.From = reference.Bot;
                activity.Recipient = reference.User;
                activity.Conversation = reference.Conversation;
                activity.Text = property.InMeetingUrl;
                activity.Attachments = new List<Attachment>()
                {
                    new Attachment()
                    {
                        ContentType = "image/png",
                        ContentUrl = $"data:image/png;base64,{base64}"
                    }
                };
                _ = await client.Conversations.SendToConversationAsync(
                    (Activity)activity,
                    cancellationToken: cancellationToken);
            }
        }

    }

}
