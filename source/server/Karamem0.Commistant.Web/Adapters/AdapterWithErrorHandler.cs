//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Logging;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Logging;

namespace Karamem0.Commistant.Adapters;

public class AdapterWithErrorHandler : CloudAdapter
{

    public AdapterWithErrorHandler(BotFrameworkAuthentication auth, ILogger<AdapterWithErrorHandler> logger)
        : base(auth, logger)
    {
        this.OnTurnError = async (turnContext, exception) =>
        {
            logger.UnhandledErrorOccurred(exception: exception);
            _ = await turnContext.SendActivityAsync("予期しない問題が発生しました。");
        };
    }

}
