//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

namespace Karamem0.Commistant.Options;

public record AzureOpenAIOptions
{

    public required Uri Endpoint { get; set; }

    public required string DeploymentName { get; set; }

}
