//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Logging;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Hosting.AspNetCore;
using Microsoft.Agents.Hosting.AspNetCore.BackgroundQueue;
using Microsoft.Extensions.Logging;

namespace Karamem0.Commistant.Adapters;

public class AdapterWithErrorHandler : CloudAdapter
{

    public AdapterWithErrorHandler(
        IChannelServiceClientFactory factory,
        IActivityTaskQueue activityTaskQueue,
        ILogger<AdapterWithErrorHandler> logger
    )
        : base(
            factory,
            activityTaskQueue,
            logger
        )
    {
        this.OnTurnError = async (turnContext, exception) =>
        {
            logger.UnhandledErrorOccurred(exception: exception);
            _ = await turnContext.SendActivityAsync($"予期しない問題が発生しました: {exception.Message}");
        };
    }

}
