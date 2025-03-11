//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Models;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Functions.Tests;

[Category("Karamem0.Commistant.Functions")]
public class ExecuteCommandFunctionTests
{

    [Test()]
    public async Task RunAsync_WhenValid_ShouldSucceed()
    {
        // Setup
        var loggerFactory = Substitute.For<ILoggerFactory>();
        var blobStateClient = Substitute.For<BlobContainerClient>();
        _ = blobStateClient
            .GetBlobsAsync()
            .Returns(
                callInfo =>
                {
                    var asyncPageable = Substitute.For<AsyncPageable<BlobItem>>();
                    _ = asyncPageable
                        .GetAsyncEnumerator()
                        .Returns(
                            callInfo =>
                            {
                                var asyncEnumerator = Substitute.For<IAsyncEnumerator<BlobItem>>();
                                _ = asyncEnumerator
                                    .MoveNextAsync()
                                    .Returns(true, false);
                                _ = asyncEnumerator.Current.Returns(BlobsModelFactory.BlobItem("BotState"));
                                return asyncEnumerator;
                            }
                        );
                    return asyncPageable;
                }
            );
        _ = blobStateClient
            .GetBlobClient(Arg.Any<string>())
            .Returns(
                callInfo =>
                {
                    var blobClient = Substitute.For<BlobClient>();
                    _ = blobClient
                        .ExistsAsync()
                        .Returns(
                            callInfo =>
                            {
                                var blobResponse = Substitute.For<Response<bool>>();
                                _ = blobResponse.Value.Returns(true);
                                return Task.FromResult(blobResponse);
                            }
                        );
                    _ = blobClient
                        .DownloadContentAsync()
                        .Returns(
                            callInfo =>
                            {
                                var blobResponse = Substitute.For<Response<BlobDownloadResult>>();
                                _ = blobResponse.Value.Returns(
                                    BlobsModelFactory.BlobDownloadResult(
                                        content: BinaryData.FromString(
                                            JsonConvert.SerializeObject(
                                                new Dictionary<string, object?>()
                                                {
                                                    ["ConversationProperty"] = new ConversationProperty(),
                                                    ["ConversationReference"] = new ConversationReference()
                                                },
                                                new JsonSerializerSettings()
                                                {
                                                    TypeNameHandling = TypeNameHandling.All
                                                }
                                            )
                                        ),
                                        details: BlobsModelFactory.BlobDownloadDetails(eTag: new ETag())
                                    )
                                );
                                return Task.FromResult(blobResponse);
                            }
                        );
                    return blobClient;
                }
            );
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<ConversationProperty>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        // Execute
        var target = new ExecuteCommandFunction(
            loggerFactory,
            blobStateClient,
            commandSet
        );
        await target.RunAsync(new object());
        // Validate
        _ = commandContext
            .Received(3)
            .ExecuteCommandAsync(Arg.Any<string>());
    }

    [Test()]
    public async Task RunAsync_WhenInvalidData_ShouldSkip()
    {
        // Setup
        var loggerFactory = Substitute.For<ILoggerFactory>();
        var blobStateClient = Substitute.For<BlobContainerClient>();
        _ = blobStateClient
            .GetBlobsAsync()
            .Returns(
                callInfo =>
                {
                    var asyncPageable = Substitute.For<AsyncPageable<BlobItem>>();
                    _ = asyncPageable
                        .GetAsyncEnumerator()
                        .Returns(
                            callInfo =>
                            {
                                var asyncEnumerator = Substitute.For<IAsyncEnumerator<BlobItem>>();
                                _ = asyncEnumerator
                                    .MoveNextAsync()
                                    .Returns(true, false);
                                _ = asyncEnumerator.Current.Returns(BlobsModelFactory.BlobItem("BotState"));
                                return asyncEnumerator;
                            }
                        );
                    return asyncPageable;
                }
            );
        _ = blobStateClient
            .GetBlobClient(Arg.Any<string>())
            .Returns(
                callInfo =>
                {
                    var blobClient = Substitute.For<BlobClient>();
                    _ = blobClient
                        .ExistsAsync()
                        .Returns(
                            callInfo =>
                            {
                                var blobResponse = Substitute.For<Response<bool>>();
                                _ = blobResponse.Value.Returns(true);
                                return Task.FromResult(blobResponse);
                            }
                        );
                    _ = blobClient
                        .DownloadContentAsync()
                        .Returns(
                            callInfo =>
                            {
                                var blobResponse = Substitute.For<Response<BlobDownloadResult>>();
                                _ = blobResponse.Value.Returns(
                                    BlobsModelFactory.BlobDownloadResult(
                                        content: null,
                                        details: BlobsModelFactory.BlobDownloadDetails(eTag: new ETag())
                                    )
                                );
                                return Task.FromResult(blobResponse);
                            }
                        );
                    return blobClient;
                }
            );
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<ConversationProperty>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        // Execute
        var target = new ExecuteCommandFunction(
            loggerFactory,
            blobStateClient,
            commandSet
        );
        await target.RunAsync(new object());
        // Validate
        _ = commandContext
            .DidNotReceive()
            .ExecuteCommandAsync(Arg.Any<string>());
    }

    [Test()]
    public async Task RunAsync_WhenInvalidConversationProperty_ShouldSkip()
    {
        // Setup
        var loggerFactory = Substitute.For<ILoggerFactory>();
        var blobStateClient = Substitute.For<BlobContainerClient>();
        _ = blobStateClient
            .GetBlobsAsync()
            .Returns(
                callInfo =>
                {
                    var asyncPageable = Substitute.For<AsyncPageable<BlobItem>>();
                    _ = asyncPageable
                        .GetAsyncEnumerator()
                        .Returns(
                            callInfo =>
                            {
                                var asyncEnumerator = Substitute.For<IAsyncEnumerator<BlobItem>>();
                                _ = asyncEnumerator
                                    .MoveNextAsync()
                                    .Returns(true, false);
                                _ = asyncEnumerator.Current.Returns(BlobsModelFactory.BlobItem("BotState"));
                                return asyncEnumerator;
                            }
                        );
                    return asyncPageable;
                }
            );
        _ = blobStateClient
            .GetBlobClient(Arg.Any<string>())
            .Returns(
                callInfo =>
                {
                    var blobClient = Substitute.For<BlobClient>();
                    _ = blobClient
                        .ExistsAsync()
                        .Returns(
                            callInfo =>
                            {
                                var blobResponse = Substitute.For<Response<bool>>();
                                _ = blobResponse.Value.Returns(true);
                                return Task.FromResult(blobResponse);
                            }
                        );
                    _ = blobClient
                        .DownloadContentAsync()
                        .Returns(
                            callInfo =>
                            {
                                var blobResponse = Substitute.For<Response<BlobDownloadResult>>();
                                _ = blobResponse.Value.Returns(
                                    BlobsModelFactory.BlobDownloadResult(
                                        content: BinaryData.FromString(
                                            JsonConvert.SerializeObject(
                                                new Dictionary<string, object?>()
                                                {
                                                    ["ConversationProperty"] = new ConversationProperty(),
                                                    ["ConversationReference"] = null
                                                },
                                                new JsonSerializerSettings()
                                                {
                                                    TypeNameHandling = TypeNameHandling.All
                                                }
                                            )
                                        ),
                                        details: BlobsModelFactory.BlobDownloadDetails(eTag: new ETag())
                                    )
                                );
                                return Task.FromResult(blobResponse);
                            }
                        );
                    return blobClient;
                }
            );
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<ConversationProperty>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        // Execute
        var target = new ExecuteCommandFunction(
            loggerFactory,
            blobStateClient,
            commandSet
        );
        await target.RunAsync(new object());
        // Validate
        _ = commandContext
            .DidNotReceive()
            .ExecuteCommandAsync(Arg.Any<string>());
    }

    [Test()]
    public async Task RunAsync_WhenInvalidConversationReference_ShouldSkip()
    {
        // Setup
        var loggerFactory = Substitute.For<ILoggerFactory>();
        var blobStateClient = Substitute.For<BlobContainerClient>();
        _ = blobStateClient
            .GetBlobsAsync()
            .Returns(
                callInfo =>
                {
                    var asyncPageable = Substitute.For<AsyncPageable<BlobItem>>();
                    _ = asyncPageable
                        .GetAsyncEnumerator()
                        .Returns(
                            callInfo =>
                            {
                                var asyncEnumerator = Substitute.For<IAsyncEnumerator<BlobItem>>();
                                _ = asyncEnumerator
                                    .MoveNextAsync()
                                    .Returns(true, false);
                                _ = asyncEnumerator.Current.Returns(BlobsModelFactory.BlobItem("BotState"));
                                return asyncEnumerator;
                            }
                        );
                    return asyncPageable;
                }
            );
        _ = blobStateClient
            .GetBlobClient(Arg.Any<string>())
            .Returns(
                callInfo =>
                {
                    var blobClient = Substitute.For<BlobClient>();
                    _ = blobClient
                        .ExistsAsync()
                        .Returns(
                            callInfo =>
                            {
                                var blobResponse = Substitute.For<Response<bool>>();
                                _ = blobResponse.Value.Returns(true);
                                return Task.FromResult(blobResponse);
                            }
                        );
                    _ = blobClient
                        .DownloadContentAsync()
                        .Returns(
                            callInfo =>
                            {
                                var blobResponse = Substitute.For<Response<BlobDownloadResult>>();
                                _ = blobResponse.Value.Returns(
                                    BlobsModelFactory.BlobDownloadResult(
                                        content: BinaryData.FromString(
                                            JsonConvert.SerializeObject(
                                                new Dictionary<string, object?>()
                                                {
                                                    ["ConversationProperty"] = null,
                                                    ["ConversationReference"] = new ConversationReference()
                                                },
                                                new JsonSerializerSettings()
                                                {
                                                    TypeNameHandling = TypeNameHandling.All
                                                }
                                            )
                                        ),
                                        details: BlobsModelFactory.BlobDownloadDetails(eTag: new ETag())
                                    )
                                );
                                return Task.FromResult(blobResponse);
                            }
                        );
                    return blobClient;
                }
            );
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<ConversationProperty>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        // Execute
        var target = new ExecuteCommandFunction(
            loggerFactory,
            blobStateClient,
            commandSet
        );
        await target.RunAsync(new object());
        // Validate
        _ = commandContext
            .DidNotReceive()
            .ExecuteCommandAsync(Arg.Any<string>());
    }

}
