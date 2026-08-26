//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.App;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Core.Models;
using Microsoft.Agents.Extensions.Teams.Models;
using Microsoft.Agents.Storage;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Text.Json;

namespace Karamem0.Commistant.Agents.Tests;

[Category("Karamem0.Commistant.Agents")]
public class WebAgentApplicationTests
{

    [Test()]
    public async Task OnMeetingStartedAsync_Success()
    {
        // Setup
        var storage = new MemoryStorage();
        var conversationState = new ConversationState(storage);
        var dialogService = Substitute.For<IDialogService<MainDialog>>();
        var meetingService = Substitute.For<IMeetingService>();
        _ = meetingService
            .GetMeetingInfoAsync(Arg.Any<ITurnContext>(), default)
            .Returns(
                new MeetingInfo()
                {
                    Details = new MeetingDetails()
                    {
                        ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
                        ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z")
                    }
                }
            );
        var logger = Substitute.For<ILogger<WebAgentApplication>>();
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
        var target = new WebAgentApplication(
            new AgentApplicationOptions(storage),
            conversationState,
            dialogService,
            meetingService,
            logger
        );
        // Execute
        await conversationState.LoadAsync(turnContext);
        await target.OnMeetingStartedAsync(turnContext, turnState);
        // Assert
        var actual = conversationState.GetValue<CommandSettings>(nameof(CommandSettings));
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual.MeetingInProgress, Is.True);
            Assert.That(actual.MeetingStartedSended, Is.False);
            Assert.That(actual.MeetingEndingSended, Is.False);
            Assert.That(actual.ScheduledStartTime, Is.EqualTo(DateTime.Parse("2000-01-01T09:00:00Z")));
            Assert.That(actual.ScheduledEndTime, Is.EqualTo(DateTime.Parse("2000-01-01T09:30:00Z")));
        }
    }

    [Test()]
    public async Task OnMeetingEndedAsync_Success()
    {
        // Setup
        var logger = Substitute.For<ILogger<WebAgentApplication>>();
        var storage = new MemoryStorage();
        var conversationState = new ConversationState(storage);
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
        var target = new WebAgentApplication(
            new AgentApplicationOptions(storage),
            conversationState,
            Substitute.For<IDialogService<MainDialog>>(),
            Substitute.For<IMeetingService>(),
            logger
        );
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
        await target.OnMeetingEndedAsync(turnContext, turnState);
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

    [Test()]
    public async Task OnMembersAddedAsync_Success()
    {
        // Setup
        var logger = Substitute.For<ILogger<WebAgentApplication>>();
        var storage = new MemoryStorage();
        var conversationState = new ConversationState(storage);
        var dialogService = Substitute.For<IDialogService<MainDialog>>();
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
                },
                MembersAdded =
                [
                    new ChannelAccount()
                    {
                        Id = "48d31887-5fad-4d73-a9f5-3c356e68a038"
                    }
                ]
            }
        );
        _ = turnContext.StackState.Returns(new TurnContextStateCollection());
        var turnState = Substitute.For<ITurnState>();
        var target = new WebAgentApplication(
            new AgentApplicationOptions(storage),
            conversationState,
            dialogService,
            Substitute.For<IMeetingService>(),
            logger
        );
        // Execute
        await conversationState.LoadAsync(turnContext);
        await target.OnMembersAddedAsync(turnContext, turnState);
        // Assert
        _ = await dialogService
            .Received()
            .RunAsync(
                turnContext,
                conversationState,
                default
            );
    }

    [Test()]
    public async Task OnMembersRemovedAsync_Success()
    {
        // Setup
        var logger = Substitute.For<ILogger<WebAgentApplication>>();
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
                    Id = "48d31887-5fad-4d73-a9f5-3c356e68a038"
                },
                MembersRemoved =
                [
                    new ChannelAccount()
                    {
                        Id = "48d31887-5fad-4d73-a9f5-3c356e68a038"
                    }
                ]
            }
        );
        var turnState = Substitute.For<ITurnState>();
        var target = new WebAgentApplication(
            new AgentApplicationOptions(storage),
            conversationState,
            Substitute.For<IDialogService<MainDialog>>(),
            Substitute.For<IMeetingService>(),
            logger
        );
        // Execute
        await target.OnMembersRemovedAsync(turnContext, turnState);
        // Assert
        await storage
            .Received()
            .DeleteAsync(Arg.Any<string[]>(), default);
    }

    [Test()]
    public async Task OnMessageAsync_Success()
    {
        // Setup
        var logger = Substitute.For<ILogger<WebAgentApplication>>();
        var storage = new MemoryStorage();
        var conversationState = new ConversationState(storage);
        var dialogService = Substitute.For<IDialogService<MainDialog>>();
        var turnContext = Substitute.For<ITurnContext>();
        _ = turnContext.Activity.Returns(
            new Activity()
            {
                ChannelId = Channels.Test,
                Type = ActivityTypes.Message,
                Conversation = new ConversationAccount()
                {
                    Id = "2f6f9ab4-e65f-480e-9e12-d130196afc98",
                }
            }
        );
        var turnState = Substitute.For<ITurnState>();
        var target = new WebAgentApplication(
            new AgentApplicationOptions(storage),
            conversationState,
            dialogService,
            Substitute.For<IMeetingService>(),
            logger
        );
        // Execute
        await target.OnMessageAsync(turnContext, turnState);
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
