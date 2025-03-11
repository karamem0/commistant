//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Bots.Tests;

[Category("Karamem0.Commistant.Bots")]
public class ActivityBotTests
{

    [Test()]
    public async Task OnMembersAddedAsync_ShouldSucceed()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var dislogSet = Substitute.For<DialogSet>();
        var openAIService = Substitute.For<IOpenAIService>();
        var logger = Substitute.For<ILogger<ActivityBot>>();
        var adapter = new TestAdapter();
        var activity = new Activity()
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
        };
        var turnContext = new TurnContext(adapter, activity);
        // Execute
        var target = new ActivityBot(
            conversationState,
            dislogSet,
            openAIService,
            logger
        );
        await target.OnTurnAsync(turnContext);
        // Validate
        Assert.That(adapter.ActiveQueue, Has.Count.EqualTo(1));
    }

    [Test()]
    public async Task OnMembersRemovedAsync_ShouldSucceed()
    {
        // Setup
        var conversationState = Substitute.For<ConversationState>(new MemoryStorage());
        var dislogSet = Substitute.For<DialogSet>();
        var openAIService = Substitute.For<IOpenAIService>();
        var logger = Substitute.For<ILogger<ActivityBot>>();
        var adapter = new TestAdapter();
        var activity = new Activity()
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
        };
        var turnContext = new TurnContext(adapter, activity);
        // Execute
        var target = new ActivityBot(
            conversationState,
            dislogSet,
            openAIService,
            logger
        );
        await target.OnTurnAsync(turnContext);
        // Validate
        _ = conversationState
            .Received()
            .DeleteAsync(Arg.Any<ITurnContext>(), Arg.Any<CancellationToken>());
    }

}
