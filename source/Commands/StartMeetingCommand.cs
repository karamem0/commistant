//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
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

        private readonly ConversationState conversationState;

        private readonly QrCodeService qrCodeService;

        public StartMeetingCommand(ConversationState conversationState, QrCodeService qrCodeService)
        {
            this.conversationState = conversationState;
            this.qrCodeService = qrCodeService;
        }

        public override async Task ExecuteAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            var accessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
            var property = await accessor.GetAsync(turnContext, () => new(), cancellationToken);
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
            if (string.IsNullOrEmpty(property.StartMeetingMessage) is not true)
            {
                _ = await turnContext.SendActivityAsync(
                    MessageFactory.Text(property.StartMeetingMessage),
                    cancellationToken: cancellationToken);
            }
            if (string.IsNullOrEmpty(property.StartMeetingUrl) is not true)
            {
                var bytes = await this.qrCodeService.CreateAsync(property.StartMeetingUrl);
                var base64 = Convert.ToBase64String(bytes);
                var activity = MessageFactory.Text(property.StartMeetingUrl);
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
            property.StartMeetingSended = true;
            await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

    }

}
