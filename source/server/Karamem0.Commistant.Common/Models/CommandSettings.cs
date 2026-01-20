//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

namespace Karamem0.Commistant.Models;

public record CommandSettings
{

    public bool MeetingStartedSended { get; set; }

    public int MeetingStartedSchedule { get; set; } = -1;

    public string? MeetingStartedMessage { get; set; }

    public string? MeetingStartedUrl { get; set; }

    public bool MeetingEndingSended { get; set; }

    public int MeetingEndingSchedule { get; set; } = -1;

    public string? MeetingEndingMessage { get; set; }

    public string? MeetingEndingUrl { get; set; }

    public bool MeetingInProgress { get; set; }

    public int MeetingInProgressSchedule { get; set; } = -1;

    public string? MeetingInProgressMessage { get; set; }

    public string? MeetingInProgressUrl { get; set; }

    public DateTime? ScheduledEndTime { get; set; }

    public DateTime? ScheduledStartTime { get; set; }

}
