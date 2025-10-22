//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
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

namespace Karamem0.Commistant.Bots;

public class TeamsBot(
    ConversationState conversationState,
    DialogSet dialogSet,
    IOpenAIService openAIService,
    ILogger<TeamsBot> logger
) : TeamsActivityHandler
{

    private readonly ConversationState conversationState = conversationState;

    private readonly DialogSet dialogSet = dialogSet;

    private readonly IOpenAIService openAIService = openAIService;

    private readonly ILogger logger = logger;

    public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
    {
        var conversationReferenceAccessor = this.conversationState.CreateProperty<ConversationReference>(nameof(ConversationReference));
        var conversationReference = turnContext.Activity.GetConversationReference();
        await conversationReferenceAccessor.SetAsync(
            turnContext,
            conversationReference,
            cancellationToken
        );
        var commandSettingsAccessor = this.conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var commandSettings = await commandSettingsAccessor.GetAsync(
            turnContext,
            () => new(),
            cancellationToken
        );
        await commandSettingsAccessor.SetAsync(
            turnContext,
            commandSettings,
            cancellationToken: cancellationToken
        );
        await base.OnTurnAsync(turnContext, cancellationToken);
        await this.conversationState.SaveChangesAsync(turnContext, cancellationToken: cancellationToken);
    }

    protected override async Task OnMembersAddedAsync(
        IList<ChannelAccount> membersAdded,
        ITurnContext<IConversationUpdateActivity> turnContext,
        CancellationToken cancellationToken = default
    )
    {
        this.logger.MembersAddedExecuting(conversationId: turnContext.Activity.Conversation.Id);
        foreach (var member in membersAdded)
        {
            if (member.Id == turnContext.Activity.Recipient.Id)
            {
                _ = await turnContext.SendActivityAsync(
                    string.Join(
                        "",
                        [
                            "<b>Commistant にようこそ！</b>",
                            "<br/>",
                            "Commistant は Microsoft Teams 会議によるコミュニティ イベントをサポートするアシスタント ボットです。",
                            "会議の開始時、終了時、または会議中に定型のメッセージ通知を送信します。",
                            "通知にはテキストおよび QR コードつきの URL を添付することができます。"
                        ]
                    ),
                    cancellationToken: cancellationToken
                );
            }
        }
        this.logger.MembersAddedExecuted(conversationId: turnContext.Activity.Conversation.Id);
        await base.OnMembersAddedAsync(
            membersAdded,
            turnContext,
            cancellationToken
        );
    }

    protected override async Task OnMembersRemovedAsync(
        IList<ChannelAccount> membersRemoved,
        ITurnContext<IConversationUpdateActivity> turnContext,
        CancellationToken cancellationToken = default
    )
    {
        this.logger.MembersRemovedExecuting(conversationId: turnContext.Activity.Conversation.Id);
        foreach (var member in membersRemoved)
        {
            if (member.Id == turnContext.Activity.Recipient.Id)
            {
                await this.conversationState.DeleteAsync(turnContext, cancellationToken);
            }
        }
        this.logger.MembersRemovedExecuted(conversationId: turnContext.Activity.Conversation.Id);
        await base.OnMembersRemovedAsync(
            membersRemoved,
            turnContext,
            cancellationToken
        );
    }

    protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken = default)
    {
        var participant = await TeamsInfo.GetMeetingParticipantAsync(
            turnContext,
            participantId: turnContext.Activity.Recipient.AadObjectId,
            cancellationToken: cancellationToken
        );
        if (participant.Meeting.Role != "Organizer")
        {
            _ = await turnContext.SendActivityAsync("開催者のみがコマンドを実行できます。", cancellationToken: cancellationToken);
            return;
        }
        var dialogContext = await this.dialogSet.CreateContextAsync(turnContext, cancellationToken);
        var command = turnContext.Activity.RemoveRecipientMention();
        if (command is null)
        {
            if (dialogContext.ActiveDialog is not null)
            {
                _ = await dialogContext.ContinueDialogAsync(cancellationToken);
            }
            return;
        }
        var commandSettingsAccessor = this.conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var commandSettings = await commandSettingsAccessor.GetAsync(
            turnContext,
            () => new(),
            cancellationToken
        );
        if (commandSettings.MeetingRunning is true)
        {
            _ = await turnContext.SendActivityAsync("会議の実行中は設定を変更できません。", cancellationToken: cancellationToken);
            return;
        }
        if (dialogContext.ActiveDialog is not null)
        {
            _ = await turnContext.SendActivityAsync("新しいコマンドを開始する前に中断されたコマンドを完了させてください。", cancellationToken: cancellationToken);
            return;
        }
        var arguments = await this.openAIService.GetCommandOptionsAsync(command, cancellationToken);
        var result = arguments?.Type switch
        {
            Constants.MeetingStartCommand => await dialogContext.BeginDialogAsync(
                nameof(MeetingStartDialog),
                arguments,
                cancellationToken: cancellationToken
            ),
            Constants.MeetingEndCommand => await dialogContext.BeginDialogAsync(
                nameof(MeetingEndDialog),
                arguments,
                cancellationToken: cancellationToken
            ),
            Constants.MeetingRunCommand => await dialogContext.BeginDialogAsync(
                nameof(MeetingRunDialog),
                arguments,
                cancellationToken: cancellationToken
            ),
            Constants.ResetCommand => await dialogContext.BeginDialogAsync(nameof(ResetDialog), cancellationToken: cancellationToken),
            _ => null,
        };
        if (result is null)
        {
            _ = await turnContext.SendActivityAsync("認識できないコマンドです。", cancellationToken: cancellationToken);
        }
    }

    protected override async Task OnTeamsMeetingStartAsync(
        MeetingStartEventDetails meeting,
        ITurnContext<IEventActivity> turnContext,
        CancellationToken cancellationToken
    )
    {
        this.logger.MeetingStarted(conversationId: turnContext.Activity.Conversation.Id, meetingId: meeting.Id);
        var commandSettingsAccessor = this.conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var commandSettings = await commandSettingsAccessor.GetAsync(
            turnContext,
            () => new(),
            cancellationToken
        );
        var meetingInfo = await TeamsInfo.GetMeetingInfoAsync(turnContext, cancellationToken: cancellationToken);
        commandSettings.MeetingRunning = true;
        commandSettings.MeetingStartSended = false;
        commandSettings.MeetingEndSended = false;
        commandSettings.ScheduledStartTime = meetingInfo.Details.ScheduledStartTime;
        commandSettings.ScheduledEndTime = meetingInfo.Details.ScheduledEndTime;
        await commandSettingsAccessor.SetAsync(
            turnContext,
            commandSettings,
            cancellationToken: cancellationToken
        );
    }

    protected override async Task OnTeamsMeetingEndAsync(
        MeetingEndEventDetails meeting,
        ITurnContext<IEventActivity> turnContext,
        CancellationToken cancellationToken
    )
    {
        this.logger.MeetingEnded(conversationId: turnContext.Activity.Conversation.Id, meetingId: meeting.Id);
        var commandSettingsAccessor = this.conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var commandSettings = await commandSettingsAccessor.GetAsync(
            turnContext,
            () => new(),
            cancellationToken
        );
        commandSettings.MeetingRunning = false;
        commandSettings.MeetingStartSended = false;
        commandSettings.MeetingEndSended = false;
        commandSettings.ScheduledStartTime = null;
        commandSettings.ScheduledEndTime = null;
        await commandSettingsAccessor.SetAsync(
            turnContext,
            commandSettings,
            cancellationToken: cancellationToken
        );
    }

}
