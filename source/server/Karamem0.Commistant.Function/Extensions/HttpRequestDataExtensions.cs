//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Extensions;

public static partial class HttpRequestDataExtensions
{

    public static string? GetUserId(this HttpRequestData target)
    {
        if (target.Headers.TryGetValues("X-MS-CLIENT-PRINCIPAL-ID", out var headerValues) is false)
        {
            return null;
        }
        return headerValues.Single();
    }

}
