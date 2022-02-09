//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
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

        public ResetDialog(ConversationState conversationState)
        {
            this.conversationState = conversationState;
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
                await this.conversationState.ClearStateAsync(stepContext.Context, cancellationToken);
                _ = await stepContext.Context.SendActivityAsync(
                    "設定を初期化しました。",
                    cancellationToken: cancellationToken);
            }
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

    }

}
