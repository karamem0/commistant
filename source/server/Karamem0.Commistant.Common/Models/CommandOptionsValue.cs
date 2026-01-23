//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System.Text.Json.Serialization;

namespace Karamem0.Commistant.Models;

public record CommandOptionsValue
{

    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("schedule")]
    public int Schedule { get; set; } = -1;

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

}
