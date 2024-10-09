//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Serialization;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Bots;

public class ActivityBot(
    ConversationState conversationState,
    DialogSet dialogSet,
    OpenAIService openAIService,
    ILogger<ActivityBot> logger
) : TeamsActivityHandler
{

    private readonly ConversationState conversationState = conversationState;

    private readonly DialogSet dialogSet = dialogSet;

    private readonly OpenAIService openAIService = openAIService;

    private readonly ILogger logger = logger;

    public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
    {
        await base.OnTurnAsync(turnContext, cancellationToken);
    }

    protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
    {
        foreach (var member in membersAdded)
        {
            if (member.Id == turnContext.Activity.Recipient.Id)
            {
                var referenceAccessor = this.conversationState.CreateProperty<ConversationReference>(nameof(ConversationReference));
                var reference = turnContext.Activity.GetConversationReference();
                await referenceAccessor.SetAsync(turnContext, reference, cancellationToken);
                await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
                _ = await turnContext.SendActivityAsync(
                    "<b>Commistant にようこそ！</b>" + "<br/>" +
                    "Commistant は Microsoft Teams 会議によるコミュニティ イベントをサポートするアシスタント ボットです。" +
                    "会議の開始時、終了時、または会議中に定型のメッセージ通知を送信します。" +
                    "通知にはテキストおよび QR コードつきの URL を添付することができます。",
                    cancellationToken: cancellationToken
                );
            }
        }
        await base.OnMembersAddedAsync(membersAdded, turnContext, cancellationToken);
    }

    protected override async Task OnMembersRemovedAsync(IList<ChannelAccount> membersRemoved, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
    {
        foreach (var member in membersRemoved)
        {
            if (member.Id == turnContext.Activity.Recipient.Id)
            {
                await this.conversationState.DeleteAsync(turnContext, cancellationToken);
            }
        }
        await base.OnMembersRemovedAsync(membersRemoved, turnContext, cancellationToken);
    }

    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
    {
        var participant = await TeamsInfo.GetMeetingParticipantAsync(
            turnContext,
            participantId: turnContext.Activity.Recipient.AadObjectId,
            cancellationToken: cancellationToken
        );
        if (participant.Meeting.Role == "Organizer")
        {
            var dc = await this.dialogSet.CreateContextAsync(turnContext, cancellationToken);
            var command = turnContext.Activity.RemoveRecipientMention();
            if (command is null)
            {
                if (dc.ActiveDialog is not null)
                {
                    _ = await dc.ContinueDialogAsync(cancellationToken);
                }
            }
            else
            {
                if (dc.ActiveDialog is null)
                {
                    var result = await new Func<Task<DialogTurnResult?>>(async () =>
                    {
                        var message = await this.openAIService.ChatCompletionAsync(command, cancellationToken: cancellationToken);
                        if (message.FinishReason == ChatFinishReason.ToolCalls)
                        {
                            var arguments = message.ToolCalls.Select(item => item.FunctionArguments).First();
                            var content = JsonSerializer.Deserialize<ConversationPropertyArguments>(arguments?.ToString());
                            return content?.Type switch
                            {
                                "会議開始後" => await dc.BeginDialogAsync(nameof(StartMeetingDialog), content, cancellationToken: cancellationToken),
                                "会議終了前" => await dc.BeginDialogAsync(nameof(EndMeetingDialog), content, cancellationToken: cancellationToken),
                                "会議中" => await dc.BeginDialogAsync(nameof(InMeetingDialog), content, cancellationToken: cancellationToken),
                                "初期化" => await dc.BeginDialogAsync(nameof(ResetDialog), cancellationToken: cancellationToken),
                                _ => null,
                            };
                        }
                        else
                        {
                            return null;
                        }
                    })();
                    if (result is null)
                    {
                        _ = await turnContext.SendActivityAsync(
                            "認識できないコマンドです。",
                            cancellationToken: cancellationToken
                        );
                    }
                }
                else
                {
                    _ = await turnContext.SendActivityAsync(
                        "新しいコマンドを開始する前に中断されたコマンドを完了させてください。",
                        cancellationToken: cancellationToken
                    );
                }
            }
        }
        else
        {
            _ = await turnContext.SendActivityAsync(
                "開催者のみがコマンドを実行できます。",
                cancellationToken: cancellationToken
            );
        }
        await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
    }

    protected override async Task OnTeamsMeetingStartAsync(
        MeetingStartEventDetails meeting,
        ITurnContext<IEventActivity> turnContext,
        CancellationToken cancellationToken
    )
    {
        this.logger.MeetingStarted(turnContext.Activity, meeting.Id);
        var propertyAccessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
        var property = await propertyAccessor.GetAsync(turnContext, () => new(), cancellationToken);
        var meetingInfo = await TeamsInfo.GetMeetingInfoAsync(turnContext, cancellationToken: cancellationToken);
        property.InMeeting = true;
        property.StartMeetingSended = false;
        property.EndMeetingSended = false;
        property.ScheduledStartTime = meetingInfo.Details.ScheduledStartTime;
        property.ScheduledEndTime = meetingInfo.Details.ScheduledEndTime;
        await propertyAccessor.SetAsync(turnContext, property, cancellationToken: cancellationToken);
        await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
    }

    protected override async Task OnTeamsMeetingEndAsync(
        MeetingEndEventDetails meeting,
        ITurnContext<IEventActivity> turnContext,
        CancellationToken cancellationToken
    )
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
        await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
    }

}
