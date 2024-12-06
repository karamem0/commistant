//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using Karamem0.Commistant.Extensions;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Karamem0.Commistant.Validators;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Dialogs;

public class ResetSettingsDialog(
    ConversationState conversationState,
    IAdaptiveCardService adaptiveCardService,
    ILogger<ResetSettingsDialog> logger
) : ComponentDialog
{

    private readonly ConversationState conversationState = conversationState;

    private readonly IAdaptiveCardService adaptiveCardService = adaptiveCardService;

    private readonly ILogger logger = logger;

    protected override async Task OnInitializeAsync(DialogContext dc)
    {
        _ = this.AddDialog(new WaterfallDialog(
            nameof(WaterfallDialog),
            [
                this.OnBeforeAsync,
                this.OnAfterAsync
            ]
        ));
        _ = this.AddDialog(new TextPrompt(nameof(this.OnBeforeAsync), AdaptiveCardvalidator.Validate));
        await base.OnInitializeAsync(dc);
    }

    private async Task<DialogTurnResult> OnBeforeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        this.logger.SettingsResetting(stepContext.Context.Activity);
        var card = await this.adaptiveCardService.GetCardAsync(
            AdaptiveCardTemplateTypes.ResetOnBefore,
            null,
            cancellationToken
        );
        var activity = MessageFactory.Attachment(new Attachment()
        {
            ContentType = "application/vnd.microsoft.card.adaptive",
            Content = JsonConvert.DeserializeObject(card)
        });
        this.logger.SettingsResetting(stepContext.Context.Activity);
        return await stepContext.PromptAsync(
            nameof(this.OnBeforeAsync),
            new PromptOptions()
            {
                Prompt = (Activity)activity
            },
            cancellationToken
        );
    }

    private async Task<DialogTurnResult> OnAfterAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        var value = (JObject)stepContext.Context.Activity.Value;
        if (value is null)
        {
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        if (value.Value<string>("Button") == "Yes")
        {
            var accessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
            await accessor.SetAsync(stepContext.Context, new(), cancellationToken);
            this.logger.SettingsReseted(stepContext.Context.Activity);
            _ = await stepContext.Context.SendSettingsResetedAsync(cancellationToken);
        }
        if (value.Value<string>("Button") == "No")
        {
            this.logger.SettingsCancelled(stepContext.Context.Activity);
            _ = await stepContext.Context.SendSettingsCancelledAsync(cancellationToken);
        }
        if (stepContext.Context.Activity.ReplyToId is not null)
        {
            var card = await this.adaptiveCardService.GetCardAsync(
                AdaptiveCardTemplateTypes.ResetOnAfter,
                null,
                cancellationToken
            );
            var activity = MessageFactory.Attachment(new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(card)
            });
            activity.Id = stepContext.Context.Activity.ReplyToId;
            _ = await stepContext.Context.UpdateActivityAsync(activity, cancellationToken);
        }
        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }

}
