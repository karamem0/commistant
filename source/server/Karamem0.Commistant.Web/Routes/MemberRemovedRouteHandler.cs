//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Routes.Abstraction;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Karamem0.Commistant.Routes;

public class MemberRemovedRouteHandler(ConversationState conversationState, ILogger<MemberRemovedRouteHandler> logger)
    : RouteHandler<MemberRemovedRouteHandler>(logger)
{

    private readonly ConversationState conversationState = conversationState;

    private readonly ILogger<MemberRemovedRouteHandler> logger = logger;

    protected override async Task InvokeAsyncCore(
        ITurnContext turnContext,
        ITurnState turnState,
        CancellationToken cancellationToken = default
    )
    {
        this.logger.MembersRemoved(conversationId: turnContext.Activity.Conversation.Id);
        foreach (var member in turnContext.Activity.MembersRemoved)
        {
            if (member.Id == turnContext.Activity.Recipient.Id)
            {
                await this.conversationState.DeleteStateAsync(turnContext, cancellationToken);
            }
        }
    }

}
