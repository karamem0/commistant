//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Routes.Abstraction;
using Microsoft.Agents.Builder.App;

namespace Karamem0.Commistant.Extensions;

public static class AgentApplicationExtensions
{

    public static AgentApplication OnActivity(
        this AgentApplication target,
        string eventName,
        IRouteHandler handler,
        ushort rank = RouteRank.Unspecified
    )
    {
        return target.OnActivity(
            eventName,
            handler.InvokeAsync,
            rank
        );
    }

    public static AgentApplication OnBeforeTurn(this AgentApplication target, ITurnEventHandler handler)
    {
        return target.OnBeforeTurn(handler.InvokeAsync);
    }

    public static AgentApplication OnConversationUpdate(
        this AgentApplication target,
        string eventName,
        IRouteHandler handler,
        ushort rank = RouteRank.Unspecified
    )
    {
        return target.OnConversationUpdate(
            eventName,
            handler.InvokeAsync,
            rank
        );
    }

    public static AgentApplication OnEvent(
        this AgentApplication target,
        string eventName,
        IRouteHandler handler,
        ushort rank = RouteRank.Unspecified
    )
    {
        return target.OnEvent(
            eventName,
            handler.InvokeAsync,
            rank
        );
    }

}
