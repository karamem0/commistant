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
            if ((int)timeSpan.TotalMinutes < property.StartMeetingSchedule)
            {
                return;
            }
            var client = new ConnectorClient(new Uri(reference.ServiceUrl), this.credentials);
            if (string.IsNullOrEmpty(property.StartMeetingMessage) is not true)
            {
                this.logger.StartMeetingMessageSending(reference, property.StartMeetingMessage);
                var activity = Activity.CreateMessageActivity();
                activity.From = reference.Bot;
                activity.Recipient = reference.User;
                activity.Conversation = reference.Conversation;
                activity.Text = property.StartMeetingMessage;
                _ = await client.Conversations.SendToConversationAsync(
                    (Activity)activity,
                    cancellationToken: cancellationToken);
            }
            if (string.IsNullOrEmpty(property.StartMeetingUrl) is not true)
            {
                this.logger.StartMeetingUrlSending(reference, property.StartMeetingUrl);
                var bytes = await this.qrCodeService.CreateAsync(property.StartMeetingUrl);
                var base64 = Convert.ToBase64String(bytes);
                var activity = Activity.CreateMessageActivity();
                activity.From = reference.Bot;
                activity.Recipient = reference.User;
                activity.Conversation = reference.Conversation;
                activity.Text = property.StartMeetingUrl;
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
            property.StartMeetingSended = true;
            // await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

    }

}
