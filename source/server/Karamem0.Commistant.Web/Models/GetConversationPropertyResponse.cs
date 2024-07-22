//
// Copyright (c) 2022-2024 karamem0
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

public class GetConversationPropertyResponse
{

    public GetConversationPropertyResponse()
    {
    }

    public string? ChannelId { get; set; }

    public string? MeetingId { get; set; }

    public bool IsOrganizer { get; set; }

    public int StartMeetingSchedule { get; set; } = -1;

    public string? StartMeetingMessage { get; set; }

    public string? StartMeetingUrl { get; set; }

    public int EndMeetingSchedule { get; set; } = -1;

    public string? EndMeetingMessage { get; set; }

    public string? EndMeetingUrl { get; set; }

    public int InMeetingSchedule { get; set; } = -1;

    public string? InMeetingMessage { get; set; }

    public string? InMeetingUrl { get; set; }

}
