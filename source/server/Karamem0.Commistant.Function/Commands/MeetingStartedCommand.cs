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

public class MeetingStartedCommand(
    IConnectorClientService connectorClientService,
    IDateTimeService dateTimeService,
    IMapper mapper,
    ILogger<MeetingStartedCommand> logger
) : Command()
{

    private readonly IConnectorClientService connectorClientService = connectorClientService;

    private readonly IDateTimeService dateTimeService = dateTimeService;

    private readonly IMapper mapper = mapper;

    private readonly ILogger<MeetingStartedCommand> logger = logger;

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
        if (commandSettings.MeetingStartedSchedule < 0)
        {
            return;
        }
        if (commandSettings.MeetingStartedSended is true)
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
        if (commandSettings.MeetingStartedSchedule > (int)timeSpan.TotalMinutes)
        {
            return;
        }
        try
        {
            this.logger.MeetingStartedMessageNotifying(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingStartedMessage,
                url: commandSettings.MeetingStartedUrl
            );
            var notifyCardData = this.mapper.Map<MeetingStartedNotifyCardData>(commandSettings);
            var notifyCard = MeetingStartedNotifyCard.Create(notifyCardData);
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
            this.logger.MeetingStartedMessageNotified(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingStartedMessage,
                url: commandSettings.MeetingStartedUrl
            );
        }
        commandSettings.MeetingStartedSended = true;
    }

    public class MapperConfiguration(IQRCodeService qrCodeService) : IRegister
    {

        private readonly IQRCodeService qrCodeService = qrCodeService;

        public void Register(TypeAdapterConfig config)
        {
            _ = config
                .NewConfig<CommandSettings, MeetingStartedNotifyCardData>()
                .Map(d => d.Schedule, s => s.MeetingStartedSchedule)
                .Map(d => d.Message, s => s.MeetingStartedMessage ?? "")
                .Map(d => d.Url, s => s.MeetingStartedUrl ?? "")
                .Map(d => d.QrCode, s => "")
                .AfterMapping(async (s, d) =>
                    {
                        if (Uri.TryCreate(
                                s.MeetingStartedUrl,
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
