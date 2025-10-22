//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Bot.Builder.Dialogs;
using System.Threading;

namespace Karamem0.Commistant.Validators;

public static class AdaptiveCardValidator
{

    public static Task<bool> Validate(PromptValidatorContext<string> promptContext, CancellationToken _)
    {
        if (promptContext.Recognized.Succeeded)
        {
            return Task.FromResult(false);
        }
        var value = promptContext.Context.Activity.Value;
        if (value is null)
        {
            return Task.FromResult(false);
        }
        return Task.FromResult(true);
    }

}
