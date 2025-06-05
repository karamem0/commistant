//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards;
using AdaptiveCards.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Helpers;

public static class AdaptiveCardHelper
{

    public static async Task<AdaptiveCard> CreateViewCardAsync(string name, object? rootData = null)
    {
        var json = await File.ReadAllTextAsync(
            Path.Combine(
                AppContext.BaseDirectory,
                "Resources",
                "ViewCard",
                $"{name}.json"
            )
        );
        var template = new AdaptiveCardTemplate(json);
        var card = template.Expand(rootData);
        return AdaptiveCard.FromJson(card)
            .Card;
    }

    public static async Task<AdaptiveCard> CreateEditCardAsync(string name, object? rootData = null)
    {
        var json = await File.ReadAllTextAsync(
            Path.Combine(
                AppContext.BaseDirectory,
                "Resources",
                "EditCard",
                $"{name}.json"
            )
        );
        var template = new AdaptiveCardTemplate(json);
        var card = template.Expand(rootData);
        return AdaptiveCard.FromJson(card)
            .Card;
    }

}
