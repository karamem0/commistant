//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Karamem0.Commistant.Types;
using Microsoft.Agents.Builder.Dialogs;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Core.Models;
using System.Threading;

namespace Karamem0.Commistant.Dialogs;

public class MainDialog(
    ConversationState conversationState,
    MeetingStartedDialog meetingStartedDialog,
    MeetingEndingDialog meetingEndingDialog,
    MeetingInProgressDialog meetingInProgressDialog,
    InitializeDialog initializeDialog,
    IMeetingService meetingService,
    IOpenAIService openAIService
) : ComponentDialog(nameof(MainDialog))
{

    private readonly ConversationState conversationState = conversationState;

    private readonly MeetingStartedDialog meetingStartedDialog = meetingStartedDialog;

    private readonly MeetingEndingDialog meetingEndingDialog = meetingEndingDialog;

    private readonly MeetingInProgressDialog meetingInProgressDialog = meetingInProgressDialog;

    private readonly InitializeDialog initializeDialog = initializeDialog;

    private readonly IMeetingService meetingService = meetingService;

    private readonly IOpenAIService openAIService = openAIService;

    protected override async Task OnInitializeAsync(DialogContext dialogContext)
    {
        _ = this.AddDialog(
            new WaterfallDialog(
                nameof(WaterfallDialog),
                [
                    this.OnBeforeAsync,
                    this.OnAfterAsync,
                    this.OnFinalizeAsync
                ]
            )
        );
        _ = this.AddDialog(this.meetingStartedDialog);
        _ = this.AddDialog(this.meetingEndingDialog);
        _ = this.AddDialog(this.meetingInProgressDialog);
        _ = this.AddDialog(this.initializeDialog);
        await base.OnInitializeAsync(dialogContext);
    }

    private async Task<DialogTurnResult> OnBeforeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(new DialogTurnResult(DialogTurnStatus.Waiting));
    }

    private async Task<DialogTurnResult> OnAfterAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
    {
        var participant = await this.meetingService.GetMeetingParticipantAsync(
            stepContext.Context,
            participantId: stepContext.Context.Activity.Recipient.AadObjectId,
            cancellationToken: cancellationToken
        );
        if (participant.Meeting.Role != "Organizer")
        {
            _ = await stepContext.Context.SendActivityAsync(Messages.UserIsNotOrganizer, cancellationToken: cancellationToken);
            _ = await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
            return await stepContext.BeginDialogAsync(nameof(WaterfallDialog), cancellationToken: cancellationToken);
        }
        var command = stepContext.Context.Activity.RemoveRecipientMention();
        var commandSettings = this.conversationState.GetValue<CommandSettings>(nameof(CommandSettings), () => new());
        if (commandSettings.MeetingInProgress is true)
        {
            _ = await stepContext.Context.SendActivityAsync(Messages.SettingsCannotUpdateWhenMeetingInProgress, cancellationToken: cancellationToken);
            _ = await stepContext.EndDialogAsync(null, cancellationToken);
            return await stepContext.BeginDialogAsync(
                nameof(WaterfallDialog),
                null,
                cancellationToken
            );
        }
        var arguments = await this.openAIService.GetCommandOptionsAsync(command, cancellationToken);
        switch (arguments?.Type)
        {
            case CommandTypes.MeetingStarted:
                return await stepContext.BeginDialogAsync(
                    nameof(MeetingStartedDialog),
                    arguments,
                    cancellationToken
                );
            case CommandTypes.MeetingEnding:
                return await stepContext.BeginDialogAsync(
                    nameof(MeetingEndingDialog),
                    arguments,
                    cancellationToken
                );
            case CommandTypes.MeetingInProgress:
                return await stepContext.BeginDialogAsync(
                    nameof(MeetingInProgressDialog),
                    arguments,
                    cancellationToken
                );
            case CommandTypes.Initialize:
                return await stepContext.BeginDialogAsync(
                    nameof(InitializeDialog),
                    arguments,
                    cancellationToken
                );
            default:
                _ = await stepContext.Context.SendActivityAsync(Messages.CommandIsNotRecognized, cancellationToken: cancellationToken);
                _ = await stepContext.EndDialogAsync(null, cancellationToken);
                return await stepContext.BeginDialogAsync(
                    nameof(WaterfallDialog),
                    null,
                    cancellationToken
                );
        }
    }

    private async Task<DialogTurnResult> OnFinalizeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken = default)
    {
        _ = await stepContext.EndDialogAsync(null, cancellationToken);
        return await stepContext.BeginDialogAsync(
            nameof(WaterfallDialog),
            null,
            cancellationToken
        );
    }

}
