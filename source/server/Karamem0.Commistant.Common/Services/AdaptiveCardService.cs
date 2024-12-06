//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AdaptiveCards.Templating;
using Karamem0.Commistant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Services;

public interface IAdaptiveCardService
{

    Task<string> GetCardAsync(string template, AdaptiveCardTemplateArguments arguments);

}

public class AdaptiveCardService : IAdaptiveCardService
{

    private static readonly string templatePath = "Karamem0.Commistant.Cards";

    public async Task<string> GetCardAsync(string template, AdaptiveCardTemplateArguments arguments)
    {
        var path = $"{templatePath}.{template}.json";
        using var stream = this.GetType().Assembly.GetManifestResourceStream(path) ?? throw new InvalidOperationException();
        using var reader = new StreamReader(stream);
        return new AdaptiveCardTemplate(await reader.ReadToEndAsync()).Expand(arguments);
    }

}
