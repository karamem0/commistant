//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Testing;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Validators.Tests;

[Category("Karamem0.Commistant.Validators")]
public class AdaptiveCardValidatorTests
{

    [Test()]
    public async Task AdaptiveCardValidator_Success()
    {
        // Setup
        var dialog = new ComponentDialog();
        _ = dialog.AddDialog(
            new WaterfallDialog(
                nameof(WaterfallDialog),
                [
                    (stepContext, cancellationToken) => stepContext.PromptAsync(
                        nameof(TextPrompt),
                        new PromptOptions()
                        {
                            Prompt = new Activity(ActivityTypes.Message, text: "Please enter a value."),
                            RetryPrompt = new Activity(ActivityTypes.Message, text: "Invalid input. Please try again."),
                            Validations = stepContext.Values
                        },
                        cancellationToken
                    ),
                    (stepContext, cancellationToken) => stepContext.EndDialogAsync(null, cancellationToken)
                ]
            )
        );
        _ = dialog.AddDialog(new TextPrompt(nameof(TextPrompt), AdaptiveCardValidator.Validate));
        // Execute
        var client = new DialogTestClient(Channels.Msteams, dialog);
        _ = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        var activity = await client.SendActivityAsync<IMessageActivity>(
            new Activity(
                ActivityTypes.Message,
                text: null,
                value: JObject.FromObject(new Dictionary<string, object>())
            )
        );
        // Assert
        Assert.That(activity, Is.Null);
    }

    [Test()]
    public async Task AdaptiveCardValidator_Failure_WhenTextIsNotNull()
    {
        // Setup
        var dialog = new ComponentDialog();
        _ = dialog.AddDialog(
            new WaterfallDialog(
                nameof(WaterfallDialog),
                [
                    (stepContext, cancellationToken) => stepContext.PromptAsync(
                        nameof(TextPrompt),
                        new PromptOptions()
                        {
                            Prompt = new Activity(ActivityTypes.Message, text: "Please enter a value."),
                            RetryPrompt = new Activity(ActivityTypes.Message, text: "Invalid input. Please try again."),
                            Validations = stepContext.Values
                        },
                        cancellationToken
                    ),
                    (stepContext, cancellationToken) => stepContext.EndDialogAsync(null, cancellationToken)
                ]
            )
        );
        _ = dialog.AddDialog(new TextPrompt(nameof(TextPrompt), AdaptiveCardValidator.Validate));
        // Execute
        var client = new DialogTestClient(Channels.Msteams, dialog);
        _ = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        var activity = await client.SendActivityAsync<IMessageActivity>(
            new Activity(
                ActivityTypes.Message,
                text: "Some value",
                value: JObject.FromObject(new Dictionary<string, object>())
            )
        );
        // Assert
        Assert.That(activity, Is.Not.Null);
    }

    [Test()]
    public async Task AdaptiveCardValidator_Failure_WhenValueIsNull()
    {
        // Setup
        var dialog = new ComponentDialog();
        _ = dialog.AddDialog(
            new WaterfallDialog(
                nameof(WaterfallDialog),
                [
                    (stepContext, cancellationToken) => stepContext.PromptAsync(
                        nameof(TextPrompt),
                        new PromptOptions()
                        {
                            Prompt = new Activity(ActivityTypes.Message, text: "Please enter a value."),
                            RetryPrompt = new Activity(ActivityTypes.Message, text: "Invalid input. Please try again."),
                            Validations = stepContext.Values
                        },
                        cancellationToken
                    ),
                    (stepContext, cancellationToken) => stepContext.EndDialogAsync(null, cancellationToken)
                ]
            )
        );
        _ = dialog.AddDialog(new TextPrompt(nameof(TextPrompt), AdaptiveCardValidator.Validate));
        // Execute
        var client = new DialogTestClient(Channels.Msteams, dialog);
        _ = await client.SendActivityAsync<IMessageActivity>(new Activity(ActivityTypes.Message));
        var activity = await client.SendActivityAsync<IMessageActivity>(
            new Activity(
                ActivityTypes.Message,
                text: null,
                value: null
            )
        );
        // Assert
        Assert.That(activity, Is.Not.Null);
    }

}
