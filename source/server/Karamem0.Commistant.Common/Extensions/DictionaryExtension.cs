//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Serialization;
using System.Text.Json;

namespace Karamem0.Commistant.Extensions;

public static class DictionaryExtension
{

    public static T? GetValueOrDefault<T>(this IDictionary<string, object?> target, string key)
    {
        if (target.TryGetValue(key, out var value))
        {
            if (value is JsonElement jsonValue)
            {
                return JsonConverter.Deserialize<T>(jsonValue);
            }
            if (value is T typedValue)
            {
                return typedValue;
            }
            throw new InvalidOperationException("値の型が正しくありません");
        }
        else
        {
            return default;
        }
    }

}
