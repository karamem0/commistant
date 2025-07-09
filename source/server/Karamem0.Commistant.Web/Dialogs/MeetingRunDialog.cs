//
// Copyright (c) 2022-2025 karamem0
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
using Karamem0.Commistant.Templates;
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

public class MeetingRunDialog(
    ConversationState conversationState,
    IMapper mapper,
    ILogger<MeetingRunDialog> logger
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
        var editCardData = this.mapper.Map<MeetingRunEditCardData>(
            this.mapper.Map(
                commandOptions,
                commandSettings with
                {
                }
            )
        );
        var editCard = MeetingRunEditCard.Create(editCardData);
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
        var response = value.ToObject<MeetingRunResponse>();
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
            _ = await stepContext.Context.SendSettingsUpdatedAsync(cancellationToken);
        }
        if (response.Button == Constants.CancelButton)
        {
            this.logger.SettingsCancelled(conversationId: stepContext.Context.Activity.Id);
            _ = await stepContext.Context.SendSettingsCancelledAsync(cancellationToken);
        }
        if (stepContext.Context.Activity.ReplyToId is not null)
        {
            var viewCardData = this.mapper.Map<MeetingRunViewCardData>(commandSettings);
            var viewCard = MeetingRunViewCard.Create(viewCardData);
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

    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile(IQRCodeService qrCodeService)
        {
            _ = this
                .CreateMap<CommandSettings, MeetingRunEditCardData>()
                .ForMember(d => d.Schedule, o => o.MapFrom(s => s.MeetingRunSchedule))
                .ForMember(d => d.Message, o => o.MapFrom(s => s.MeetingRunMessage ?? ""))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.MeetingRunUrl ?? ""));
            _ = this
                .CreateMap<CommandSettings, MeetingRunViewCardData>()
                .ForMember(
                    d => d.Schedule,
                    o => o.MapFrom((s, d) => s.MeetingRunSchedule switch
                        {
                            -1 => "なし",
                            _ => $"{s.MeetingRunSchedule} 分ごと"
                        }
                    )
                )
                .ForMember(d => d.Message, o => o.MapFrom(s => s.MeetingRunMessage ?? ""))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.MeetingRunUrl ?? ""))
                .ForMember(d => d.QrCode, o => o.MapFrom(s => ""))
                .AfterMap(async (s, d) =>
                    {
                        if (Uri.TryCreate(
                                s.MeetingRunUrl,
                                UriKind.Absolute,
                                out var url
                            ))
                        {
                            var bytes = await qrCodeService.CreateAsync(url.ToString());
                            var base64 = Convert.ToBase64String(bytes);
                            d.QrCode = base64;
                        }
                    }
                );
            _ = this
                .CreateMap<MeetingRunResponse, CommandSettings>()
                .ForMember(d => d.MeetingRunSchedule, o => o.MapFrom(s => s.Schedule))
                .ForMember(d => d.MeetingRunMessage, o => o.MapFrom(s => s.Message))
                .ForMember(d => d.MeetingRunUrl, o => o.MapFrom(s => s.Url));
        }

    }

}
