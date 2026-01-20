//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Extensions;
using Karamem0.Commistant.Routes;
using Microsoft.Agents.Builder.App;
using Microsoft.Agents.Core.Models;

namespace Karamem0.Commistant.Agents;

public class TeamsAgentApplication : AgentApplication
{

    public TeamsAgentApplication(
        AgentApplicationOptions options,
        BeforeTurnRouteHandler beforeTurnRouteHandler,
        MemberAddedRouteHandler memberAddedRouteHandler,
        MemberRemovedRouteHandler memberRemovedRouteHandler,
        MessageRouteHandler messageRouteHandler,
        MeetingStartedRouteHandler meetingStartedEventHandler,
        MeetingEndedRouteHandler meetingEndingEventHandler
    )
        : base(options)
    {
        _ = this.OnBeforeTurn(beforeTurnRouteHandler);
        _ = this.OnConversationUpdate(
            ConversationUpdateEvents.MembersAdded,
            memberAddedRouteHandler,
            RouteRank.Last
        );
        _ = this.OnConversationUpdate(
            ConversationUpdateEvents.MembersRemoved,
            memberRemovedRouteHandler,
            RouteRank.Last
        );
        _ = this.OnActivity(
            ActivityTypes.Message,
            messageRouteHandler,
            RouteRank.Last
        );
        _ = this.OnEvent(
            "application/vnd.microsoft.meetingStart",
            meetingStartedEventHandler,
            RouteRank.Last
        );
        _ = this.OnEvent(
            "application/vnd.microsoft.meetingEnd",
            meetingEndingEventHandler,
            RouteRank.Last
        );
    }

}
