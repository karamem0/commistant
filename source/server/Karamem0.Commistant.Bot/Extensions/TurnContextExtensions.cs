//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Extensions;

public static class TurnContextExtensions
{

    public static async Task<ResourceResponse> SendSettingsCancelledAsync(this ITurnContext target, CancellationToken cancellationToken = default)
    {
        return await target.SendActivityAsync(
            "キャンセルしました。設定は変更されていません。",
            cancellationToken: cancellationToken
        );
    }

    public static async Task<ResourceResponse> SendSettingsResetedAsync(this ITurnContext target, CancellationToken cancellationToken = default)
    {
        return await target.SendActivityAsync(
            "設定を初期化しました。",
            cancellationToken: cancellationToken
        );
    }

    public static async Task<ResourceResponse> SendSettingsUpdatedAsync(this ITurnContext target, CancellationToken cancellationToken = default)
    {
        return await target.SendActivityAsync(
            "設定を変更しました。",
            cancellationToken: cancellationToken
        );
    }

}