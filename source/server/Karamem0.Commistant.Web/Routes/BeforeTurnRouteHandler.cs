//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Karamem0.Commistant.Routes.Abstraction;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Core.Models;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Karamem0.Commistant.Routes;

public class BeforeTurnRouteHandler(ConversationState conversationState, ILogger<BeforeTurnRouteHandler> logger)
    : TurnEventHandler<BeforeTurnRouteHandler>(logger)
{

    private readonly ConversationState conversationState = conversationState;

    protected override async Task<bool> InvokeAsyncCore(
        ITurnContext turnContext,
        ITurnState turnState,
        CancellationToken cancellationToken = default
    )
    {
        this.conversationState.SetValue(nameof(ConversationReference), turnContext.Activity.GetConversationReference());
        this.conversationState.SetValue(nameof(CommandSettings), this.conversationState.GetValue<CommandSettings>(nameof(CommandSettings), () => new()));
        return await Task.FromResult(true);
    }

}
