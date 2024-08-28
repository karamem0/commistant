//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using AutoMapper;
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

public class StartMeetingDialog(
    ConversationState conversationState,
    QrCodeService qrCodeService,
    IMapper mapper,
    ILogger<StartMeetingDialog> logger
) : ComponentDialog
{

    private readonly ConversationState conversationState = conversationState;

    private readonly QrCodeService qrCodeService = qrCodeService;

    private readonly IMapper mapper = mapper;

    private readonly ILogger logger = logger;

    protected override async Task OnInitializeAsync(DialogContext dc)
    {
        _ = this.AddDialog(new WaterfallDialog(
            nameof(WaterfallDialog),
            [
                this.BeforeConfirmAsync,
                this.AfterConrifmAsync
            ]
        ));
        _ = this.AddDialog(new TextPrompt(nameof(this.BeforeConfirmAsync), AdaptiveCardvalidator.Validate));
        await base.OnInitializeAsync(dc);
    }

    private async Task<DialogTurnResult> BeforeConfirmAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        var accessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
        var property = await accessor.GetAsync(stepContext.Context, () => new(), cancellationToken);
        var options = (ConversationPropertyArguments?)stepContext.Options;
        var value = this.mapper.Map(options, property.Clone());
        var card = new AdaptiveCard("1.3")
        {
            Body =
            [
                new AdaptiveChoiceSetInput()
                {
                    Id = "Schedule",
                    Label = "スケジュール",
                    Placeholder = "通知を表示する時間",
                    Choices =
                    [
                        new()
                        {
                            Title = "なし",
                            Value = "-1"
                        },
                        new()
                        {
                            Title = "予定時刻",
                            Value = "0"
                        },
                        new()
                        {
                            Title = "5 分後",
                            Value = "5"
                        },
                        new()
                        {
                            Title = "10 分後",
                            Value = "10"
                        },
                        new()
                        {
                            Title = "15 分後",
                            Value = "15"
                        },
                    ],
                    Value = value.StartMeetingSchedule.ToString()
                },
                new AdaptiveTextInput()
                {
                    Id = "Message",
                    Label = "メッセージ",
                    Placeholder = "開始後に表示されるメッセージ",
                    Style = AdaptiveTextInputStyle.Text,
                    Value = value.StartMeetingMessage
                },
                new AdaptiveTextInput()
                {
                    Id = "Url",
                    Label = "URL",
                    Placeholder = "開始後に表示されるリンクの URL",
                    Style = AdaptiveTextInputStyle.Url,
                    Value = value.StartMeetingUrl
                }
            ],
            Actions =
            [
                new AdaptiveSubmitAction()
                {
                    Id = "Submit",
                    Title = "保存",
                    Data = new
                    {
                        Button = "Submit"
                    }
                },
                new AdaptiveSubmitAction()
                {
                    Id = "Cancel",
                    Title = "キャンセル",
                    Data = new
                    {
                        Button = "Cancel"
                    }
                }
            ]
        };
        var activity = MessageFactory.Attachment(new Attachment()
        {
            ContentType = AdaptiveCard.ContentType,
            Content = JsonConvert.DeserializeObject(card.ToJson())
        });
        this.logger.SettingsUpdating(stepContext.Context.Activity);
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
        var accessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
        var property = await accessor.GetAsync(stepContext.Context, () => new(), cancellationToken);
        if (value.Value<string>("Button") == "Submit")
        {
            property.StartMeetingSchedule = value.Value("Schedule", -1);
            property.StartMeetingMessage = value.Value<string>("Message", null);
            property.StartMeetingUrl = value.Value<string>("Url", null);
            this.logger.SettingsUpdated(stepContext.Context.Activity);
            _ = await stepContext.Context.SendActivityAsync(
                "設定を変更しました。",
                cancellationToken: cancellationToken
            );
        }
        if (value.Value<string>("Button") == "Cancel")
        {
            this.logger.SettingsCancelled(stepContext.Context.Activity);
            _ = await stepContext.Context.SendActivityAsync(
                "キャンセルしました。設定は変更されていません。",
                cancellationToken: cancellationToken
            );
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
                                Title = "スケジュール",
                                Value = new Func<string>(() =>
                                    property.StartMeetingSchedule switch
                                    {
                                        -1 => "なし",
                                        0 => "予定時刻",
                                        _ => $"{property.StartMeetingSchedule} 分後"
                                    }
                                )()
                            },
                            new()
                            {
                                Title = "メッセージ",
                                Value = $"{property.StartMeetingMessage}"
                            },
                            new()
                            {
                                Title = "URL",
                                Value = $"{property.StartMeetingUrl}"
                            }
                        ]
                    }
                ]
            };
            if (property.StartMeetingUrl is not null)
            {
                var bytes = await this.qrCodeService.CreateAsync(property.StartMeetingUrl, cancellationToken);
                var base64 = Convert.ToBase64String(bytes);
                card.Body.Add(new AdaptiveImage()
                {
                    AltText = property.StartMeetingUrl,
                    Size = AdaptiveImageSize.Stretch,
                    Url = new Uri($"data:image/png;base64,{base64}")
                });
            }
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
