//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System.Text.Json.Serialization;

namespace Karamem0.Commistant.Models;

public record CommandSettings
{

    [JsonPropertyName("meetingStartedSended")]
    public bool MeetingStartedSended { get; set; }

    [JsonPropertyName("meetingStartedSchedule")]
    public int MeetingStartedSchedule { get; set; } = -1;

    [JsonPropertyName("meetingStartedMessage")]
    public string? MeetingStartedMessage { get; set; }

    [JsonPropertyName("meetingStartedUrl")]
    public string? MeetingStartedUrl { get; set; }

    [JsonPropertyName("meetingEndingSended")]
    public bool MeetingEndingSended { get; set; }

    [JsonPropertyName("meetingEndingSchedule")]
    public int MeetingEndingSchedule { get; set; } = -1;

    [JsonPropertyName("meetingEndingMessage")]
    public string? MeetingEndingMessage { get; set; }

    [JsonPropertyName("meetingEndingUrl")]
    public string? MeetingEndingUrl { get; set; }

    [JsonPropertyName("meetingInProgress")]
    public bool MeetingInProgress { get; set; }

    [JsonPropertyName("meetingInProgressSchedule")]
    public int MeetingInProgressSchedule { get; set; } = -1;

    [JsonPropertyName("meetingInProgressMessage")]
    public string? MeetingInProgressMessage { get; set; }

    [JsonPropertyName("meetingInProgressUrl")]
    public string? MeetingInProgressUrl { get; set; }

    [JsonPropertyName("scheduledEndTime")]
    public DateTime? ScheduledEndTime { get; set; }

    [JsonPropertyName("scheduledStartTime")]
    public DateTime? ScheduledStartTime { get; set; }

}
