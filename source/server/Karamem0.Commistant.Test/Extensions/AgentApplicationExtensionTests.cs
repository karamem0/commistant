//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Routes.Abstraction;
using Microsoft.Agents.Builder.App;
using Microsoft.Agents.Core.Models;
using Microsoft.Agents.Storage;
using NSubstitute;
using NUnit.Framework;

namespace Karamem0.Commistant.Extensions.Test;

[Category("Karamem0.Commistant.Extensions")]
public class AgentApplicationExtensionTests
{

    [Test()]
    public void OnActivity_Success()
    {
        // Setup
        var options = new AgentApplicationOptions(new MemoryStorage());
        var handler = Substitute.For<IRouteHandler>();
        // Execute
        var target = Substitute.For<AgentApplication>(options);
        _ = AgentApplicationExtension.OnActivity(target, ActivityTypes.Message, handler);
        // Assert
        _ = target.Received().OnActivity(
            ActivityTypes.Message,
            handler.InvokeAsync,
            RouteRank.Unspecified,
            default,
            default
        );
    }

    [Test()]
    public void OnBeforeTurn_Success()
    {
        // Setup
        var options = new AgentApplicationOptions(new MemoryStorage());
        var handler = Substitute.For<ITurnEventHandler>();
        // Execute
        var target = Substitute.For<AgentApplication>(options);
        _ = AgentApplicationExtension.OnBeforeTurn(target, handler);
        // Assert
        _ = target.Received().OnBeforeTurn(handler.InvokeAsync);
    }

    [Test()]
    public void OnConversationUpdate_Success()
    {
        // Setup
        var options = new AgentApplicationOptions(new MemoryStorage());
        var handler = Substitute.For<IRouteHandler>();
        // Execute
        var target = Substitute.For<AgentApplication>(options);
        _ = AgentApplicationExtension.OnConversationUpdate(target, ActivityTypes.ConversationUpdate, handler);
        // Assert
        _ = target.Received().OnConversationUpdate(
            ActivityTypes.ConversationUpdate,
            handler.InvokeAsync,
            RouteRank.Unspecified,
            default,
            default
        );
    }

    [Test()]
    public void OnEvent_Success()
    {
        // Setup
        var options = new AgentApplicationOptions(new MemoryStorage());
        var handler = Substitute.For<IRouteHandler>();
        // Execute
        var target = Substitute.For<AgentApplication>(options);
        _ = AgentApplicationExtension.OnEvent(target, ActivityTypes.Event, handler);
        // Assert
        _ = target.Received().OnEvent(
            ActivityTypes.Event,
            handler.InvokeAsync,
            RouteRank.Unspecified,
            default,
            default
        );
    }

}
