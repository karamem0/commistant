//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
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
    public async Task RunAsync_Success()
    {
        // Setup
        var blobService = Substitute.For<IBlobService>();
        _ = blobService
            .GetBlobNamesAsync(Arg.Any<CancellationToken>())
            .Returns(
                new List<string>()
                {
                    "BotState"
                }.ToAsyncEnumerable()
            );
        _ = blobService
            .GetObjectAsync<Dictionary<string, object?>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(
                new BlobContent<Dictionary<string, object?>>()
                {
                    Data = new Dictionary<string, object?>()
                    {
                        ["CommandSettings"] = new CommandSettings(),
                        ["ConversationReference"] = new ConversationReference()
                    }
                }
            );
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<CommandSettings>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        var logger = Substitute.For<ILogger<ExecuteCommandFunction>>();
        // Execute
        var target = new ExecuteCommandFunction(
            blobService,
            commandSet,
            logger
        );
        await target.RunAsync(new TimerInfo(), default);
        // Validate
        _ = commandContext
            .Received(3)
            .ExecuteCommandAsync(Arg.Any<string>());
    }

    [Test()]
    public async Task RunAsync_Failure_WhenDataIsNull()
    {
        // Setup
        var blobService = Substitute.For<IBlobService>();
        _ = blobService
            .GetBlobNamesAsync(Arg.Any<CancellationToken>())
            .Returns(
                new List<string>()
                {
                    "BotState"
                }.ToAsyncEnumerable()
            );
        _ = blobService
            .GetObjectAsync<Dictionary<string, object?>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(
                new BlobContent<Dictionary<string, object?>>()
                {
                    Data = null
                }
            );
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<CommandSettings>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        var logger = Substitute.For<ILogger<ExecuteCommandFunction>>();
        // Execute
        var target = new ExecuteCommandFunction(
            blobService,
            commandSet,
            logger
        );
        await target.RunAsync(new TimerInfo(), default);
        // Validate
        _ = commandContext
            .DidNotReceive()
            .ExecuteCommandAsync(Arg.Any<string>());
    }

    [Test()]
    public async Task RunAsync_Failure_WhenCommandSettingsIsNull()
    {
        // Setup
        var blobService = Substitute.For<IBlobService>();
        _ = blobService
            .GetBlobNamesAsync(Arg.Any<CancellationToken>())
            .Returns(
                new List<string>()
                {
                    "BotState"
                }.ToAsyncEnumerable()
            );
        _ = blobService
            .GetObjectAsync<Dictionary<string, object?>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(
                new BlobContent<Dictionary<string, object?>>()
                {
                    Data = new Dictionary<string, object?>()
                    {
                        ["CommandSettings"] = null,
                        ["ConversationReference"] = new ConversationReference()
                    }
                }
            );
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<CommandSettings>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        var logger = Substitute.For<ILogger<ExecuteCommandFunction>>();
        // Execute
        var target = new ExecuteCommandFunction(
            blobService,
            commandSet,
            logger
        );
        await target.RunAsync(new TimerInfo(), default);
        // Validate
        _ = commandContext
            .DidNotReceive()
            .ExecuteCommandAsync(Arg.Any<string>());
    }

    [Test()]
    public async Task RunAsync_Failure_WhenConversationReferenceIsNull()
    {
        // Setup
        var blobService = Substitute.For<IBlobService>();
        _ = blobService
            .GetBlobNamesAsync(Arg.Any<CancellationToken>())
            .Returns(
                new List<string>()
                {
                    "BotState"
                }.ToAsyncEnumerable()
            );
        _ = blobService
            .GetObjectAsync<Dictionary<string, object?>>(Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(
                new BlobContent<Dictionary<string, object?>>()
                {
                    Data = new Dictionary<string, object?>()
                    {
                        ["CommandSettings"] = new CommandSettings(),
                        ["ConversationReference"] = null
                    }
                }
            );
        var commandContext = Substitute.For<ICommandContext>();
        var commandSet = Substitute.For<ICommandSet>();
        _ = commandSet
            .CreateContext(Arg.Any<CommandSettings>(), Arg.Any<ConversationReference>())
            .Returns(commandContext);
        var logger = Substitute.For<ILogger<ExecuteCommandFunction>>();
        // Execute
        var target = new ExecuteCommandFunction(
            blobService,
            commandSet,
            logger
        );
        await target.RunAsync(new TimerInfo(), default);
        // Validate
        _ = commandContext
            .DidNotReceive()
            .ExecuteCommandAsync(Arg.Any<string>());
    }

}
