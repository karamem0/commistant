//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using AutoMapper;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Karamem0.Commistant.Templates;
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

public class MeetingRunCommand(
    IBotConnectorService botConnectorService,
    IDateTimeService dateTimeService,
    IMapper mapper,
    ILogger<MeetingRunCommand> logger
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
        if (commandSettings.MeetingRunSchedule <= 0)
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
        if ((int)timeSpan.TotalMinutes % commandSettings.MeetingRunSchedule > 0)
        {
            return;
        }
        try
        {
            this.logger.MeetingRunMessageNotifying(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingRunMessage,
                url: commandSettings.MeetingRunUrl
            );
            var notifyCardData = this.mapper.Map<MeetingRunNotifyCardData>(commandSettings);
            var notifyCard = MeetingRunNotifyCard.Create(notifyCardData);
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
            this.logger.MeetingRunMessageNotified(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingRunMessage,
                url: commandSettings.MeetingRunUrl
            );
        }
    }

    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile(IQRCodeService qrCodeService)
        {
            _ = this
                .CreateMap<CommandSettings, MeetingRunNotifyCardData>()
                .ForMember(d => d.Schedule, o => o.MapFrom(s => s.MeetingRunSchedule))
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
        }

    }

}
