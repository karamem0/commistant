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
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
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

        private readonly ConversationState conversationState;

        private readonly QrCodeService qrCodeService;

        private readonly ILogger logger;

        public EndMeetingCommand(
            ConversationState conversationState,
            QrCodeService qrCodeService,
            ILogger<EndMeetingCommand> logger)
        {
            this.conversationState = conversationState;
            this.qrCodeService = qrCodeService;
            this.logger = logger;
        }

        public override async Task ExecuteAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var accessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
            var property = await accessor.GetAsync(turnContext, () => new(), cancellationToken);
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
            if (string.IsNullOrEmpty(property.EndMeetingMessage) is not true)
            {
                this.logger.EndMeetingMessageSending(property.EndMeetingMessage);
                _ = await turnContext.SendActivityAsync(
                    MessageFactory.Text(property.EndMeetingMessage),
                    cancellationToken: cancellationToken);
            }
            if (string.IsNullOrEmpty(property.EndMeetingUrl) is not true)
            {
                this.logger.EndMeetingUrlSending(property.EndMeetingUrl);
                var bytes = await this.qrCodeService.CreateAsync(property.EndMeetingUrl);
                var base64 = Convert.ToBase64String(bytes);
                var activity = MessageFactory.Text(property.EndMeetingUrl);
                activity.Attachments = new List<Attachment>()
                    {
                        new Attachment()
                        {
                            ContentType = "image/png",
                            ContentUrl = $"data:image/png;base64,{base64}"
                        }
                    };
                _ = await turnContext.SendActivityAsync(
                    activity,
                    cancellationToken: cancellationToken);
            }
            property.EndMeetingSended = true;
            await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

    }

}
