//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Routes.Abstraction;
using Karamem0.Commistant.Services;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Karamem0.Commistant.Routes;

public class MessageRouteHandler(
    ConversationState conversationState,
    IDialogService<MainDialog> dialogService,
    ILogger<MessageRouteHandler> logger
) : RouteHandler
{

    private readonly ConversationState conversationState = conversationState;

    private readonly IDialogService<MainDialog> dialogService = dialogService;

    private readonly ILogger<MessageRouteHandler> logger = logger;

    public override async Task InvokeAsync(
        ITurnContext turnContext,
        ITurnState turnState,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            this.logger.MethodExecuting();
            this.logger.MessageReceived(conversationId: turnContext.Activity.Conversation.Id);
            _ = await this.dialogService.RunAsync(
                turnContext,
                this.conversationState,
                cancellationToken
            );
        }
        finally
        {
            this.logger.MethodExecuted();
        }
    }

}
