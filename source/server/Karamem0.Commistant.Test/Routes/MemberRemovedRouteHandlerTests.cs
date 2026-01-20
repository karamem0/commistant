//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Core.Models;
using Microsoft.Agents.Storage;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Karamem0.Commistant.Routes.Tests;

[Category("Karamem0.Commistant.Routes")]
public class MemberRemovedRouteHandlerTests
{

    [Test()]
    public async Task InvokeAsync_Success()
    {
        // Setup
        var logger = Substitute.For<ILogger<MemberRemovedRouteHandler>>();
        var storage = Substitute.For<IStorage>();
        var conversationState = new ConversationState(storage);
        var turnContext = Substitute.For<ITurnContext>();
        _ = turnContext.Activity.Returns(
            new Activity()
            {
                ChannelId = Channels.Test,
                Type = ActivityTypes.ConversationUpdate,
                Conversation = new ConversationAccount()
                {
                    Id = "2f6f9ab4-e65f-480e-9e12-d130196afc98",
                },
                Recipient = new ChannelAccount()
                {
                    Id = "48d31887-5fad-4d73-a9f5-3c356e68a038",
                    Name = "Megan Bowen",
                    Role = "User"
                },
                MembersRemoved =
                [
                    new ChannelAccount()
                    {
                        Id = "dc355a55-e124-44a9-bcc8-e679fd4f706c",
                        Name = "Bot",
                        Role = "Bot"
                    },
                    new ChannelAccount()
                    {
                        Id = "48d31887-5fad-4d73-a9f5-3c356e68a038",
                        Name = "Megan Bowen",
                        Role = "User"
                    }
                ]
            }
        );
        var turnState = Substitute.For<ITurnState>();
        // Execute
        var target = new MemberRemovedRouteHandler(conversationState, logger);
        await target.InvokeAsync(turnContext, turnState);
        // Assert
        await storage
            .Received()
            .DeleteAsync(Arg.Any<string[]>(), default);
    }

}
