//
// Copyright (c) 2022-2024 karamem0
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

namespace Karamem0.Commistant.Extensions
{

    public static class DictionaryExtension
    {

        public static T? GetValueOrDefault<T>(this IDictionary<string, object> target, string key)
        {
            if (target.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return default;
        }

    }

}
