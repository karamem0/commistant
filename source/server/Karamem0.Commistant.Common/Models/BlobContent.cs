//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure;

namespace Karamem0.Commistant.Models;

public record BlobContent<T>
{

    public T? Data { get; set; }

    public ETag? ETag { get; set; }

}
