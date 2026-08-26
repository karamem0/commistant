//
// Copyright (c) 2022-2026 karamem0
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
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.App;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Core.Models;
using Microsoft.Agents.Extensions.Teams.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;

namespace Karamem0.Commistant.Agents;

public class WebAgentApplication(
    AgentApplicationOptions options,
    ConversationState conversationState,
    IDialogService<MainDialog> dialogService,
    IMeetingService meetingService,
    ILogger<WebAgentApplication> logger
) : AgentApplication(options)
{

    private readonly ConversationState conversationState = conversationState;

    private readonly IDialogService<MainDialog> dialogService = dialogService;

    private readonly IMeetingService meetingService = meetingService;

    private readonly ILogger<WebAgentApplication> logger = logger;

    [MembersAddedRoute()]
    public async Task OnMembersAddedAsync(
        ITurnContext turnContext,
        ITurnState _0,
        CancellationToken cancellationToken = default
    )
    {
        this.logger.MembersAdded(conversationId: turnContext.Activity.Conversation.Id);
        foreach (var member in turnContext.Activity.MembersAdded)
        {
            if (member.Id == turnContext.Activity.Recipient.Id)
            {
                _ = await turnContext.SendActivityAsync(
                    """
                    <b>Commistant にようこそ！</b>
                    <br/>
                    Commistant は Microsoft Teams 会議によるコミュニティ イベントをサポートする AI エージェントです。
                    会議の開始時、終了時、または会議中に定型のメッセージ通知を送信します。
                    通知にはテキストおよび QR コードつきの URL を添付することができます。
                    """,
                    cancellationToken: cancellationToken
                );
                this.conversationState.SetValue(nameof(ConversationReference), turnContext.Activity.GetConversationReference());
                this.conversationState.SetValue(
                    nameof(CommandSettings),
                    this.conversationState.GetValue<CommandSettings>(nameof(CommandSettings), () => new())
                );
            }
            _ = await this.dialogService.RunAsync(
                turnContext,
                this.conversationState,
                cancellationToken
            );
        }
    }

    [MembersRemovedRoute()]
    public async Task OnMembersRemovedAsync(
        ITurnContext turnContext,
        ITurnState _0,
        CancellationToken cancellationToken = default
    )
    {
        this.logger.MembersRemoved(conversationId: turnContext.Activity.Conversation.Id);
        foreach (var member in turnContext.Activity.MembersRemoved)
        {
            if (member.Id == turnContext.Activity.Recipient.Id)
            {
                await this.conversationState.DeleteStateAsync(turnContext, cancellationToken);
            }
        }
    }

    [MessageRoute()]
    public async Task OnMessageAsync(
        ITurnContext turnContext,
        ITurnState _0,
        CancellationToken cancellationToken = default
    )
    {
        this.logger.MessageReceived(conversationId: turnContext.Activity.Conversation.Id);
        _ = await this.dialogService.RunAsync(
            turnContext,
            this.conversationState,
            cancellationToken
        );
    }

    [EventRoute("application/vnd.microsoft.meetingStart")]
    public async Task OnMeetingStartedAsync(
        ITurnContext turnContext,
        ITurnState _0,
        CancellationToken cancellationToken = default
    )
    {
        var value = (JsonElement)turnContext.Activity.Value;
        var meeting = JsonConverter.Deserialize<MeetingStartEventDetails>(value);
        _ = meeting ?? throw new InvalidOperationException($"{nameof(MeetingStartEventDetails)} を null にはできません");
        this.logger.MeetingStarted(conversationId: turnContext.Activity.Conversation.Id, meetingId: meeting.Id);
        var commandSettings = this.conversationState.GetValue<CommandSettings>(nameof(CommandSettings), () => new());
        var meetingInfo = await this.meetingService.GetMeetingInfoAsync(turnContext, cancellationToken: cancellationToken);
        commandSettings.MeetingInProgress = true;
        commandSettings.MeetingStartedSended = false;
        commandSettings.MeetingEndingSended = false;
        commandSettings.ScheduledStartTime = meetingInfo.Details.ScheduledStartTime;
        commandSettings.ScheduledEndTime = meetingInfo.Details.ScheduledEndTime;
        this.conversationState.SetValue(nameof(CommandSettings), commandSettings);
    }

    [EventRoute("application/vnd.microsoft.meetingEnd")]
    public async Task OnMeetingEndedAsync(
        ITurnContext turnContext,
        ITurnState _0,
        CancellationToken _1 = default
    )
    {
        var value = (JsonElement)turnContext.Activity.Value;
        var meeting = JsonConverter.Deserialize<MeetingEndEventDetails>(value);
        _ = meeting ?? throw new InvalidOperationException($"{nameof(MeetingEndEventDetails)} を null にはできません");
        this.logger.MeetingEnded(conversationId: turnContext.Activity.Conversation.Id, meetingId: meeting.Id);
        var commandSettings = this.conversationState.GetValue<CommandSettings>(nameof(CommandSettings), () => new());
        commandSettings.MeetingInProgress = false;
        commandSettings.MeetingStartedSended = false;
        commandSettings.MeetingEndingSended = false;
        commandSettings.ScheduledStartTime = null;
        commandSettings.ScheduledEndTime = null;
        this.conversationState.SetValue(nameof(CommandSettings), commandSettings);
    }

}
