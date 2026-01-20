//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Routes.Abstraction;
using Karamem0.Commistant.Services;
using Karamem0.Commistant.Types;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Core.Models;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Karamem0.Commistant.Routes;

public class MessageRouteHandler(
    ConversationState conversationState,
    IMeetingService meetingService,
    IOpenAIService openAIService,
    IDialogSetFactory dialogSetFactory,
    ILogger<MessageRouteHandler> logger
) : RouteHandler
{

    private readonly ConversationState conversationState = conversationState;

    private readonly IMeetingService meetingService = meetingService;

    private readonly IOpenAIService openAIService = openAIService;

    private readonly IDialogSetFactory dialogSetFactory = dialogSetFactory;

    private readonly ILogger<MessageRouteHandler> logger = logger;

    public override async Task InvokeAsync(
        ITurnContext turnContext,
        ITurnState turnState,
        CancellationToken cancellationToken = default
    )
    {
        this.logger.MessageReceived(conversationId: turnContext.Activity.Conversation.Id);
        var participant = await this.meetingService.GetMeetingParticipantAsync(
            turnContext,
            participantId: turnContext.Activity.Recipient.AadObjectId,
            cancellationToken: cancellationToken
        );
        if (participant.Meeting.Role != "Organizer")
        {
            _ = await turnContext.SendActivityAsync(Messages.UserIsNotOrganizer, cancellationToken: cancellationToken);
            return;
        }
        var dialogSet = this.dialogSetFactory.Create();
        var dialogContext = await dialogSet.CreateContextAsync(turnContext, cancellationToken);
        var command = turnContext.Activity.RemoveRecipientMention();
        if (command is null)
        {
            if (dialogContext.ActiveDialog is not null)
            {
                _ = await dialogContext.ContinueDialogAsync(cancellationToken);
            }
            return;
        }
        var commandSettings = this.conversationState.GetValue<CommandSettings>(nameof(CommandSettings), () => new());
        if (commandSettings.MeetingInProgress is true)
        {
            _ = await turnContext.SendActivityAsync(Messages.SettingsCannotUpdateWhenMeetingInProgress, cancellationToken: cancellationToken);
            return;
        }
        if (dialogContext.ActiveDialog is not null)
        {
            _ = await turnContext.SendActivityAsync(Messages.InterruptedCommandPending, cancellationToken: cancellationToken);
            return;
        }
        var arguments = await this.openAIService.GetCommandOptionsAsync(command, cancellationToken);
        var result = arguments?.Type switch
        {
            CommandTypes.MeetingStarted => await dialogContext.BeginDialogAsync(
                nameof(MeetingStartedDialog),
                arguments,
                cancellationToken
            ),
            CommandTypes.MeetingEnding => await dialogContext.BeginDialogAsync(
                nameof(MeetingEndingDialog),
                arguments,
                cancellationToken
            ),
            CommandTypes.MeetingInProgress => await dialogContext.BeginDialogAsync(
                nameof(MeetingInProgressDialog),
                arguments,
                cancellationToken
            ),
            CommandTypes.Initialize => await dialogContext.BeginDialogAsync(
                nameof(InitializeDialog),
                null,
                cancellationToken
            ),
            _ => null,
        };
        if (result is null)
        {
            _ = await turnContext.SendActivityAsync(Messages.CommandIsNotRecognized, cancellationToken: cancellationToken);
        }
    }

}
