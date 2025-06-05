//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Commands;

public class MeetingEndCommand(
    IBotConnectorService botConnectorService,
    IDateTimeService dateTimeService,
    IQRCodeService qrCodeService,
    ILogger<MeetingEndCommand> logger
) : Command()
{

    private readonly IBotConnectorService botConnectorService = botConnectorService;

    private readonly IDateTimeService dateTimeService = dateTimeService;

    private readonly IQRCodeService qrCodeService = qrCodeService;

    private readonly ILogger logger = logger;

    public override async Task ExecuteAsync(
        CommandSettings commandSettings,
        ConversationReference conversationReference,
        CancellationToken cancellationToken = default
    )
    {
        if (commandSettings.MeetingRunning is false)
        {
            return;
        }
        if (commandSettings.MeetingEndSchedule < 0)
        {
            return;
        }
        if (commandSettings.MeetingEndSended is true)
        {
            return;
        }
        var endTime = commandSettings.ScheduledEndTime;
        if (endTime is null)
        {
            return;
        }
        var currentTime = this.dateTimeService.GetCurrentDateTime();
        var timeSpan = (DateTime)endTime - currentTime;
        if (timeSpan.TotalMinutes < 0)
        {
            return;
        }
        if ((int)timeSpan.TotalMinutes > commandSettings.MeetingEndSchedule)
        {
            return;
        }
        try
        {
            this.logger.MeetingEndMessageNotifying(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingEndMessage,
                url: commandSettings.MeetingEndUrl
            );
            var card = new AdaptiveCard("1.3");
            if (string.IsNullOrEmpty(commandSettings.MeetingEndMessage) is false)
            {
                card.Body.Add(
                    new AdaptiveTextBlock()
                    {
                        Text = commandSettings.MeetingEndMessage,
                        Wrap = true
                    }
                );
            }
            if (Uri.TryCreate(
                    commandSettings.MeetingEndUrl,
                    UriKind.Absolute,
                    out var url
                ))
            {
                var bytes = await this.qrCodeService.CreateAsync(url.ToString(), cancellationToken);
                var base64 = Convert.ToBase64String(bytes);
                card.Body.Add(
                    new AdaptiveImage()
                    {
                        AltText = url.ToString(),
                        Size = AdaptiveImageSize.Stretch,
                        Url = new Uri($"data:image/png;base64,{base64}")
                    }
                );
                card.Actions.Add(
                    new AdaptiveOpenUrlAction()
                    {
                        Title = "URL を開く",
                        Url = url,
                    }
                );
            }
            var activity = MessageFactory.Attachment(
                new Attachment()
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = card
                }
            );
            activity.From = conversationReference.Bot;
            activity.Recipient = conversationReference.User;
            activity.Conversation = conversationReference.Conversation;
            _ = await this.botConnectorService.SendActivityAsync(
                new Uri(conversationReference.ServiceUrl),
                (Activity)activity,
                cancellationToken
            );
        }
        finally
        {
            this.logger.MeetingEndMessageNotified(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingEndMessage,
                url: commandSettings.MeetingEndUrl
            );
        }
        commandSettings.MeetingEndSended = true;
    }

}
