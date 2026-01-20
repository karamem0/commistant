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
using Karamem0.Commistant.Services;
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

public class MeetingInProgressDialog(
    ConversationState conversationState,
    IMapper mapper,
    ILogger<MeetingInProgressDialog> logger
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
        var commandSettings = this.conversationState.GetValue<CommandSettings>(nameof(CommandSettings));
        var commandOptions = (CommandOptions?)stepContext.Options;
        var editCardData = this.mapper.Map<MeetingInProgressEditCardData>(
            this.mapper.Map(
                commandOptions,
                commandSettings with
                {
                }
            )
        );
        var editCard = MeetingInProgressEditCard.Create(editCardData);
        var activity = MessageFactory.Attachment(
            new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = editCard
            }
        );
        this.logger.SettingsUpdating(conversationId: stepContext.Context.Activity.Id);
        return await stepContext.PromptAsync(
            nameof(this.OnBeforeAsync),
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
        var response = JsonConverter.Deserialize<MeetingInProgressResponse>(value);
        if (response is null)
        {
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
        var commandSettings = this.conversationState.GetValue<CommandSettings>(nameof(CommandSettings));
        if (response.Button == ButtonTypes.Submit)
        {
            _ = this.mapper.Map(response, commandSettings);
            this.logger.SettingsUpdated(conversationId: stepContext.Context.Activity.Id);
            _ = await stepContext.Context.SendActivityAsync(Messages.SettingsUpdated, cancellationToken: cancellationToken);
        }
        if (response.Button == ButtonTypes.Cancel)
        {
            this.logger.SettingsUpdateCancelled(conversationId: stepContext.Context.Activity.Id);
            _ = await stepContext.Context.SendActivityAsync(Messages.SettingsUpdateCancelled, cancellationToken: cancellationToken);
        }
        if (stepContext.Context.Activity.ReplyToId is not null)
        {
            var viewCardData = this.mapper.Map<MeetingInProgressViewCardData>(commandSettings);
            var viewCard = MeetingInProgressViewCard.Create(viewCardData);
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
        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }

    public class MapperConfiguration(IQRCodeService qrCodeService) : IRegister
    {

        private readonly IQRCodeService qrCodeService = qrCodeService;

        public void Register(TypeAdapterConfig config)
        {
            _ = config
                .NewConfig<CommandSettings, MeetingInProgressEditCardData>()
                .Map(d => d.Schedule, s => s.MeetingInProgressSchedule)
                .Map(d => d.Message, s => s.MeetingInProgressMessage ?? "")
                .Map(d => d.Url, s => s.MeetingInProgressUrl ?? "");
            _ = config
                .NewConfig<CommandSettings, MeetingInProgressViewCardData>()
                .Map(d => d.Message, s => s.MeetingInProgressMessage ?? "")
                .Map(d => d.Url, s => s.MeetingInProgressUrl ?? "")
                .Map(d => d.QrCode, s => "")
                .AfterMapping(async (s, d) =>
                    {
                        d.Schedule = s.MeetingInProgressSchedule switch
                        {
                            -1 => "なし",
                            _ => $"{s.MeetingInProgressSchedule} 分ごと"
                        };
                        if (Uri.TryCreate(
                                s.MeetingInProgressUrl,
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
                .NewConfig<MeetingInProgressResponse, CommandSettings>()
                .Map(d => d.MeetingInProgressSchedule, s => s.Schedule)
                .Map(d => d.MeetingInProgressMessage, s => s.Message)
                .Map(d => d.MeetingInProgressUrl, s => s.Url);
        }

    }

}
