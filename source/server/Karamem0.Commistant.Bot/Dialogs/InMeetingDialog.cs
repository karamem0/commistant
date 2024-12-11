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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Dialogs;

public class InMeetingDialog(
    ConversationState conversationState,
    IQRCodeService qrCodeService,
    IMapper mapper,
    ILogger<InMeetingDialog> logger
) : ComponentDialog
{

    private readonly ConversationState conversationState = conversationState;

    private readonly IQRCodeService qrCodeService = qrCodeService;

    private readonly IMapper mapper = mapper;

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

    private async Task<DialogTurnResult> OnBeforeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
    {
        var accessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
        var property = await accessor.GetAsync(stepContext.Context, () => new(), cancellationToken);
        var options = (ConversationPropertyOptions?)stepContext.Options;
        var value = this.mapper.Map(options, property with {});
        var card = new AdaptiveCard("1.3")
        {
            Body =
            [
                new AdaptiveChoiceSetInput()
                {
                    Id = "Schedule",
                    Label = "スケジュール",
                    Placeholder = "通知を繰り返す間隔",
                    Choices =
                    [
                        new()
                        {
                            Title = "なし",
                            Value = "-1"
                        },
                        new()
                        {
                            Title = "5 分",
                            Value = "5"
                        },
                        new()
                        {
                            Title = "10 分",
                            Value = "10"
                        },
                        new()
                        {
                            Title = "15 分",
                            Value = "15"
                        },
                        new()
                        {
                            Title = "30 分",
                            Value = "30"
                        },
                        new()
                        {
                            Title = "60 分",
                            Value = "60"
                        },
                    ],
                    Value = value.InMeetingSchedule.ToString()
                },
                new AdaptiveTextInput()
                {
                    Id = "Message",
                    IsMultiline = true,
                    Label = "メッセージ",
                    Placeholder = "会議中に表示されるメッセージ",
                    Style = AdaptiveTextInputStyle.Text,
                    Value = value.InMeetingMessage
                },
                new AdaptiveTextInput()
                {
                    Id = "Url",
                    Label = "URL",
                    Placeholder = "会議中に表示されるリンクの URL",
                    Style = AdaptiveTextInputStyle.Url,
                    Value = value.InMeetingUrl
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
            Content = card
        });
        this.logger.SettingsUpdating(stepContext.Context.Activity);
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
        var accessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
        var property = await accessor.GetAsync(stepContext.Context, () => new(), cancellationToken);
        if (value.Value<string>("Button") == "Submit")
        {
            property.InMeetingSchedule = value.Value("Schedule", 0);
            property.InMeetingMessage = value.Value<string>("Message", null);
            property.InMeetingUrl = value.Value<string>("Url", null);
            this.logger.SettingsUpdated(stepContext.Context.Activity);
            _ = await stepContext.Context.SendSettingsUpdatedAsync(cancellationToken);
        }
        if (value.Value<string>("Button") == "Cancel")
        {
            this.logger.SettingsCancelled(stepContext.Context.Activity);
            _ = await stepContext.Context.SendSettingsCancelledAsync(cancellationToken);
        }
        if (stepContext.Context.Activity.ReplyToId is not null)
        {
            var card = new AdaptiveCard("1.3")
            {
                Body =
                [
                    new AdaptiveColumnSet()
                    {
                        Columns =
                        [
                            new AdaptiveColumn()
                            {
                                Items =
                                [
                                    new AdaptiveTextBlock()
                                    {
                                        Text = "スケジュール",
                                        Weight = AdaptiveTextWeight.Bolder
                                    }
                                ],
                                Width = "90px"
                            },
                            new AdaptiveColumn()
                            {
                                Items =
                                [
                                    new AdaptiveTextBlock()
                                    {
                                        Text = property.InMeetingSchedule switch
                                        {
                                            -1 => "なし",
                                            0 => "予定時刻",
                                            _ => $"{property.InMeetingSchedule} 分ごと"
                                        }
                                    }
                                ],
                                Width = "stretch"
                            }
                        ],
                    },
                    new AdaptiveColumnSet()
                    {
                        Columns =
                        [
                            new AdaptiveColumn()
                            {
                                Items =
                                [
                                    new AdaptiveTextBlock()
                                    {
                                        Text = "メッセージ",
                                        Weight = AdaptiveTextWeight.Bolder
                                    }
                                ],
                                Width = "90px"
                            },
                            new AdaptiveColumn()
                            {
                                Items =
                                [
                                    new AdaptiveTextBlock()
                                    {
                                        Text = $"{property.InMeetingMessage}",
                                        Wrap = true
                                    }
                                ],
                                Width = "stretch"
                            }
                        ]
                    },
                    new AdaptiveColumnSet()
                    {
                        Columns =
                        [
                            new AdaptiveColumn()
                            {
                                Items =
                                [
                                    new AdaptiveTextBlock()
                                    {
                                        Text = "URL",
                                        Weight = AdaptiveTextWeight.Bolder
                                    }
                                ],
                                Width = "90px"
                            },
                            new AdaptiveColumn()
                            {
                                Items =
                                [
                                    new AdaptiveTextBlock()
                                    {
                                        Text = $"{property.InMeetingUrl}"
                                    }
                                ],
                                Width = "stretch"
                            }
                        ]
                    }
                ]
            };
            if (Uri.TryCreate(property.InMeetingUrl, UriKind.Absolute, out var url))
            {
                var bytes = await this.qrCodeService.CreateAsync(url.ToString(), cancellationToken);
                var base64 = Convert.ToBase64String(bytes);
                card.Body.Add(new AdaptiveImage()
                {
                    AltText = url.ToString(),
                    Size = AdaptiveImageSize.Large,
                    Url = new Uri($"data:image/png;base64,{base64}")
                });
                card.Actions.Add(new AdaptiveOpenUrlAction()
                {
                    Title = "URL を開く",
                    Url = url,
                });
            }
            var activity = MessageFactory.Attachment(new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            });
            activity.Id = stepContext.Context.Activity.ReplyToId;
            _ = await stepContext.Context.UpdateActivityAsync(activity, cancellationToken);
        }
        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }

}
