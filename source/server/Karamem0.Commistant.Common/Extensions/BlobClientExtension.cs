//
// Copyright (c) 2023 karamem0
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Extensions
{

    public static class BlobClientExtension
    {

        public static async Task<BlobContent<T>> GetObjectAsync<T>(this BlobClient target)
        {
            var value = new BlobContent<T>();
            var exists = await target.ExistsAsync();
            if (exists.Value)
            {
                var content = await target.DownloadContentAsync();
                value.Data = JsonSerializer.Deserialize<T>(content.Value.Content.ToString());
                value.ETag = content.Value.Details.ETag;
            }
            return value;
        }

        public static async Task SetObjectAsync<T>(this BlobClient target, BlobContent<T> value)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value.Data)));
            _ = await target.UploadAsync(
                stream,
                new BlobUploadOptions()
                {
                    Conditions = new BlobRequestConditions()
                    {
                        IfMatch = value.ETag
                    }
                }
            );
        }

    }

}
