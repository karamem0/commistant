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
using Karamem0.Commistant.Templates;
using Mapster;
using MapsterMapper;
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

public class MeetingStartCommand(
    IBotConnectorService botConnectorService,
    IDateTimeService dateTimeService,
    IMapper mapper,
    ILogger<MeetingStartCommand> logger
) : Command()
{

    private readonly IBotConnectorService botConnectorService = botConnectorService;

    private readonly IDateTimeService dateTimeService = dateTimeService;

    private readonly IMapper mapper = mapper;

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
        if (commandSettings.MeetingStartSchedule < 0)
        {
            return;
        }
        if (commandSettings.MeetingStartSended is true)
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
        if (commandSettings.MeetingStartSchedule > (int)timeSpan.TotalMinutes)
        {
            return;
        }
        try
        {
            this.logger.MeetingStartMessageNotifying(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingStartMessage,
                url: commandSettings.MeetingStartUrl
            );
            var notifyCardData = this.mapper.Map<MeetingStartNotifyCardData>(commandSettings);
            var notifyCard = MeetingStartNotifyCard.Create(notifyCardData);
            var activity = MessageFactory.Attachment(
                new Attachment()
                {
                    ContentType = AdaptiveCard.ContentType,
                    Content = notifyCard
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
            this.logger.MeetingStartMessageNotified(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingStartMessage,
                url: commandSettings.MeetingStartUrl
            );
        }
        commandSettings.MeetingStartSended = true;
    }

    public class MapperConfiguration(IQRCodeService qrCodeService) : IRegister
    {

        private readonly IQRCodeService qrCodeService = qrCodeService;

        public void Register(TypeAdapterConfig config)
        {
            _ = config
                .NewConfig<CommandSettings, MeetingStartNotifyCardData>()
                .Map(d => d.Schedule, s => s.MeetingStartSchedule)
                .Map(d => d.Message, s => s.MeetingStartMessage ?? "")
                .Map(d => d.Url, s => s.MeetingStartUrl ?? "")
                .Map(d => d.QrCode, s => "")
                .AfterMapping(async (s, d) =>
                    {
                        if (Uri.TryCreate(
                                s.MeetingStartUrl,
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
        }

    }

}
