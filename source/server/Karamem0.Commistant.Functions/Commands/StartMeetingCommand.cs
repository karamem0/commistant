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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Commands;

public class StartMeetingCommand(
    IDateTimeService dateTimeService,
    IConnectorClientService connectorClientService,
    IQRCodeService qrCodeService,
    ILogger<StartMeetingCommand> logger
) : Command()
{

    private readonly IDateTimeService dateTimeService = dateTimeService;

    private readonly IConnectorClientService connectorClientService = connectorClientService;

    private readonly IQRCodeService qrCodeService = qrCodeService;

    private readonly ILogger logger = logger;

    public override async Task ExecuteAsync(
        ConversationProperty property,
        ConversationReference reference,
        CancellationToken cancellationToken = default
    )
    {
        if (property.InMeeting is false)
        {
            return;
        }
        if (property.StartMeetingSchedule < 0)
        {
            return;
        }
        if (property.StartMeetingSended is true)
        {
            return;
        }
        var startTime = property.ScheduledStartTime;
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
        if (property.StartMeetingSchedule > (int)timeSpan.TotalMinutes)
        {
            return;
        }
        try
        {
            this.logger.StartMeetingMessageNotifying(reference, property);
            var card = new AdaptiveCard("1.3");
            if (string.IsNullOrEmpty(property.StartMeetingMessage) is false)
            {
                card.Body.Add(new AdaptiveTextBlock()
                {
                    Text = property.StartMeetingMessage,
                    Wrap = true
                });
            }
            if (Uri.TryCreate(property.StartMeetingUrl, UriKind.Absolute, out var url))
            {
                var bytes = await this.qrCodeService.CreateAsync(url.ToString(), cancellationToken);
                var base64 = Convert.ToBase64String(bytes);
                card.Body.Add(new AdaptiveImage()
                {
                    AltText = url.ToString(),
                    Size = AdaptiveImageSize.Stretch,
                    Url = new Uri($"data:image/png;base64,{base64}")
                });
                card.Actions.Add(new AdaptiveOpenUrlAction()
                {
                    Title = "URL を開く",
                    Url = url,
                });
            }
            var activity = MessageFactory.Attachment(new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card
            });
            activity.From = reference.Bot;
            activity.Recipient = reference.User;
            activity.Conversation = reference.Conversation;
            _ = await this.connectorClientService.SendActivityAsync(
                new Uri(reference.ServiceUrl),
                (Activity)activity,
                cancellationToken
            );
        }
        finally
        {
            this.logger.StartMeetingMessageNotified(reference, property);
        }
        property.StartMeetingSended = true;
    }

}
