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
using Karamem0.Commistant.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Karamem0.Commistant.Services;

public interface IBlobService
{

    IAsyncEnumerable<string> GetBlobNamesAsync(CancellationToken cancellationToken = default);

    Task<BlobContent<T>> GetObjectAsync<T>(string name, CancellationToken cancellationToken = default);

    Task SetObjectAsync<T>(
        string name,
        BlobContent<T> content,
        CancellationToken cancellationToken = default
    );

    Task SetObjectAsync<T>(
        string channelId,
        string meetingId,
        BlobContent<T> content,
        CancellationToken cancellationToken = default
    );

}

public class BlobService(BlobContainerClient blobContainerClient) : IBlobService
{

    private readonly BlobContainerClient blobContainerClient = blobContainerClient;

    public async IAsyncEnumerable<string> GetBlobNamesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var blobItem in this.blobContainerClient.GetBlobsAsync(cancellationToken: cancellationToken))
        {
            yield return blobItem.Name;
        }
    }

    public async Task<BlobContent<T>> GetObjectAsync<T>(string name, CancellationToken cancellationToken = default)
    {
        var client = this.blobContainerClient.GetBlobClient(name);
        var content = new BlobContent<T>();
        var exists = await client.ExistsAsync(cancellationToken);
        if (exists.Value)
        {
            var download = await client.DownloadContentAsync(cancellationToken);
            content.Data = JsonSerializer.Deserialize<T>(download.Value.Content.ToString());
            content.ETag = download.Value.Details.ETag;
        }
        return content;
    }

    public async Task SetObjectAsync<T>(
        string name,
        BlobContent<T> content,
        CancellationToken cancellationToken = default
    )
    {
        var client = this.blobContainerClient.GetBlobClient(name);
        var upload = BinaryData.FromString(JsonSerializer.Serialize(content.Data));
        _ = await client.UploadAsync(
            upload,
            new BlobUploadOptions()
            {
                Conditions = new BlobRequestConditions()
                {
                    IfMatch = content.ETag
                }
            },
            cancellationToken
        );
    }

    public Task SetObjectAsync<T>(
        string channelId,
        string meetingId,
        BlobContent<T> content,
        CancellationToken cancellationToken = default
    )
    {
        return this.SetObjectAsync<T>(
            HttpUtility.UrlEncode($"{channelId}/conversations/{meetingId}"),
            content,
            cancellationToken
        );
    }

}
