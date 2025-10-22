//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

namespace Karamem0.Commistant.Extensions;

public static class DictionaryExtension
{

    public static T? GetValueOrDefault<T>(this IDictionary<string, object?> target, string key)
    {
        if (target.TryGetValue(key, out var value))
        {
            return (T?)value;
        }
        else
        {
            return default;
        }
    }

}
