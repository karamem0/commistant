//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Core.Models;
using Microsoft.Agents.Storage;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Text.Json;

namespace Karamem0.Commistant.Routes.Tests;

[Category("Karamem0.Commistant.Routes")]
public class MeetingEndedRouteHandlerTests
{

    [Test()]
    public async Task InvokeAsync_Success()
    {
        // Setup
        var logger = Substitute.For<ILogger<MeetingEndedRouteHandler>>();
        var conversationState = new ConversationState(new MemoryStorage());
        var turnContext = Substitute.For<ITurnContext>();
        _ = turnContext.Activity.Returns(
            new Activity()
            {
                ChannelId = Channels.Test,
                Type = ActivityTypes.Event,
                Conversation = new ConversationAccount()
                {
                    Id = "2f6f9ab4-e65f-480e-9e12-d130196afc98",
                },
                Value = JsonElement.Parse(
                    """
                    {
                        "id": "1234567890"
                    }
                    """
                )
            }
        );
        _ = turnContext.StackState.Returns(new TurnContextStateCollection());
        var turnState = Substitute.For<ITurnState>();
        // Execute
        await conversationState.LoadAsync(turnContext);
        conversationState.SetValue(
            nameof(CommandSettings),
            new CommandSettings()
            {
                MeetingInProgress = true,
                MeetingStartedSended = true,
                MeetingEndingSended = true,
                ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
                ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z")
            }
        );
        var target = new MeetingEndedRouteHandler(conversationState, logger);
        await target.InvokeAsync(turnContext, turnState);
        // Assert
        var actual = conversationState.GetValue<CommandSettings>(nameof(CommandSettings));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.MeetingInProgress, Is.False);
            Assert.That(actual.MeetingStartedSended, Is.False);
            Assert.That(actual.MeetingEndingSended, Is.False);
            Assert.That(actual.ScheduledStartTime, Is.Null);
            Assert.That(actual.ScheduledEndTime, Is.Null);
        }
    }

}
