//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Services;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Core.Models;
using Microsoft.Agents.Storage;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

namespace Karamem0.Commistant.Routes.Tests;

[Category("Karamem0.Commistant.Routes")]
public class MessageRouteHandlerTests
{

    [Test()]
    public async Task InvokeAsync_Success()
    {
        // Setup
        var dialogService = Substitute.For<IDialogService<MainDialog>>();
        var logger = Substitute.For<ILogger<MessageRouteHandler>>();
        var conversationState = new ConversationState(new MemoryStorage());
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
                MembersAdded =
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
        _ = turnContext.StackState.Returns(new TurnContextStateCollection());
        // Execute
        await conversationState.LoadAsync(turnContext);
        var target = new MessageRouteHandler(
            conversationState,
            dialogService,
            logger
        );
        await target.InvokeAsync(turnContext, turnState);
        // Assert
        _ = await dialogService
            .Received()
            .RunAsync(
                turnContext,
                conversationState,
                default
            );
    }

}
