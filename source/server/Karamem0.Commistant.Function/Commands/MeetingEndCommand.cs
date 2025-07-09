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

public class MeetingEndCommand(
    IBotConnectorService botConnectorService,
    IDateTimeService dateTimeService,
    IMapper mapper,
    ILogger<MeetingEndCommand> logger
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
            var notifyCardData = this.mapper.Map<MeetingEndNotifyCardData>(commandSettings);
            var notifyCard = MeetingEndNotifyCard.Create(notifyCardData);
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
            this.logger.MeetingEndMessageNotified(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingEndMessage,
                url: commandSettings.MeetingEndUrl
            );
        }
        commandSettings.MeetingEndSended = true;
    }

    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile(IQRCodeService qrCodeService)
        {
            _ = this
                .CreateMap<CommandSettings, MeetingEndNotifyCardData>()
                .ForMember(d => d.Schedule, o => o.MapFrom(s => s.MeetingEndSchedule))
                .ForMember(d => d.Message, o => o.MapFrom(s => s.MeetingEndMessage ?? ""))
                .ForMember(d => d.Url, o => o.MapFrom(s => s.MeetingEndUrl ?? ""))
                .ForMember(d => d.QrCode, o => o.MapFrom(s => ""))
                .AfterMap(async (s, d) =>
                    {
                        if (Uri.TryCreate(
                                s.MeetingEndUrl,
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
