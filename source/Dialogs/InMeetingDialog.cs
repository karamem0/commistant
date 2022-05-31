//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using Karamem0.Commistant.Extensions;
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

namespace Karamem0.Commistant.Dialogs
{

    public class InMeetingDialog : ComponentDialog
    {

        private readonly ConversationState conversationState;

        private readonly ILogger logger;

        public InMeetingDialog(ConversationState conversationState, ILogger<InMeetingDialog> logger)
        {
            this.conversationState = conversationState;
            this.logger = logger;
        }

        protected override async Task OnInitializeAsync(DialogContext dc)
        {
            _ = this.AddDialog(new WaterfallDialog(
                nameof(WaterfallDialog),
                new WaterfallStep[]
                {
                    this.BeforeConfirmAsync,
                    this.AfterConrifmAsync
                }));
            _ = this.AddDialog(new TextPrompt(nameof(this.BeforeConfirmAsync), AdaptiveCardvalidator.Validate));
            await base.OnInitializeAsync(dc);
        }

        private async Task<DialogTurnResult> BeforeConfirmAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var accessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
            var property = await accessor.GetAsync(stepContext.Context, () => new(), cancellationToken);
            var card = new AdaptiveCard("1.3")
            {
                Body = new List<AdaptiveElement>()
                {
                    new AdaptiveChoiceSetInput()
                    {
                        Id = "Schedule",
                        Label = "スケジュール",
                        Placeholder = "通知を繰り返す間隔",
                        Choices = new List<AdaptiveChoice>()
                        {
                            new AdaptiveChoice()
                            {
                                Title = "なし",
                                Value = "-1"
                            },
                            new AdaptiveChoice()
                            {
                                Title = "5 分",
                                Value = "5"
                            },
                            new AdaptiveChoice()
                            {
                                Title = "10 分",
                                Value = "10"
                            },
                            new AdaptiveChoice()
                            {
                                Title = "15 分",
                                Value = "15"
                            },
                            new AdaptiveChoice()
                            {
                                Title = "30 分",
                                Value = "30"
                            },
                            new AdaptiveChoice()
                            {
                                Title = "60 分",
                                Value = "60"
                            },
                        },
                        Value = property.InMeetingSchedule.ToString()
                    },
                    new AdaptiveTextInput()
                    {
                        Id = "Message",
                        Label = "メッセージ",
                        Placeholder = "会議中に表示されるメッセージ",
                        Style = AdaptiveTextInputStyle.Text,
                        Value = property.InMeetingMessage
                    },
                    new AdaptiveTextInput()
                    {
                        Id = "Url",
                        Label = "URL",
                        Placeholder = "会議中に表示されるリンクの URL",
                        Style = AdaptiveTextInputStyle.Url,
                        Value = property.InMeetingUrl
                    }
                },
                Actions = new List<AdaptiveAction>()
                {
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
                }
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
                cancellationToken);
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
                property.InMeetingSchedule = value.Value<int>("Schedule", 0);
                property.InMeetingMessage = value.Value<string>("Message", null);
                property.InMeetingUrl = value.Value<string>("Url", null);
                this.logger.SettingsUpdated(stepContext.Context.Activity);
                _ = await stepContext.Context.SendActivityAsync(
                    "設定を変更しました。",
                    cancellationToken: cancellationToken);
            }
            if (stepContext.Context.Activity.ReplyToId != null)
            {
                var card = new AdaptiveCard("1.3")
                {
                    Body = new List<AdaptiveElement>()
                    {
                        new AdaptiveFactSet()
                        {
                            Facts = new List<AdaptiveFact>()
                            {
                                new AdaptiveFact()
                                {
                                    Title = "スケジュール",
                                    Value = new Func<string>(() =>
                                        property.InMeetingSchedule switch
                                        {
                                            -1 => "なし",
                                            _ => $"{property.InMeetingSchedule} 分"
                                        }
                                    )()
                                },
                                new AdaptiveFact()
                                {
                                    Title = "メッセージ",
                                    Value = $"{property.InMeetingMessage}"
                                },
                                new AdaptiveFact()
                                {
                                    Title = "URL",
                                    Value = $"{property.InMeetingUrl}"
                                }
                            }
                        }
                    }
                };
                var activity = MessageFactory.Attachment(new Attachment()
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = JsonConvert.DeserializeObject(card.ToJson())
                });
                activity.Id = stepContext.Context.Activity.ReplyToId;
                _ = await stepContext.Context.UpdateActivityAsync(
                    activity,
                    cancellationToken: cancellationToken);
            }
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

    }

}
