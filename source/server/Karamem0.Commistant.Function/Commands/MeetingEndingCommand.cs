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

public class MeetingEndingCommand(
    IConnectorClientService connectorClientService,
    IDateTimeService dateTimeService,
    IMapper mapper,
    ILogger<MeetingEndingCommand> logger
) : Command()
{

    private readonly IConnectorClientService connectorClientService = connectorClientService;

    private readonly IDateTimeService dateTimeService = dateTimeService;

    private readonly IMapper mapper = mapper;

    private readonly ILogger logger = logger;

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
        if (commandSettings.MeetingEndingSchedule < 0)
        {
            return;
        }
        if (commandSettings.MeetingEndingSended is true)
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
        if ((int)timeSpan.TotalMinutes > commandSettings.MeetingEndingSchedule)
        {
            return;
        }
        try
        {
            this.logger.MeetingEndingMessageNotifying(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingEndingMessage,
                url: commandSettings.MeetingEndingUrl
            );
            var notifyCardData = this.mapper.Map<MeetingEndingNotifyCardData>(commandSettings);
            var notifyCard = MeetingEndingNotifyCard.Create(notifyCardData);
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
            this.logger.MeetingEndingMessageNotified(
                conversationId: conversationReference.Conversation.Id,
                message: commandSettings.MeetingEndingMessage,
                url: commandSettings.MeetingEndingUrl
            );
        }
        commandSettings.MeetingEndingSended = true;
    }

    public class MapperConfiguration(IQRCodeService qrCodeService) : IRegister
    {

        private readonly IQRCodeService qrCodeService = qrCodeService;

        public void Register(TypeAdapterConfig config)
        {
            _ = config
                .NewConfig<CommandSettings, MeetingEndingNotifyCardData>()
                .Map(d => d.Schedule, s => s.MeetingEndingSchedule)
                .Map(d => d.Message, s => s.MeetingEndingMessage ?? "")
                .Map(d => d.Url, s => s.MeetingEndingUrl ?? "")
                .Map(d => d.QrCode, s => "")
                .AfterMapping(async (s, d) =>
                    {
                        if (Uri.TryCreate(
                                s.MeetingEndingUrl,
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
