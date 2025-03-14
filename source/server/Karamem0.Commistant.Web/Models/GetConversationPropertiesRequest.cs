//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Models;

public record GetConversationPropertiesRequest
{

    [Required(ErrorMessage = "ChannelId は必須です。")]
    public string? ChannelId { get; set; }

    [Required(ErrorMessage = "MeetingId は必須です。")]
    public string? MeetingId { get; set; }

}
