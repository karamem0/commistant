//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Karamem0.Commistant.Templates;
using Karamem0.Commistant.Validators;
using Mapster;
using MapsterMapper;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace Karamem0.Commistant.Dialogs;

public class MeetingEndDialog(
    ConversationState conversationState,
    IMapper mapper,
    ILogger<MeetingEndDialog> logger
) : ComponentDialog
{

    private readonly ConversationState conversationState = conversationState;

    private readonly IMapper mapper = mapper;

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
        var commandSettingsAccessor = this.conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var commandSettings = await commandSettingsAccessor.GetAsync(
            stepContext.Context,
            () => new(),
            cancellationToken
        );
        var commandOptions = (CommandOptions?)stepContext.Options;
        var editCardData = this.mapper.Map<MeetingEndEditCardData>(
            this.mapper.Map(
                commandOptions,
                commandSettings with
                {
                }
            )
        );
        var editCard = MeetingEndEditCard.Create(editCardData);
        var activity = MessageFactory.Attachment(
            new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = editCard
            }
        );
        this.logger.SettingsUpdating(conversationId: stepContext.Context.Activity.Id);
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
        var response = value.ToObject<MeetingEndResponse>();
        if (response is null)
        {
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        var commandSettingsAccessor = this.conversationState.CreateProperty<CommandSettings>(nameof(CommandSettings));
        var commandSettings = await commandSettingsAccessor.GetAsync(
            stepContext.Context,
            () => new(),
            cancellationToken
        );
        if (response.Button == Constants.SubmitButton)
        {
            _ = this.mapper.Map(response, commandSettings);
            this.logger.SettingsUpdated(conversationId: stepContext.Context.Activity.Id);
            _ = await stepContext.Context.SendActivityAsync(Messages.SettingsUpdated, cancellationToken: cancellationToken);
        }
        if (response.Button == Constants.CancelButton)
        {
            this.logger.SettingsCancelled(conversationId: stepContext.Context.Activity.Id);
            _ = await stepContext.Context.SendActivityAsync(Messages.SettingsCancelled, cancellationToken: cancellationToken);
        }
        if (stepContext.Context.Activity.ReplyToId is not null)
        {
            var viewCardData = this.mapper.Map<MeetingEndViewCardData>(commandSettings);
            var viewCard = MeetingEndViewCard.Create(viewCardData);
            var activity = MessageFactory.Attachment(
                new Attachment()
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = viewCard
                }
            );
            activity.Id = stepContext.Context.Activity.ReplyToId;
            _ = await stepContext.Context.UpdateActivityAsync(activity, cancellationToken);
        }
        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }

    public class MapperConfiguration(IQRCodeService qrCodeService) : IRegister
    {

        private readonly IQRCodeService qrCodeService = qrCodeService;

        public void Register(TypeAdapterConfig config)
        {
            _ = config
                .NewConfig<CommandSettings, MeetingEndEditCardData>()
                .Map(d => d.Schedule, s => s.MeetingEndSchedule)
                .Map(d => d.Message, s => s.MeetingEndMessage ?? "")
                .Map(d => d.Url, s => s.MeetingEndUrl ?? "");
            _ = config
                .NewConfig<CommandSettings, MeetingEndViewCardData>()
                .Map(d => d.Message, s => s.MeetingEndMessage ?? "")
                .Map(d => d.Url, s => s.MeetingEndUrl ?? "")
                .Map(d => d.QrCode, s => "")
                .AfterMapping(async (s, d) =>
                    {
                        d.Schedule = s.MeetingEndSchedule switch
                        {
                            -1 => "なし",
                            0 => "予定時刻",
                            _ => $"{s.MeetingEndSchedule} 分前"
                        };
                        if (Uri.TryCreate(
                                s.MeetingEndUrl,
                                UriKind.Absolute,
                                out var url
                            ))
                        {
                            var bytes = await this.qrCodeService.CreateAsync(url.ToString());
                            var base64 = Convert.ToBase64String(bytes);
                            d.QrCode = base64;
                        }
                    }
                );
            _ = config
                .NewConfig<MeetingEndResponse, CommandSettings>()
                .Map(d => d.MeetingEndSchedule, s => s.Schedule)
                .Map(d => d.MeetingEndMessage, s => s.Message)
                .Map(d => d.MeetingEndUrl, s => s.Url);
        }

    }

}
