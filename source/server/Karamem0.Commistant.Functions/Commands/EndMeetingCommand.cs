//
// Copyright (c) 2023 karamem0
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

    public class EndMeetingCommand : Command
    {

        private readonly QrCodeService qrCodeService;

        private readonly ILogger logger;

        public EndMeetingCommand(
            ServiceClientCredentials credentials,
            QrCodeService qrCodeService,
            ILogger<EndMeetingCommand> logger) : base(credentials)
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
            var currentTime = DateTime.UtcNow;
            var timeSpan = (DateTime)endTime - currentTime;
            if (timeSpan.TotalMinutes < 0)
            {
                return;
            }
            if ((int)timeSpan.TotalMinutes > property.EndMeetingSchedule)
            {
                return;
            }
            var client = new ConnectorClient(new Uri(reference.ServiceUrl), this.credentials);
            if (string.IsNullOrEmpty(property.EndMeetingMessage) is not true)
            {
                this.logger.EndMeetingMessageSending(reference, property.EndMeetingMessage);
                var activity = Activity.CreateMessageActivity();
                activity.From = reference.Bot;
                activity.Recipient = reference.User;
                activity.Conversation = reference.Conversation;
                activity.Text = property.EndMeetingMessage;
                _ = await client.Conversations.SendToConversationAsync(
                    (Activity)activity,
                    cancellationToken: cancellationToken);
            }
            if (string.IsNullOrEmpty(property.EndMeetingUrl) is not true)
            {
                this.logger.EndMeetingUrlSending(reference, property.EndMeetingUrl);
                var bytes = await this.qrCodeService.CreateAsync(property.EndMeetingUrl);
                var base64 = Convert.ToBase64String(bytes);
                var activity = Activity.CreateMessageActivity();
                activity.From = reference.Bot;
                activity.Recipient = reference.User;
                activity.Conversation = reference.Conversation;
                activity.Text = property.EndMeetingUrl;
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
            property.EndMeetingSended = true;
            // await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

    }

}
