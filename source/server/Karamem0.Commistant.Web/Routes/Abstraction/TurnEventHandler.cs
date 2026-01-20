//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using System.Threading;

namespace Karamem0.Commistant.Routes.Abstraction;

public interface ITurnEventHandler
{

    Task<bool> InvokeAsync(
        ITurnContext turnContext,
        ITurnState turnState,
        CancellationToken cancellationToken
    );

}

public abstract class TurnEventHandler : ITurnEventHandler
{

    public abstract Task<bool> InvokeAsync(
        ITurnContext turnContext,
        ITurnState turnState,
        CancellationToken cancellationToken
    );

}
