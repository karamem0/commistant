//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AutoMapper;
using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Mappings;
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
    public async Task EndMeetingDialog_Submit_Succeeded()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var accessor = conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
        var qrCodeService = Substitute.For<IQRCodeService>();
        var mapperConfig = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>());
        var mapper = mapperConfig.CreateMapper();
        var logger = Substitute.For<ILogger<EndMeetingDialog>>();
        var value = JObject.FromObject(new Dictionary<string, object>()
        {
            ["Button"] = "Submit",
            ["Schedule"]  = "5",
            ["Message"] = "Hello world!",
            ["Url"] = "https://www.example.com"
        });
        // Execute
        var dialog = new EndMeetingDialog(
            conversationState,
            qrCodeService,
            mapper,
            logger
        );
        var client = new DialogTestClient(Channels.Msteams, dialog);
        var activity = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        var actual = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message, value: value, replyToId: activity.Id));
        // Assert
        var property = await accessor.GetAsync(client.DialogContext.Context, () => new());
        Assert.Multiple(() =>
        {
            Assert.That(property.EndMeetingSchedule, Is.EqualTo(5));
            Assert.That(property.EndMeetingMessage, Is.EqualTo("Hello world!"));
            Assert.That(property.EndMeetingUrl, Is.EqualTo("https://www.example.com"));
        });
    }

    [Test()]
    public async Task EndMeetingDialog_Cancel_Succeeded()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var accessor = conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
        var qrCodeService = Substitute.For<IQRCodeService>();
        var mapperConfig = new MapperConfiguration(config => config.AddProfile<AutoMapperProfile>());
        var mapper = mapperConfig.CreateMapper();
        var logger = Substitute.For<ILogger<EndMeetingDialog>>();
        var value = JObject.FromObject(new Dictionary<string, object>()
        {
            ["Button"] = "Cancel",
            ["Schedule"]  = "5",
            ["Message"] = "Hello world!",
            ["Url"] = "https://www.example.com"
        });
        // Execute
        var dialog = new EndMeetingDialog(
            conversationState,
            qrCodeService,
            mapper,
            logger
        );
        var client = new DialogTestClient(Channels.Msteams, dialog);
        var activity = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        var actual = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message, value: value, replyToId: activity.Id));
        // Assert
        var property = await accessor.GetAsync(client.DialogContext.Context, () => new());
        Assert.Multiple(() =>
        {
            Assert.That(property.EndMeetingSchedule, Is.EqualTo(-1));
            Assert.That(property.EndMeetingMessage, Is.Null);
            Assert.That(property.EndMeetingUrl, Is.Null);
        });
    }

}