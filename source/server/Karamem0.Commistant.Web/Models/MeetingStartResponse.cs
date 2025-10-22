//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

namespace Karamem0.Commistant.Models;

public record MeetingStartResponse
{

    public required string Button { get; set; }

    public required int Schedule { get; set; } = -1;

    public required string Message { get; set; }

    public required string Url { get; set; }

}
