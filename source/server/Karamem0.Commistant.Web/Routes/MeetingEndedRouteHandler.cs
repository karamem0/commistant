//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Routes.Abstraction;
using Karamem0.Commistant.Serialization;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Extensions.Teams.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;

namespace Karamem0.Commistant.Routes;

public class MeetingEndedRouteHandler(ConversationState conversationState, ILogger<MeetingEndedRouteHandler> logger)
    : RouteHandler<MeetingEndedRouteHandler>(logger)
{

    private readonly ILogger<MeetingEndedRouteHandler> logger = logger;

    private readonly ConversationState conversationState = conversationState;

    protected override async Task InvokeAsyncCore(
        ITurnContext turnContext,
        ITurnState turnState,
        CancellationToken cancellationToken = default
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
