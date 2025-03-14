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

public class InMeetingCommand(
    IDateTimeService dateTimeService,
    IBotConnectorService botConnectorService,
    IQRCodeService qrCodeService,
    ILogger<InMeetingCommand> logger
) : Command()
{

    private readonly IDateTimeService dateTimeService = dateTimeService;

    private readonly IBotConnectorService botConnectorService = botConnectorService;

    private readonly IQRCodeService qrCodeService = qrCodeService;

    private readonly ILogger logger = logger;

    public override async Task ExecuteAsync(
        ConversationProperties conversationProperties,
        ConversationReference conversationReference,
        CancellationToken cancellationToken = default
    )
    {
        if (conversationProperties.InMeeting is false)
        {
            return;
        }
        if (conversationProperties.InMeetingSchedule <= 0)
        {
            return;
        }
        var startTime = conversationProperties.ScheduledStartTime;
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
        if ((int)timeSpan.TotalMinutes % conversationProperties.InMeetingSchedule > 0)
        {
            return;
        }
        try
        {
            this.logger.InMeetingMessageNotifying(
                conversationId: conversationReference.Conversation.Id,
                message: conversationProperties.InMeetingMessage,
                url: conversationProperties.InMeetingUrl
            );
            var card = new AdaptiveCard("1.3");
            if (string.IsNullOrEmpty(conversationProperties.InMeetingMessage) is false)
            {
                card.Body.Add(
                    new AdaptiveTextBlock()
                    {
                        Text = conversationProperties.InMeetingMessage,
                        Wrap = true
                    }
                );
            }
            if (Uri.TryCreate(
                    conversationProperties.InMeetingUrl,
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
            this.logger.InMeetingMessageNotified(
                conversationId: conversationReference.Conversation.Id,
                message: conversationProperties.InMeetingMessage,
                url: conversationProperties.InMeetingUrl
            );
        }
    }

}
