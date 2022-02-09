//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Logging
{

    public static class LoggerExtensions
    {

        private static readonly Action<ILogger, Exception> unhandledError =
            LoggerMessage.Define(LogLevel.Error, new EventId(1), "予期しない問題が発生しました。");

        public static void UnhandledError(this ILogger logger, Exception exception)
        {
            unhandledError.Invoke(logger, exception);
        }

    }

}
