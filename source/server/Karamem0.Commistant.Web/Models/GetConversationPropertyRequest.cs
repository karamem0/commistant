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

public class GetConversationPropertyRequest
{

    public GetConversationPropertyRequest()
    {
    }

    public string? ChannelId { get; set; }

    public string? MeetingId { get; set; }

}
