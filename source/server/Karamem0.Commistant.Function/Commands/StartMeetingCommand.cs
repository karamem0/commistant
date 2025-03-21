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

public class StartMeetingCommand(
    IBotConnectorService botConnectorService,
    IDateTimeService dateTimeService,
    IQRCodeService qrCodeService,
    ILogger<StartMeetingCommand> logger
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
        if (commandSettings.InMeeting is false)
        {
            return;
        }
        if (commandSettings.StartMeetingSchedule < 0)
        {
            return;
        }
        if (commandSettings.StartMeetingSended is true)
        {
            return;
        }
        var startTime = commandSettings.ScheduledStartTime;
        if (startTime is null)
        {
            return;
        }
        var currentTime = this.dateTimeService.GetCurrentDateTime();
        var timeSpan = currentTime - (DateTime)startTime;
        if (timeSpan.TotalMinutes < 0)
        {
            return;
        }
        if (commandSettings.StartMeetingSchedule > (int)timeSpan.TotalMinutes)
        {
            return;
        }
        try
        {
            this.logger.StartMeetingMessageNotifying(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.StartMeetingMessage,
                url: commandSettings.StartMeetingUrl
            );
            var card = new AdaptiveCard("1.3");
            if (string.IsNullOrEmpty(commandSettings.StartMeetingMessage) is false)
            {
                card.Body.Add(
                    new AdaptiveTextBlock()
                    {
                        Text = commandSettings.StartMeetingMessage,
                        Wrap = true
                    }
                );
            }
            if (Uri.TryCreate(
                    commandSettings.StartMeetingUrl,
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
            this.logger.StartMeetingMessageNotified(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.StartMeetingMessage,
                url: commandSettings.StartMeetingUrl
            );
        }
        commandSettings.StartMeetingSended = true;
    }

}
