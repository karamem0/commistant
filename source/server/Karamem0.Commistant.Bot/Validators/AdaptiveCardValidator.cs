//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Validators
{

    public static class AdaptiveCardvalidator
    {

        public static Task<bool> Validate(PromptValidatorContext<string> promptContext, CancellationToken _)
        {
            if (promptContext.Recognized.Succeeded)
            {
                return Task.FromResult(false);
            }
            var value = promptContext.Context.Activity.Value.ToString();
            if (value is null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }

    }

}
