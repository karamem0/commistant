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

    public class EndMeetingDialog : ComponentDialog
    {

        private readonly ConversationState conversationState;

        private readonly ILogger logger;

        public EndMeetingDialog(ConversationState conversationState, ILogger<EndMeetingDialog> logger)
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
                        Placeholder = "通知を表示する時間",
                        Choices = new List<AdaptiveChoice>()
                        {
                            new AdaptiveChoice()
                            {
                                Title = "なし",
                                Value = "-1"
                            },
                            new AdaptiveChoice()
                            {
                                Title = "予定時刻",
                                Value = "0"
                            },
                            new AdaptiveChoice()
                            {
                                Title = "5 分前",
                                Value = "5"
                            },
                            new AdaptiveChoice()
                            {
                                Title = "10 分前",
                                Value = "10"
                            },
                            new AdaptiveChoice()
                            {
                                Title = "15 分前",
                                Value = "15"
                            },
                        },
                        Value = property.EndMeetingSchedule.ToString()
                    },
                    new AdaptiveTextInput()
                    {
                        Id = "Message",
                        Label = "メッセージ",
                        Placeholder = "会議後に表示されるメッセージ",
                        Style = AdaptiveTextInputStyle.Text,
                        Value = property.EndMeetingMessage
                    },
                    new AdaptiveTextInput()
                    {
                        Id = "Url",
                        Label = "URL",
                        Placeholder = "会議後に表示されるリンクの URL",
                        Style = AdaptiveTextInputStyle.Url,
                        Value = property.EndMeetingUrl
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
                property.EndMeetingSchedule = value.Value("Schedule", -1);
                property.EndMeetingMessage = value.Value<string>("Message", null);
                property.EndMeetingUrl = value.Value<string>("Url", null);
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
                                        property.EndMeetingSchedule switch
                                        {
                                            -1 => "なし",
                                            0 => "予定時刻",
                                            _ => $"{property.EndMeetingSchedule} 分後"
                                        }
                                    )()
                                },
                                new AdaptiveFact()
                                {
                                    Title = "メッセージ",
                                    Value = $"{property.EndMeetingMessage}"
                                },
                                new AdaptiveFact()
                                {
                                    Title = "URL",
                                    Value = $"{property.EndMeetingUrl}"
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
