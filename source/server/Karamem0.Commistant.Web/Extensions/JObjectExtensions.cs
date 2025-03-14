//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Extensions;

public static class JObjectExtensions
{

    public static T? Value<T>(
        this JObject target,
        string propertyName,
        T? defaultValue
    )
    {
        if (target.ContainsKey(propertyName))
        {
            return target.Value<T>(propertyName);
        }
        else
        {
            return defaultValue;
        }
    }

}
