//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Karamem0.Commistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Karamem0.Commistant.Services;

public interface IBlobsStorageService
{

    IAsyncEnumerable<string> GetNamesAsync(CancellationToken cancellationToken = default);

    Task<BlobContent<T>> GetObjectAsync<T>(string name, CancellationToken cancellationToken = default);

    Task<BlobContent<T>> GetObjectAsync<T>(
        string channelId,
        string meetingId,
        CancellationToken cancellationToken = default
    );

    Task SetObjectAsync<T>(
        string name,
        BlobContent<T> value,
        CancellationToken cancellationToken = default
    );

    Task SetObjectAsync<T>(
        string channelId,
        string meetingId,
        BlobContent<T> value,
        CancellationToken cancellationToken = default
    );

}

public class BlobsStorageService(BlobContainerClient blobContainerClient) : IBlobsStorageService
{

    private readonly BlobContainerClient blobContainerClient = blobContainerClient;

    public async IAsyncEnumerable<string> GetNamesAsync([EnumeratorCancellation()] CancellationToken cancellationToken = default)
    {
        await foreach (var blobItem in this.blobContainerClient.GetBlobsAsync(cancellationToken: cancellationToken))
        {
            yield return blobItem.Name;
        }
    }

    public async Task<BlobContent<T>> GetObjectAsync<T>(string name, CancellationToken cancellationToken = default)
    {
        var client = this.blobContainerClient.GetBlobClient(name);
        var value = new BlobContent<T>();
        var exists = await client.ExistsAsync(cancellationToken);
        if (exists.Value)
        {
            var download = await client.DownloadContentAsync(cancellationToken);
            value.Data = JsonSerializer.Deserialize<T>(download.Value.Content.ToString());
            value.ETag = download.Value.Details.ETag;
        }
        return value;
    }

    public async Task<BlobContent<T>> GetObjectAsync<T>(
        string channelId,
        string meetingId,
        CancellationToken cancellationToken = default
    )
    {
        return await this.GetObjectAsync<T>(HttpUtility.UrlEncode($"{channelId}/conversations/{meetingId}"), cancellationToken);
    }

    public async Task SetObjectAsync<T>(
        string name,
        BlobContent<T> value,
        CancellationToken cancellationToken = default
    )
    {
        var client = this.blobContainerClient.GetBlobClient(name);
        var upload = BinaryData.FromString(JsonSerializer.Serialize(value.Data));
        _ = await client.UploadAsync(
            upload,
            new BlobUploadOptions()
            {
                Conditions = new BlobRequestConditions()
                {
                    IfMatch = value.ETag
                }
            },
            cancellationToken
        );
    }

    public async Task SetObjectAsync<T>(
        string channelId,
        string meetingId,
        BlobContent<T> value,
        CancellationToken cancellationToken = default
    )
    {
        await this.SetObjectAsync(
            HttpUtility.UrlEncode($"{channelId}/conversations/{meetingId}"),
            value,
            cancellationToken
        );
    }

}
