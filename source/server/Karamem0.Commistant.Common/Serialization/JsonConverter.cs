//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System.Text.Json;

namespace Karamem0.Commistant.Serialization;

public static class JsonConverter
{

    private static readonly JsonSerializerOptions jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static T? Deserialize<T>(JsonElement value)
    {
        return JsonSerializer.Deserialize<T>(value, jsonSerializerOptions);
    }

    public static T? Deserialize<T>(string value)
    {
        return JsonSerializer.Deserialize<T>(value, jsonSerializerOptions);
    }

    public static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize(value, jsonSerializerOptions);
    }

}
