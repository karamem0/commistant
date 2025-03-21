//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AutoMapper;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
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
public class EndMeetingDialogTests
{

    [Test()]
    public async Task EndMeetingDialog_WhenSubmit_ShouldSucceed()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var qrCodeService = Substitute.For<IQRCodeService>();
        var mapper = Substitute.For<IMapper>();
        _ = mapper
            .Map(Arg.Any<CommandOptions?>(), Arg.Any<CommandSettings>())
            .Returns(
                new CommandSettings()
                {
                    EndMeetingSchedule = 5,
                    EndMeetingMessage = "Hello world!",
                    EndMeetingUrl = "https://www.example.com"
                }
            );
        var logger = Substitute.For<ILogger<EndMeetingDialog>>();
        var value = JObject.FromObject(
            new Dictionary<string, object>()
            {
                ["Button"] = "Submit",
                ["Schedule"] = "5",
                ["Message"] = "Hello world!",
                ["Url"] = "https://www.example.com"
            }
        );
        // Execute
        var target = new EndMeetingDialog(
            conversationState,
            qrCodeService,
            mapper,
            logger
        );
        var client = new DialogTestClient(Channels.Msteams, target);
        var activity = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        var actual = await client.SendActivityAsync<IMessageActivity>(
            new Activity(
                ActivityTypes.Message,
                value: value,
                replyToId: activity.Id
            )
        );
        // Assert
        var commandSettingsAccessor = conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var commandSettings = await commandSettingsAccessor.GetAsync(client.DialogContext.Context, () => new());
        Assert.Multiple(
            () =>
            {
                Assert.That(commandSettings.EndMeetingSchedule, Is.EqualTo(5));
                Assert.That(commandSettings.EndMeetingMessage, Is.EqualTo("Hello world!"));
                Assert.That(commandSettings.EndMeetingUrl, Is.EqualTo("https://www.example.com"));
            }
        );
    }

    [Test()]
    public async Task EndMeetingDialog_WhenCancel_ShouldSucceed()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var qrCodeService = Substitute.For<IQRCodeService>();
        var mapper = Substitute.For<IMapper>();
        _ = mapper
            .Map(Arg.Any<CommandOptions?>(), Arg.Any<CommandSettings>())
            .Returns(
                new CommandSettings()
                {
                    EndMeetingSchedule = 5,
                    EndMeetingMessage = "Hello world!",
                    EndMeetingUrl = "https://www.example.com"
                }
            );
        var logger = Substitute.For<ILogger<EndMeetingDialog>>();
        var value = JObject.FromObject(
            new Dictionary<string, object>()
            {
                ["Button"] = "Cancel",
                ["Schedule"] = "5",
                ["Message"] = "Hello world!",
                ["Url"] = "https://www.example.com"
            }
        );
        // Execute
        var target = new EndMeetingDialog(
            conversationState,
            qrCodeService,
            mapper,
            logger
        );
        var client = new DialogTestClient(Channels.Msteams, target);
        var activity = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        var actual = await client.SendActivityAsync<IMessageActivity>(
            new Activity(
                ActivityTypes.Message,
                value: value,
                replyToId: activity.Id
            )
        );
        // Assert
        var commandSettingsAccessor = conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var commandSettings = await commandSettingsAccessor.GetAsync(client.DialogContext.Context, () => new());
        Assert.Multiple(
            () =>
            {
                Assert.That(commandSettings.EndMeetingSchedule, Is.EqualTo(-1));
                Assert.That(commandSettings.EndMeetingMessage, Is.Null);
                Assert.That(commandSettings.EndMeetingUrl, Is.Null);
            }
        );
    }

}
