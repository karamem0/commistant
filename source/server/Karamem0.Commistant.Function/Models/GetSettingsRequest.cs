//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System.Text.Json.Serialization;

namespace Karamem0.Commistant.Models;

public record GetSettingsRequest
{

    [JsonPropertyName("channelId")]
    public required string ChannelId { get; set; }

    [JsonPropertyName("meetingId")]
    public required string MeetingId { get; set; }

}
