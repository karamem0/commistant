//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Extensions;

public static partial class HttpRequestExtensions
{

    public static string? GetUserId(this HttpRequest target)
    {
        if (target.Headers.TryGetValue("X-MS-CLIENT-PRINCIPAL-ID", out var headerValues))
        {
            return headerValues.Single();
        }
        return null;
    }

}
