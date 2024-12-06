//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

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

public class EndMeetingDialog(
    ConversationState conversationState,
    IAdaptiveCardService adaptiveCardService,
    IQRCodeService qrCodeService,
    IMapper mapper,
    ILogger<EndMeetingDialog> logger
) : ComponentDialog
{

    private readonly ConversationState conversationState = conversationState;

    private readonly IAdaptiveCardService adaptiveCardService = adaptiveCardService;

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

    private async Task<DialogTurnResult> OnBeforeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
    {
        var accessor = this.conversationState.CreateProperty<ConversationProperty>(nameof(ConversationProperty));
        var property = await accessor.GetAsync(stepContext.Context, () => new(), cancellationToken);
        var options = (ConversationPropertyArguments?)stepContext.Options;
        var value = this.mapper.Map(options, property.Clone());
        var arguments = new MeetingCardTemplateArguments()
        {
            Schedule = value.EndMeetingSchedule,
            Message = value.EndMeetingMessage ?? "",
            Url = value.EndMeetingUrl ?? "",
        };
        var card = await this.adaptiveCardService.GetCardAsync(
            AdaptiveCardTemplateTypes.EndMeetingOnBefore,
            arguments,
            cancellationToken
        );
        var activity = MessageFactory.Attachment(new Attachment()
        {
            ContentType = "application/vnd.microsoft.card.adaptive",
            Content = JsonConvert.DeserializeObject(card)
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

    private async Task<DialogTurnResult> OnAfterAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
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
            _ = await stepContext.Context.SendSettingsUpdatedAsync(cancellationToken);
        }
        if (value.Value<string>("Button") == "Cancel")
        {
            this.logger.SettingsCancelled(stepContext.Context.Activity);
            _ = await stepContext.Context.SendSettingsCancelledAsync(cancellationToken);
        }
        if (stepContext.Context.Activity.ReplyToId is not null)
        {
            var arguments = new MeetingCardTemplateArguments()
            {
                Schedule = property.EndMeetingSchedule,
                Message = property.EndMeetingMessage ?? ""
            };
            if (Uri.TryCreate(property.EndMeetingUrl, UriKind.Absolute, out var url))
            {
                var bytes = await this.qrCodeService.CreateAsync(url.ToString(), cancellationToken);
                var base64 = Convert.ToBase64String(bytes);
                arguments.Url = url.ToString();
                arguments.QRCode = $"data:image/png;base64,{base64}";
            }
            var card = await this.adaptiveCardService.GetCardAsync(
                AdaptiveCardTemplateTypes.EndMeetingOnAfter,
                arguments,
                cancellationToken
            );
            var activity = MessageFactory.Attachment(new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(card)
            });
            activity.Id = stepContext.Context.Activity.ReplyToId;
            _ = await stepContext.Context.UpdateActivityAsync(activity, cancellationToken);
        }
        return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
    }

}
