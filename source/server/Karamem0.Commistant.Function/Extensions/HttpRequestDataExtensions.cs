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

    public static string? GetBearerToken(this HttpRequestData target)
    {
        if (target.Headers.TryGetValues("Authorization", out var headerValues) is false)
        {
            return null;
        }
        var headerValue = headerValues.SingleOrDefault();
        if (headerValue is null)
        {
            return null;
        }
        var headerPairs = headerValue.Split(' ');
        if (headerPairs.Length != 2 || headerPairs[0] != "Bearer")
        {
            return null;
        }
        return headerPairs[1];
    }

}
