//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Serialization;
using Karamem0.Commistant.Templates;
using Karamem0.Commistant.Types;
using Karamem0.Commistant.Validators;
using Mapster;
using MapsterMapper;
using Microsoft.Agents.Builder.Dialogs;
using Microsoft.Agents.Builder.Dialogs.Prompts;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Core.Models;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading;

namespace Karamem0.Commistant.Dialogs;

public class InitializeDialog(
    ConversationState conversationState,
    IMapper mapper,
    ILogger<InitializeDialog> logger
) : ComponentDialog
{

    private readonly ConversationState conversationState = conversationState;

    private readonly IMapper mapper = mapper;

    private readonly ILogger<InitializeDialog> logger = logger;

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
        _ = this.AddDialog(new TextPrompt(nameof(TextPrompt), AdaptiveCardValidator.Validate));
        await base.OnInitializeAsync(dialogContext);
    }

    private async Task<DialogTurnResult> OnBeforeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
    {
        var editCardData = new InitializeCardData();
        var editCard = InitializeEditCard.Create(editCardData);
        var activity = MessageFactory.Attachment(
            new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = editCard
            }
        );
        this.logger.SettingsInitializing(conversationId: stepContext.Context.Activity.Id);
        return await stepContext.PromptAsync(
            nameof(TextPrompt),
            new PromptOptions()
            {
                Prompt = activity
            },
            cancellationToken
        );
    }

    private async Task<DialogTurnResult> OnAfterAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
    {
        var value = (JsonElement)stepContext.Context.Activity.Value;
        var response = JsonConverter.Deserialize<InitializeResponse>(value);
        if (response is null)
        {
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        if (response.Button == ButtonTypes.Yes)
        {
            this.conversationState.SetValue<CommandSettings>(nameof(CommandSettings), new());
            this.logger.SettingsInitialized(conversationId: stepContext.Context.Activity.Id);
            _ = await stepContext.Context.SendActivityAsync(Messages.SettingsInitialized, cancellationToken: cancellationToken);
        }
        if (response.Button == ButtonTypes.No)
        {
            this.logger.SettingsInitializeCancelled(conversationId: stepContext.Context.Activity.Id);
            _ = await stepContext.Context.SendActivityAsync(Messages.SettingsInitializeCancelled, cancellationToken: cancellationToken);
        }
        if (stepContext.Context.Activity.ReplyToId is not null)
        {
            var viewCardData = this.mapper.Map<InitializeViewCardData>(response);
            var viewCard = InitializeViewCard.Create(viewCardData);
            var activity = MessageFactory.Attachment(
                new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = viewCard
                }
            );
            activity.Id = stepContext.Context.Activity.ReplyToId;
            _ = await stepContext.Context.UpdateActivityAsync(activity, cancellationToken);
        }
        return await stepContext.EndDialogAsync(null, cancellationToken);
    }

    public class MapperConfiguration : IRegister
    {

        public void Register(TypeAdapterConfig config)
        {
            _ = config
                .NewConfig<InitializeResponse, InitializeViewCardData>()
                .AfterMapping((s, d) => d.Value = s.Button switch
                    {
                        ButtonTypes.Yes => "はい",
                        ButtonTypes.No => "いいえ",
                        _ => ""
                    }
                );
        }

    }

}
