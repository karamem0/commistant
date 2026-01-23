//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Karamem0.Commistant.Templates;
using Mapster;
using MapsterMapper;
using Microsoft.Agents.Core.Models;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Karamem0.Commistant.Commands;

public class MeetingInProgressCommand(
    IConnectorClientService connectorClientService,
    IDateTimeService dateTimeService,
    IMapper mapper,
    ILogger<MeetingInProgressCommand> logger
) : Command()
{

    private readonly IConnectorClientService connectorClientService = connectorClientService;

    private readonly IDateTimeService dateTimeService = dateTimeService;

    private readonly IMapper mapper = mapper;

    private readonly ILogger<MeetingInProgressCommand> logger = logger;

    public override async Task ExecuteAsync(
        CommandSettings commandSettings,
        ConversationReference conversationReference,
        CancellationToken cancellationToken = default
    )
    {
        if (commandSettings.MeetingInProgress is false)
        {
            return;
        }
        if (commandSettings.MeetingInProgressSchedule <= 0)
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
        if ((int)timeSpan.TotalMinutes % commandSettings.MeetingInProgressSchedule > 0)
        {
            return;
        }
        try
        {
            this.logger.MeetingInProgressMessageNotifying(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingInProgressMessage,
                url: commandSettings.MeetingInProgressUrl
            );
            var notifyCardData = this.mapper.Map<MeetingInProgressNotifyCardData>(commandSettings);
            var notifyCard = MeetingInProgressNotifyCard.Create(notifyCardData);
            var activity = MessageFactory.Attachment(
                new Attachment()
                {
                    ContentType = "application/vnd.microsoft.card.adaptive",
                    Content = notifyCard
                }
            );
            activity.From = conversationReference.Agent;
            activity.Recipient = conversationReference.User;
            activity.Conversation = conversationReference.Conversation;
            _ = await this.connectorClientService.SendActivityAsync(
                conversationReference.ServiceUrl,
                activity,
                cancellationToken
            );
        }
        finally
        {
            this.logger.MeetingInProgressMessageNotified(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingInProgressMessage,
                url: commandSettings.MeetingInProgressUrl
            );
        }
    }

    public class MapperConfiguration(IQRCodeService qrCodeService) : IRegister
    {

        private readonly IQRCodeService qrCodeService = qrCodeService;

        public void Register(TypeAdapterConfig config)
        {
            _ = config
                .NewConfig<CommandSettings, MeetingInProgressNotifyCardData>()
                .Map(d => d.Schedule, s => s.MeetingInProgressSchedule)
                .Map(d => d.Message, s => s.MeetingInProgressMessage ?? "")
                .Map(d => d.Url, s => s.MeetingInProgressUrl ?? "")
                .Map(d => d.QrCode, s => "")
                .AfterMapping(async (s, d) =>
                    {
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
        }

    }

}
