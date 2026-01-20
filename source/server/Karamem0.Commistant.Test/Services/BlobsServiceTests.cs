//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NSubstitute;
using NUnit.Framework;

namespace Karamem0.Commistant.Services.Tests;

[Category("Karamem0.Commistant.Services")]
public class BlobsServiceTests
{

    [Test()]
    public async Task GetBlobNamesAsync_Success()
    {
        // Setup
        var blobContainerClient = Substitute.For<BlobContainerClient>();
        _ = blobContainerClient
            .GetBlobsAsync()
            .Returns(
                AsyncPageable<BlobItem>.FromPages(
                    [
                        Page<BlobItem>.FromValues(
                            new List<BlobItem>()
                            {
                                BlobsModelFactory.BlobItem(name: "item1"),
                                BlobsModelFactory.BlobItem(name: "item2"),
                                BlobsModelFactory.BlobItem(name: "item3")
                            },
                            null,
                            Substitute.For<Response>()
                        )
                    ]
                )
            );
        // Execute
        var target = new BlobsService(blobContainerClient);
        var actual = await target
            .GetBlobNamesAsync()
            .ToListAsync();
        // Assert
        using (Assert.EnterMultipleScope())
        {
            Assert.That(actual[0], Is.EqualTo("item1"));
            Assert.That(actual[1], Is.EqualTo("item2"));
            Assert.That(actual[2], Is.EqualTo("item3"));
        }
    }

    [Test()]
    public async Task GetObjectAsync_Success()
    {
        // Setup
        var blobClient = Substitute.For<BlobClient>();
        _ = blobClient
            .ExistsAsync(default)
            .Returns(Response.FromValue(true, Substitute.For<Response>()));
        _ = blobClient
            .DownloadContentAsync(default)
            .Returns(
                Response.FromValue(
                    BlobsModelFactory.BlobDownloadResult(
                        content: BinaryData.FromObjectAsJson("value1"),
                        details: BlobsModelFactory.BlobDownloadDetails(eTag: new ETag())
                    ),
                    Substitute.For<Response>()
                )
            );
        var blobContainerClient = Substitute.For<BlobContainerClient>();
        _ = blobContainerClient
            .GetBlobClient(Arg.Any<string>())
            .Returns(blobClient);
        // Execute
        var target = new BlobsService(blobContainerClient);
        var actual = await target.GetObjectAsync<string>("item1");
        // Assert
        Assert.That(actual?.Data, Is.EqualTo("value1"));
    }

}
