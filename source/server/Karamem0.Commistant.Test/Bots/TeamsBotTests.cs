//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;

namespace Karamem0.Commistant.Bots.Tests;

[Category("Karamem0.Commistant.Bots")]
public class TeamsBotTests
{

    [Test()]
    public async Task OnTurnAsync_Success_WhenMembersAdded()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var dislogSet = Substitute.For<DialogSet>();
        var meetingService = Substitute.For<IMeetingService>();
        var openAIService = Substitute.For<IOpenAIService>();
        var logger = Substitute.For<ILogger<TeamsBot>>();
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
        var target = new TeamsBot(
            conversationState,
            dislogSet,
            meetingService,
            openAIService,
            logger
        );
        await target.OnTurnAsync(turnContext);
        // Assert
        var convesationReferenceAccessor = conversationState.CreateProperty<ConversationReference>(nameof(ConversationReference));
        var convesationReference = await convesationReferenceAccessor.GetAsync(turnContext);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(adapter.ActiveQueue, Has.Count.EqualTo(1));
            Assert.That(convesationReference, Is.Not.Null);
        }
    }

    [Test()]
    public async Task OnTurnAsync_Success_WhenMembersRemoved()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var dislogSet = Substitute.For<DialogSet>();
        var meetingService = Substitute.For<IMeetingService>();
        var openAIService = Substitute.For<IOpenAIService>();
        var logger = Substitute.For<ILogger<TeamsBot>>();
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
        var target = new TeamsBot(
            conversationState,
            dislogSet,
            meetingService,
            openAIService,
            logger
        );
        await target.OnTurnAsync(turnContext);
        // Assert
        var convesationReferenceAccessor = conversationState.CreateProperty<ConversationReference>(nameof(ConversationReference));
        var convesationReference = await convesationReferenceAccessor.GetAsync(turnContext);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(adapter.ActiveQueue, Has.Count.EqualTo(0));
            Assert.That(convesationReference, Is.Null);
        }
    }

    [Test()]
    public async Task OnTurnAsync_Success_WhenTeamsMeetingStart()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var dislogSet = Substitute.For<DialogSet>();
        var meetingService = Substitute.For<IMeetingService>();
        _ = meetingService
            .GetMeetingInfoAsync(Arg.Any<ITurnContext>(), default)
            .Returns(
                new MeetingInfo()
                {
                    Details = new MeetingDetails()
                    {
                        ScheduledStartTime = DateTime.Parse("2000-01-01T09:00:00Z"),
                        ScheduledEndTime = DateTime.Parse("2000-01-01T09:30:00Z"),
                    }
                }
            );
        var openAIService = Substitute.For<IOpenAIService>();
        var logger = Substitute.For<ILogger<TeamsBot>>();
        var adapter = new TestAdapter();
        var activity = new Activity()
        {
            ChannelId = Channels.Msteams,
            Type = ActivityTypes.Event,
            Name = "application/vnd.microsoft.meetingStart",
            Conversation = new ConversationAccount()
            {
                Id = "2f6f9ab4-e65f-480e-9e12-d130196afc98",
            },
            Value = JObject.FromObject(
                new MeetingStartEventDetails()
                {
                    Id = "1234567890"
                }
            )
        };
        var turnContext = new TurnContext(adapter, activity);
        // Execute
        var target = new TeamsBot(
            conversationState,
            dislogSet,
            meetingService,
            openAIService,
            logger
        );
        await target.OnTurnAsync(turnContext);
        // Assert
        var commandSettingsAccessor = conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var commandSettings = await commandSettingsAccessor.GetAsync(turnContext);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(adapter.ActiveQueue, Has.Count.EqualTo(0));
            Assert.That(commandSettings.MeetingRunning, Is.True);
            Assert.That(commandSettings.MeetingStartSended, Is.False);
            Assert.That(commandSettings.MeetingEndSended, Is.False);
            Assert.That(commandSettings.ScheduledStartTime, Is.EqualTo(DateTime.Parse("2000-01-01T09:00:00Z")));
            Assert.That(commandSettings.ScheduledEndTime, Is.EqualTo(DateTime.Parse("2000-01-01T09:30:00Z")));
        }
    }

    [Test()]
    public async Task OnTurnAsync_Success_WhenTeamsMeetingEnd()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var dislogSet = Substitute.For<DialogSet>();
        var meetingService = Substitute.For<IMeetingService>();
        var openAIService = Substitute.For<IOpenAIService>();
        var logger = Substitute.For<ILogger<TeamsBot>>();
        var adapter = new TestAdapter();
        var activity = new Activity()
        {
            ChannelId = Channels.Msteams,
            Type = ActivityTypes.Event,
            Name = "application/vnd.microsoft.meetingEnd",
            Conversation = new ConversationAccount()
            {
                Id = "2f6f9ab4-e65f-480e-9e12-d130196afc98",
            },
            Value = JObject.FromObject(
                new MeetingEndEventDetails()
                {
                    Id = "1234567890"
                }
            )
        };
        var turnContext = new TurnContext(adapter, activity);
        // Execute
        var target = new TeamsBot(
            conversationState,
            dislogSet,
            meetingService,
            openAIService,
            logger
        );
        await target.OnTurnAsync(turnContext);
        // Assert
        var commandSettingsAccessor = conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var commandSettings = await commandSettingsAccessor.GetAsync(turnContext);
        using (Assert.EnterMultipleScope())
        {
            Assert.That(adapter.ActiveQueue, Has.Count.EqualTo(0));
            Assert.That(commandSettings.MeetingRunning, Is.False);
            Assert.That(commandSettings.MeetingStartSended, Is.False);
            Assert.That(commandSettings.MeetingEndSended, Is.False);
            Assert.That(commandSettings.ScheduledStartTime, Is.Null);
            Assert.That(commandSettings.ScheduledEndTime, Is.Null);
        }
    }

}
