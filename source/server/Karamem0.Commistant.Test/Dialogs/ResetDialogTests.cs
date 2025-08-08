//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Mappings;
using Karamem0.Commistant.Models;
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
public class ResetDialogTests
{

    [Test()]
    public async Task ResetDialog_Success_WhenYes()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
        TypeAdapterConfig.GlobalSettings.Apply(new ResetDialog.MapperConfiguration());
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        var logger = Substitute.For<ILogger<ResetDialog>>();
        var value = JObject.FromObject(
            new Dictionary<string, object>()
            {
                ["button"] = "Yes"
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
                            var accessor = conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
                            await accessor.SetAsync(
                                stepContext.Context,
                                new CommandSettings()
                                {
                                    MeetingStartSchedule = 5,
                                    MeetingEndSchedule = 5,
                                    MeetingRunSchedule = 5
                                },
                                cancellationToken
                            );
                            return await stepContext.BeginDialogAsync(
                                nameof(ResetDialog),
                                null,
                                cancellationToken
                            );
                        }
                    )
                ]
            )
        );
        _ = dialog.AddDialog(
            new ResetDialog(
                conversationState,
                mapper,
                logger
            )
        );
        var client = new DialogTestClient(Channels.Msteams, dialog);
        var activity = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        await conversationState.SaveChangesAsync(client.DialogContext.Context);
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
            Assert.That(property.MeetingStartSchedule, Is.EqualTo(-1));
            Assert.That(property.MeetingEndSchedule, Is.EqualTo(-1));
            Assert.That(property.MeetingRunSchedule, Is.EqualTo(-1));
        }
    }

    [Test()]
    public async Task ResetDialog_Success_WhenNo()
    {
        // Setup
        var conversationState = new ConversationState(new MemoryStorage());
        TypeAdapterConfig.GlobalSettings.Apply(new MapperConfiguration());
        TypeAdapterConfig.GlobalSettings.Apply(new ResetDialog.MapperConfiguration());
        var mapper = new Mapper(TypeAdapterConfig.GlobalSettings);
        var logger = Substitute.For<ILogger<ResetDialog>>();
        var value = JObject.FromObject(
            new Dictionary<string, object>()
            {
                ["button"] = "No"
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
                            var accessor = conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
                            await accessor.SetAsync(
                                stepContext.Context,
                                new CommandSettings()
                                {
                                    MeetingStartSchedule = 5,
                                    MeetingEndSchedule = 5,
                                    MeetingRunSchedule = 5
                                },
                                cancellationToken
                            );
                            return await stepContext.BeginDialogAsync(
                                nameof(ResetDialog),
                                null,
                                cancellationToken
                            );
                        }
                    )
                ]
            )
        );
        _ = dialog.AddDialog(
            new ResetDialog(
                conversationState,
                mapper,
                logger
            )
        );
        var client = new DialogTestClient(Channels.Msteams, dialog);
        var activity = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        await conversationState.SaveChangesAsync(client.DialogContext.Context);
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
            Assert.That(property.MeetingStartSchedule, Is.EqualTo(5));
            Assert.That(property.MeetingEndSchedule, Is.EqualTo(5));
            Assert.That(property.MeetingRunSchedule, Is.EqualTo(5));
        }
    }

}
