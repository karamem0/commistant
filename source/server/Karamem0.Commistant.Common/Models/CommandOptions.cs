//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System.Text.Json.Serialization;

namespace Karamem0.Commistant.Models;

public record CommandOptions
{

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("value")]
    public CommandOptionsValue? Value { get; set; }

}
