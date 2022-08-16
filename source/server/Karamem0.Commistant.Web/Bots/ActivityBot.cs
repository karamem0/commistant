//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Webs
{

    public class ActivityBot : TeamsActivityHandler
    {

        private readonly ConversationState conversationState;

        private readonly DialogSet dialogSet;

        private readonly ILogger logger;

        public ActivityBot(
            ConversationState conversationState,
            DialogSet dialogSet,
            ILogger<ActivityBot> logger)
        {
            this.conversationState = conversationState;
            this.dialogSet = dialogSet;
            this.logger = logger;
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var dc = await this.dialogSet.CreateContextAsync(turnContext, cancellationToken);
            var command = turnContext.Activity.RemoveRecipientMention();
            if (command is null)
            {
                _ = await dc.ContinueDialogAsync(cancellationToken);
            }
            else
            {
                var participant = await TeamsInfo.GetMeetingParticipantAsync(
                    turnContext,
                    participantId: turnContext.Activity.Recipient.AadObjectId,
                    cancellationToken: cancellationToken);
                if (participant.Meeting.Role == "Organizer")
                {
                    if (dc.ActiveDialog != null)
                    {
                        _ = await dc.EndDialogAsync(cancellationToken: cancellationToken);
                    }
                    _ = command switch
                    {
                        "会議開始後" => await dc.BeginDialogAsync(nameof(StartMeetingDialog), cancellationToken: cancellationToken),
                        "会議終了前" => await dc.BeginDialogAsync(nameof(EndMeetingDialog), cancellationToken: cancellationToken),
                        "会議中" => await dc.BeginDialogAsync(nameof(InMeetingDialog), cancellationToken: cancellationToken),
                        "初期化" => await dc.BeginDialogAsync(nameof(ResetDialog), cancellationToken: cancellationToken),
                        _ => await dc.ContinueDialogAsync(cancellationToken)
                    };
                }
                else
                {
                    _ = await turnContext.SendActivityAsync(
                        "開催者のみが設定を変更できます。",
                        cancellationToken: cancellationToken);
                }
            }
            await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

        protected override async Task OnTeamsMeetingStartAsync(MeetingStartEventDetails meeting, ITurnContext<IEventActivity> turnContext, CancellationToken cancellationToken)
        {
            this.logger.MeetingStarted(turnContext.Activity, meeting.Id);
            var propertyAccessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
            var property = await propertyAccessor.GetAsync(turnContext, () => new(), cancellationToken);
            var meetingInfo = await TeamsInfo.GetMeetingInfoAsync(turnContext);
            property.InMeeting = true;
            property.StartMeetingSended = false;
            property.EndMeetingSended = false;
            property.ScheduledStartTime = meetingInfo.Details.ScheduledStartTime;
            property.ScheduledEndTime = meetingInfo.Details.ScheduledEndTime;
            await propertyAccessor.SetAsync(turnContext, property, cancellationToken: cancellationToken);
            var referenceAccessor = this.conversationState.CreateProperty<ConversationReference>(nameof(ConversationReference));
            await referenceAccessor.SetAsync(turnContext, turnContext.Activity.GetConversationReference(), cancellationToken);
            await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

        protected override async Task OnTeamsMeetingEndAsync(MeetingEndEventDetails meeting, ITurnContext<IEventActivity> turnContext, CancellationToken cancellationToken)
        {
            this.logger.MeetingEnded(turnContext.Activity, meeting.Id);
            var propertyAccessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
            var property = await propertyAccessor.GetAsync(turnContext, () => new(), cancellationToken);
            property.InMeeting = false;
            property.StartMeetingSended = false;
            property.EndMeetingSended = false;
            property.ScheduledStartTime = null;
            property.ScheduledEndTime = null;
            await propertyAccessor.SetAsync(turnContext, property, cancellationToken: cancellationToken);
            var referenceAccessor = this.conversationState.CreateProperty<ConversationReference>(nameof(ConversationReference));
            await referenceAccessor.DeleteAsync(turnContext, cancellationToken);
            await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
        }

    }

}
