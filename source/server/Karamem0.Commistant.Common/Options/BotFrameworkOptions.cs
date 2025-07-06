//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Options;

public record BotFrameworkOptions
{

    public required string MicrosoftAppId { get; set; }

    public required string MicrosoftAppPassword { get; set; }

}
