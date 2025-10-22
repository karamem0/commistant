//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

namespace Karamem0.Commistant.Models;

public record CommandSettings
{

    public bool MeetingStartSended { get; set; }

    public int MeetingStartSchedule { get; set; } = -1;

    public string? MeetingStartMessage { get; set; }

    public string? MeetingStartUrl { get; set; }

    public bool MeetingEndSended { get; set; }

    public int MeetingEndSchedule { get; set; } = -1;

    public string? MeetingEndMessage { get; set; }

    public string? MeetingEndUrl { get; set; }

    public bool MeetingRunning { get; set; }

    public int MeetingRunSchedule { get; set; } = -1;

    public string? MeetingRunMessage { get; set; }

    public string? MeetingRunUrl { get; set; }

    public DateTime? ScheduledEndTime { get; set; }

    public DateTime? ScheduledStartTime { get; set; }

}
