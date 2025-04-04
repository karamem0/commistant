//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Testing;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Dialogs.Tests;

[Category("Karamem0.Commistant.Dialogs")]
public class ResetDialogTests
{

    [Test()]
    public async Task ResetDialog_Success_WhenYes()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var commandSettingsAccessor = conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var logger = Substitute.For<ILogger<ResetDialog>>();
        var value = JObject.FromObject(
            new Dictionary<string, object>()
            {
                ["Button"] = "Yes"
            }
        );
        // Execute
        var target = new ResetDialog(conversationState, logger);
        var client = new DialogTestClient(
            Channels.Msteams,
            target,
            conversationState: conversationState
        );
        var activity = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        await commandSettingsAccessor.SetAsync(
            client.DialogContext.Context,
            new CommandSettings()
            {
                StartMeetingSchedule = 5,
                EndMeetingSchedule = 5,
                InMeetingSchedule = 5
            }
        );
        await conversationState.SaveChangesAsync(client.DialogContext.Context);
        var actual = await client.SendActivityAsync<IMessageActivity>(
            new Activity(
                ActivityTypes.Message,
                value: value,
                replyToId: activity.Id
            )
        );
        // Assert
        var commandSettings = await commandSettingsAccessor.GetAsync(client.DialogContext.Context, () => new());
        Assert.Multiple(
            () =>
            {
                Assert.That(commandSettings.StartMeetingSchedule, Is.EqualTo(-1));
                Assert.That(commandSettings.EndMeetingSchedule, Is.EqualTo(-1));
                Assert.That(commandSettings.InMeetingSchedule, Is.EqualTo(-1));
            }
        );
    }

    [Test()]
    public async Task ResetDialog_Success_WhenNo()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var commandSettingsAccessor = conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var logger = Substitute.For<ILogger<ResetDialog>>();
        var value = JObject.FromObject(
            new Dictionary<string, object>()
            {
                ["Button"] = "No"
            }
        );
        // Execute
        var target = new ResetDialog(conversationState, logger);
        var client = new DialogTestClient(
            Channels.Msteams,
            target,
            conversationState: conversationState
        );
        var activity = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        await commandSettingsAccessor.SetAsync(
            client.DialogContext.Context,
            new CommandSettings()
            {
                StartMeetingSchedule = 5,
                EndMeetingSchedule = 5,
                InMeetingSchedule = 5
            }
        );
        await conversationState.SaveChangesAsync(client.DialogContext.Context);
        var actual = await client.SendActivityAsync<IMessageActivity>(
            new Activity(
                ActivityTypes.Message,
                value: value,
                replyToId: activity.Id
            )
        );
        // Assert
        var commandSettings = await commandSettingsAccessor.GetAsync(client.DialogContext.Context, () => new());
        Assert.Multiple(
            () =>
            {
                Assert.That(commandSettings.StartMeetingSchedule, Is.EqualTo(5));
                Assert.That(commandSettings.EndMeetingSchedule, Is.EqualTo(5));
                Assert.That(commandSettings.InMeetingSchedule, Is.EqualTo(5));
            }
        );
    }

}
