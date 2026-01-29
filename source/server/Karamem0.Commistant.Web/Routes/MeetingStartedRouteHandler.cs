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
using Karamem0.Commistant.Services;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Extensions.Teams.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;

namespace Karamem0.Commistant.Routes;

public class MeetingStartedRouteHandler(
    ConversationState conversationState,
    IMeetingService meetingService,
    ILogger<MeetingStartedRouteHandler> logger
) : RouteHandler<MeetingStartedRouteHandler>(logger)
{

    private readonly ConversationState conversationState = conversationState;

    private readonly IMeetingService meetingService = meetingService;

    private readonly ILogger<MeetingStartedRouteHandler> logger = logger;

    protected override async Task InvokeAsyncCore(
        ITurnContext turnContext,
        ITurnState turnState,
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

}
