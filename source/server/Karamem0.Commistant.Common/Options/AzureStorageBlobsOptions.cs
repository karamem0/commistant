//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

namespace Karamem0.Commistant.Options;

public record AzureStorageBlobsOptions
{

    public required Uri Endpoint { get; set; }

    public required string ContainerName { get; set; }

}
