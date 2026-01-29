//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Logging;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Karamem0.Commistant.Routes.Abstraction;

public interface IRouteHandler
{

    Task InvokeAsync(
        ITurnContext turnContext,
        ITurnState turnState,
        CancellationToken cancellationToken
    );

}

public abstract class RouteHandler<T>(ILogger<T> logger) : IRouteHandler where T : IRouteHandler
{

    private readonly ILogger<T> logger = logger;

    public async Task InvokeAsync(
        ITurnContext turnContext,
        ITurnState turnState,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            this.logger.MethodExecuting();
            await this.InvokeAsyncCore(
                turnContext,
                turnState,
                cancellationToken
            );
        }
        finally
        {
            this.logger.MethodExecuted();
        }
    }

    protected abstract Task InvokeAsyncCore(
        ITurnContext turnContext,
        ITurnState turnState,
        CancellationToken cancellationToken = default
    );

}
