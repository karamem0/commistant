//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Models;

public record GetSettingsResponse
{

    [JsonPropertyName("isOrganizer")]
    public bool IsOrganizer { get; set; } = false;

    [JsonPropertyName("channelId")]
    public string? ChannelId { get; set; }

    [JsonPropertyName("meetingId")]
    public string? MeetingId { get; set; }

    [JsonPropertyName("meetingStartSchedule")]
    public int MeetingStartSchedule { get; set; } = -1;

    [JsonPropertyName("meetingStartMessage")]
    public string? MeetingStartMessage { get; set; }

    [JsonPropertyName("meetingStartUrl")]
    public string? MeetingStartUrl { get; set; }

    [JsonPropertyName("meetingEndSchedule")]
    public int MeetingEndSchedule { get; set; } = -1;

    [JsonPropertyName("meetingEndMessage")]
    public string? MeetingEndMessage { get; set; }

    [JsonPropertyName("meetingEndUrl")]
    public string? MeetingEndUrl { get; set; }

    [JsonPropertyName("meetingRunSchedule")]
    public int MeetingRunSchedule { get; set; } = -1;

    [JsonPropertyName("meetingRunMessage")]
    public string? MeetingRunMessage { get; set; }

    [JsonPropertyName("meetingRunUrl")]
    public string? MeetingRunUrl { get; set; }

}
