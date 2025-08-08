//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Mappings;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Mapster;
using MapsterMapper;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
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
public class MeetingEndDialogTests
{

    [Test()]
    public async Task MeetingEndDialog_Success_WhenSubmit()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var qrCodeService = Substitute.For<IQRCodeService>();
        TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
        TypeAdapterConfig.GlobalSettings.Apply(new MeetingEndDialog.MapperConfiguration(qrCodeService));
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        var logger = Substitute.For<ILogger<MeetingEndDialog>>();
        var value = JObject.FromObject(
            new Dictionary<string, object?>()
            {
                ["button"] = "Submit",
                ["schedule"] = "5",
                ["message"] = "Hello world!",
                ["url"] = "https://www.example.com"
            }
        );
        // Execute
        var dialog = new ComponentDialog();
        _ = dialog.AddDialog(
            new WaterfallDialog(
                nameof(WaterfallDialog),
                [
                    new WaterfallStep(async (stepContext, cancellationToken) =>
                        {
                            var options = new CommandOptions()
                            {
                                Type = Constants.MeetingEndCommand,
                                Value = new CommandOptionsValue()
                                {
                                    Enabled = true,
                                    Schedule = 5,
                                    Message = "Hello world!",
                                    Url = "https://www.example.com"
                                }
                            };
                            return await stepContext.BeginDialogAsync(
                                nameof(MeetingEndDialog),
                                options,
                                cancellationToken
                            );
                        }
                    )
                ]
            )
        );
        _ = dialog.AddDialog(
            new MeetingEndDialog(
                conversationState,
                mapper,
                logger
            )
        );
        var client = new DialogTestClient(Channels.Msteams, dialog);
        var activity = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        var actual = await client.SendActivityAsync<IMessageActivity>(
            new Activity(
                ActivityTypes.Message,
                value: value,
                replyToId: activity.Id
            )
        );
        // Assert
        using (Assert.EnterMultipleScope())
        {
            var accessor = conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
            var property = await accessor.GetAsync(client.DialogContext.Context, () => new());
            Assert.That(property.MeetingEndSchedule, Is.EqualTo(5));
            Assert.That(property.MeetingEndMessage, Is.EqualTo("Hello world!"));
            Assert.That(property.MeetingEndUrl, Is.EqualTo("https://www.example.com"));
        }
    }

    [Test()]
    public async Task MeetingEndDialog_Success_WhenCancel()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        var qrCodeService = Substitute.For<IQRCodeService>();
        TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
        TypeAdapterConfig.GlobalSettings.Apply(new MeetingEndDialog.MapperConfiguration(qrCodeService));
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        var logger = Substitute.For<ILogger<MeetingEndDialog>>();
        var value = JObject.FromObject(
            new Dictionary<string, object>()
            {
                ["button"] = "Cancel",
                ["schedule"] = "5",
                ["message"] = "Hello world!",
                ["url"] = "https://www.example.com"
            }
        );
        // Execute
        var dialog = new ComponentDialog();
        _ = dialog.AddDialog(
            new WaterfallDialog(
                nameof(WaterfallDialog),
                [
                    new WaterfallStep(async (stepContext, cancellationToken) =>
                        {
                            var options = new CommandOptions()
                            {
                                Type = Constants.MeetingEndCommand,
                                Value = new CommandOptionsValue()
                                {
                                    Enabled = true,
                                    Schedule = 5,
                                    Message = "Hello world!",
                                    Url = "https://www.example.com"
                                }
                            };
                            return await stepContext.BeginDialogAsync(
                                nameof(MeetingEndDialog),
                                options,
                                cancellationToken
                            );
                        }
                    )
                ]
            )
        );
        _ = dialog.AddDialog(
            new MeetingEndDialog(
                conversationState,
                mapper,
                logger
            )
        );
        var client = new DialogTestClient(Channels.Msteams, dialog);
        var activity = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        var actual = await client.SendActivityAsync<IMessageActivity>(
            new Activity(
                ActivityTypes.Message,
                value: value,
                replyToId: activity.Id
            )
        );
        // Assert
        using (Assert.EnterMultipleScope())
        {
            var accessor = conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
            var property = await accessor.GetAsync(client.DialogContext.Context, () => new());
            Assert.That(property.MeetingEndSchedule, Is.EqualTo(-1));
            Assert.That(property.MeetingEndMessage, Is.Null);
            Assert.That(property.MeetingEndUrl, Is.Null);
        }
    }

}
