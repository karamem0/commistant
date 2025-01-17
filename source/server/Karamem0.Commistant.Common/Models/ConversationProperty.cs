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
using System.Threading.Tasks;

namespace Karamem0.Commistant.Models;

public record ConversationProperty
{

    public bool StartMeetingSended { get; set; }

    public int StartMeetingSchedule { get; set; } = -1;

    public string? StartMeetingMessage { get; set; }

    public string? StartMeetingUrl { get; set; }

    public bool EndMeetingSended { get; set; }

    public int EndMeetingSchedule { get; set; } = -1;

    public string? EndMeetingMessage { get; set; }

    public string? EndMeetingUrl { get; set; }

    public bool InMeeting { get; set; }

    public int InMeetingSchedule { get; set; } = -1;

    public string? InMeetingMessage { get; set; }

    public string? InMeetingUrl { get; set; }

    public DateTime? ScheduledEndTime { get; set; }

    public DateTime? ScheduledStartTime { get; set; }

}
