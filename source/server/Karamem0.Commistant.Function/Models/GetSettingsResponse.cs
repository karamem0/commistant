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

    [JsonPropertyName("startMeetingSchedule")]
    public int StartMeetingSchedule { get; set; } = -1;

    [JsonPropertyName("startMeetingMessage")]
    public string? StartMeetingMessage { get; set; }

    [JsonPropertyName("startMeetingUrl")]
    public string? StartMeetingUrl { get; set; }

    [JsonPropertyName("endMeetingSchedule")]
    public int EndMeetingSchedule { get; set; } = -1;

    [JsonPropertyName("endMeetingMessage")]
    public string? EndMeetingMessage { get; set; }

    [JsonPropertyName("endMeetingUrl")]
    public string? EndMeetingUrl { get; set; }

    [JsonPropertyName("inMeetingSchedule")]
    public int InMeetingSchedule { get; set; } = -1;

    [JsonPropertyName("inMeetingMessage")]
    public string? InMeetingMessage { get; set; }

    [JsonPropertyName("inMeetingUrl")]
    public string? InMeetingUrl { get; set; }

}
