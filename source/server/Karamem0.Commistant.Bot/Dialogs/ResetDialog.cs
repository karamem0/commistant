//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
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

public class ResetDialog(ConversationState conversationState, ILogger<ResetDialog> logger) : ComponentDialog
{

    private readonly ConversationState conversationState = conversationState;

    private readonly ILogger logger = logger;

    protected override async Task OnInitializeAsync(DialogContext dc)
    {
        _ = this.AddDialog(new WaterfallDialog(
            nameof(WaterfallDialog),
            new WaterfallStep[]
            {
                this.BeforeConfirmAsync,
                this.AfterConrifmAsync
            }
        ));
        _ = this.AddDialog(new TextPrompt(nameof(this.BeforeConfirmAsync), AdaptiveCardvalidator.Validate));
        await base.OnInitializeAsync(dc);
    }

    private async Task<DialogTurnResult> BeforeConfirmAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        this.logger.SettingsResetting(stepContext.Context.Activity);
        var card = new AdaptiveCard("1.3")
        {
            Body =
            [
                new AdaptiveTextBlock()
                {
                    Id = "Message",
                    Text = "すべての設定を初期化します。よろしいですか？",
                    Wrap = true
                },
            ],
            Actions =
            [
                new AdaptiveSubmitAction()
                {
                    Id = "Yes",
                    Title = "はい",
                    Data = new
                    {
                        Button = "Yes"
                    }
                },
                new AdaptiveSubmitAction()
                {
                    Id = "No",
                    Title = "いいえ",
                    Data = new
                    {
                        Button = "No"
                    }
                }
            ]
        };
        var activity = MessageFactory.Attachment(new Attachment()
        {
            ContentType = AdaptiveCard.ContentType,
            Content = JsonConvert.DeserializeObject(card.ToJson())
        });
        this.logger.SettingsResetting(stepContext.Context.Activity);
        return await stepContext.PromptAsync(
            nameof(this.BeforeConfirmAsync),
            new PromptOptions()
            {
                Prompt = (Activity)activity
            },
            cancellationToken
        );
    }

    private async Task<DialogTurnResult> AfterConrifmAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        var value = (JObject)stepContext.Context.Activity.Value;
        if (value is null)
        {
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        if (value.Value<string>("Button") == "Yes")
        {
            this.logger.SettingsReseted(stepContext.Context.Activity);
            var accessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
            await accessor.SetAsync(stepContext.Context, new(), cancellationToken);
            _ = await stepContext.Context.SendActivityAsync(
                "設定を初期化しました。",
                cancellationToken: cancellationToken
            );
        }
        if (value.Value<string>("Button") == "No")
        {
            this.logger.SettingsCancelled(stepContext.Context.Activity);
            _ = await stepContext.Context.SendActivityAsync(
                "キャンセルしました。設定は変更されていません。",
                cancellationToken: cancellationToken);
        }
        if (stepContext.Context.Activity.ReplyToId is not null)
        {
            var card = new AdaptiveCard("1.3")
            {
                Body =
                [
                    new AdaptiveFactSet()
                    {
                        Facts =
                        [
                            new()
                            {
                                Title = "応答",
                                Value = new Func<string>(() =>
                                    value.Value<string>("Button") switch
                                    {
                                        "Yes" => "はい",
                                        "No" => "いいえ",
                                        _ => ""
                                    }
                                )()
                            }
                        ]
                    }
                ]
            };
            var activity = MessageFactory.Attachment(new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = JsonConvert.DeserializeObject(card.ToJson())
            });
            activity.Id = stepContext.Context.Activity.ReplyToId;
            _ = await stepContext.Context.UpdateActivityAsync(
                activity,
                cancellationToken: cancellationToken
            );
        }
        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }

}
