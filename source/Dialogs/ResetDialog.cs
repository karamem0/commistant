//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Logging;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Dialogs
{

    public class ResetDialog : ComponentDialog
    {

        private readonly ConversationState conversationState;

        private readonly ILogger logger;

        public ResetDialog(ConversationState conversationState, ILogger<ResetDialog> logger)
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
            _ = this.AddDialog(new ChoicePrompt(nameof(this.BeforeConfirmAsync)));
            await base.OnInitializeAsync(dc);
        }

        private async Task<DialogTurnResult> BeforeConfirmAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            this.logger.SettingsResetting(stepContext.Context.Activity);
            return await stepContext.PromptAsync(
                nameof(this.BeforeConfirmAsync),
                new PromptOptions()
                {
                    Prompt = MessageFactory.Text("すべての設定を初期化します。よろしいですか？"),
                    Choices = ChoiceFactory.ToChoices(new[]
                    {
                        "はい",
                        "いいえ"
                    })
                },
                cancellationToken);
        }

        private async Task<DialogTurnResult> AfterConrifmAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var result = (FoundChoice)stepContext.Result;
            if (result.Value == "はい")
            {
                this.logger.SettingsReseted(stepContext.Context.Activity);
                await this.conversationState.ClearStateAsync(stepContext.Context, cancellationToken);
                _ = await stepContext.Context.SendActivityAsync(
                    "設定を初期化しました。",
                    cancellationToken: cancellationToken);
            }
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

    }

}
