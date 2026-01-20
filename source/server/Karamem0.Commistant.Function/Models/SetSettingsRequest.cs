//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System.Text.Json.Serialization;

namespace Karamem0.Commistant.Models;

public record SetSettingsRequest
{

    [JsonPropertyName("channelId")]
    public required string ChannelId { get; set; }

    [JsonPropertyName("meetingId")]
    public required string MeetingId { get; set; }

    [JsonPropertyName("meetingStartedSchedule")]
    public int MeetingStartedSchedule { get; set; } = -1;

    [JsonPropertyName("meetingStartedMessage")]
    public string? MeetingStartedMessage { get; set; }

    [JsonPropertyName("meetingStartedUrl")]
    public string? MeetingStartedUrl { get; set; }

    [JsonPropertyName("meetingEndingSchedule")]
    public int MeetingEndingSchedule { get; set; } = -1;

    [JsonPropertyName("meetingEndingMessage")]
    public string? MeetingEndingMessage { get; set; }

    [JsonPropertyName("meetingEndingUrl")]
    public string? MeetingEndingUrl { get; set; }

    [JsonPropertyName("meetingInProgressSchedule")]
    public int MeetingInProgressSchedule { get; set; } = -1;

    [JsonPropertyName("meetingInProgressMessage")]
    public string? MeetingInProgressMessage { get; set; }

    [JsonPropertyName("meetingInProgressUrl")]
    public string? MeetingInProgressUrl { get; set; }

}
