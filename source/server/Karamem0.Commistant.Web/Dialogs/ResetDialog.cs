//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using Karamem0.Commistant.Extensions;
using Karamem0.Commistant.Helpers;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Validators;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Dialogs;

public class ResetDialog(ConversationState conversationState, ILogger<ResetDialog> logger) : ComponentDialog
{

    private readonly ConversationState conversationState = conversationState;

    private readonly ILogger logger = logger;

    protected override async Task OnInitializeAsync(DialogContext dialogContext)
    {
        _ = this.AddDialog(
            new WaterfallDialog(
                nameof(WaterfallDialog),
                [
                    this.OnBeforeAsync,
                    this.OnAfterAsync
                ]
            )
        );
        _ = this.AddDialog(new TextPrompt(nameof(this.OnBeforeAsync), AdaptiveCardValidator.Validate));
        await base.OnInitializeAsync(dialogContext);
    }

    private async Task<DialogTurnResult> OnBeforeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
    {
        var card = await AdaptiveCardHelper.CreateEditCardAsync("Reset");
        var activity = MessageFactory.Attachment(
            new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            }
        );
        this.logger.SettingsResetting(conversationId: stepContext.Context.Activity.Id);
        return await stepContext.PromptAsync(
            nameof(this.OnBeforeAsync),
            new PromptOptions()
            {
                Prompt = (Activity)activity
            },
            cancellationToken
        );
    }

    private async Task<DialogTurnResult> OnAfterAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
    {
        var value = (JObject)stepContext.Context.Activity.Value;
        if (value is null)
        {
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        if (value.Value<string>("Button") == "Yes")
        {
            var accessor = this.conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
            await accessor.SetAsync(
                stepContext.Context,
                new(),
                cancellationToken
            );
            this.logger.SettingsReseted(conversationId: stepContext.Context.Activity.Id);
            _ = await stepContext.Context.SendSettingsResetedAsync(cancellationToken);
        }
        if (value.Value<string>("Button") == "No")
        {
            this.logger.SettingsCancelled(conversationId: stepContext.Context.Activity.Id);
            _ = await stepContext.Context.SendSettingsCancelledAsync(cancellationToken);
        }
        if (stepContext.Context.Activity.ReplyToId is not null)
        {
            var data = new AdaptiveCardResponseData()
            {
                Value = value.Value<string>("Button") switch
                {
                    "Yes" => "はい",
                    "No" => "いいえ",
                    _ => ""
                }
            };
            var card = await AdaptiveCardHelper.CreateViewCardAsync("Reset", data);
            var activity = MessageFactory.Attachment(
                new Attachment()
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                }
            );
            activity.Id = stepContext.Context.Activity.ReplyToId;
            _ = await stepContext.Context.UpdateActivityAsync(activity, cancellationToken);
        }
        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }

}
