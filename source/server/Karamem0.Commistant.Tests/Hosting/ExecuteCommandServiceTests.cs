//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Hosting.Tests;

[Category("Karamem0.Commistant.Hosting")]
public class ExecuteCommandServiceTests
{

    [Test()]
    public async Task ExecuteAsync_WhenValid_ShouldSucceed()
    {
        // Setup
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<ConversationProperties>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        var blobsStorageService = Substitute.For<IBlobsStorageService>();
        _ = blobsStorageService
            .GetNamesAsync(Arg.Any<CancellationToken>())
            .Returns(
                new List<string>()
                {
                    "BotState"
                }.ToAsyncEnumerable()
            );
        _ = blobsStorageService
            .GetObjectAsync<Dictionary<string, object?>>("BotState", Arg.Any<CancellationToken>())
            .Returns(
                new BlobContent<Dictionary<string, object?>>()
                {
                    Data = new Dictionary<string, object?>()
                    {
                        ["ConversationProperties"] = new ConversationProperties(),
                        ["ConversationReference"] = new ConversationReference()
                    },
                    ETag = new ETag()
                }
            );
        var hostApplicationLifetime = Substitute.For<IHostApplicationLifetime>();
        var logger = Substitute.For<ILogger<ExecuteCommandService>>();
        // Execute
        var target = new ExecuteCommandService(
            commandSet,
            blobsStorageService,
            hostApplicationLifetime,
            logger
        );
        await target.StartAsync(default);
        await target.StopAsync(default);
        // Validate
        _ = commandContext
            .Received(3)
            .ExecuteCommandAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Test()]
    public async Task ExecuteAsync_WhenInvalidData_ShouldSkip()
    {
        // Setup
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<ConversationProperties>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        var blobsStorageService = Substitute.For<IBlobsStorageService>();
        _ = blobsStorageService
            .GetNamesAsync(Arg.Any<CancellationToken>())
            .Returns(
                new List<string>()
                {
                    "BotState"
                }.ToAsyncEnumerable()
            );
        _ = blobsStorageService
            .GetObjectAsync<Dictionary<string, object?>>("BotState", Arg.Any<CancellationToken>())
            .Returns(
                new BlobContent<Dictionary<string, object?>>()
                {
                    Data = null,
                    ETag = new ETag()
                }
            );
        var hostApplicationLifetime = Substitute.For<IHostApplicationLifetime>();
        var logger = Substitute.For<ILogger<ExecuteCommandService>>();
        // Execute
        var target = new ExecuteCommandService(
            commandSet,
            blobsStorageService,
            hostApplicationLifetime,
            logger
        );
        await target.StartAsync(default);
        await target.StopAsync(default);
        // Validate
        _ = commandContext
            .DidNotReceive()
            .ExecuteCommandAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Test()]
    public async Task ExecuteAsync_WhenInvalidConversationProperties_ShouldSkip()
    {
        // Setup
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<ConversationProperties>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        var blobsStorageService = Substitute.For<IBlobsStorageService>();
        _ = blobsStorageService
            .GetNamesAsync(Arg.Any<CancellationToken>())
            .Returns(
                new List<string>()
                {
                    "BotState"
                }.ToAsyncEnumerable()
            );
        _ = blobsStorageService
            .GetObjectAsync<Dictionary<string, object?>>("BotState", Arg.Any<CancellationToken>())
            .Returns(
                new BlobContent<Dictionary<string, object?>>()
                {
                    Data = new Dictionary<string, object?>()
                    {
                        ["ConversationProperties"] = null,
                        ["ConversationReference"] = new ConversationReference()
                    },
                    ETag = new ETag()
                }
            );
        var hostApplicationLifetime = Substitute.For<IHostApplicationLifetime>();
        var logger = Substitute.For<ILogger<ExecuteCommandService>>();
        // Execute
        var target = new ExecuteCommandService(
            commandSet,
            blobsStorageService,
            hostApplicationLifetime,
            logger
        );
        await target.StartAsync(default);
        await target.StopAsync(default);
        // Validate
        _ = commandContext
            .DidNotReceive()
            .ExecuteCommandAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [Test()]
    public async Task ExecuteAsync_WhenInvalidConversationReference_ShouldSkip()
    {
        // Setup
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<ConversationProperties>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        var blobsStorageService = Substitute.For<IBlobsStorageService>();
        _ = blobsStorageService
            .GetNamesAsync(Arg.Any<CancellationToken>())
            .Returns(
                new List<string>()
                {
                    "BotState"
                }.ToAsyncEnumerable()
            );
        _ = blobsStorageService
            .GetObjectAsync<Dictionary<string, object?>>("BotState", Arg.Any<CancellationToken>())
            .Returns(
                new BlobContent<Dictionary<string, object?>>()
                {
                    Data = new Dictionary<string, object?>()
                    {
                        ["ConversationProperties"] = new ConversationProperties(),
                        ["ConversationReference"] = null
                    },
                    ETag = new ETag()
                }
            );
        var hostApplicationLifetime = Substitute.For<IHostApplicationLifetime>();
        var logger = Substitute.For<ILogger<ExecuteCommandService>>();
        // Execute
        var target = new ExecuteCommandService(
            commandSet,
            blobsStorageService,
            hostApplicationLifetime,
            logger
        );
        await target.StartAsync(default);
        await target.StopAsync(default);
        // Validate
        _ = commandContext
            .DidNotReceive()
            .ExecuteCommandAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

}
