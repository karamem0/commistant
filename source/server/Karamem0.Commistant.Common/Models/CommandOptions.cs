//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

namespace Karamem0.Commistant.Models;

public record CommandOptions
{

    public required string Type { get; set; }

    public required CommandOptionsValue Value { get; set; }

}
