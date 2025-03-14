//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Commands;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Extensions;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Hosting;

public class ExecuteCommandService(
    ICommandSet commandSet,
    IBlobsStorageService blobsStorageService,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<ExecuteCommandService> logger
) : BackgroundService
{

    private readonly ICommandSet commandSet = commandSet;

    private readonly IBlobsStorageService blobsStorageService = blobsStorageService;

    private readonly IHostApplicationLifetime hostApplicationLifetime = hostApplicationLifetime;

    private readonly ILogger<ExecuteCommandService> logger = logger;

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        this.logger.TimerExecuting();
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        this.logger.TimerExecuted();
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            this.logger.TimerStarted();
            await foreach (var blobName in this.blobsStorageService.GetNamesAsync(cancellationToken))
            {
                var blobContent = await this.blobsStorageService.GetObjectAsync<Dictionary<string, object?>>(blobName, cancellationToken);
                if (blobContent.Data is null)
                {
                    continue;
                }
                var conversationProperties = blobContent.Data.GetValueOrDefault<ConversationProperties>(nameof(ConversationProperties));
                if (conversationProperties is null)
                {
                    continue;
                }
                var conversationReference = blobContent.Data.GetValueOrDefault<ConversationReference>(nameof(ConversationReference));
                if (conversationReference is null)
                {
                    continue;
                }
                var commandContext = this.commandSet.CreateContext(conversationProperties, conversationReference);
                await commandContext.ExecuteCommandAsync(nameof(StartMeetingCommand), cancellationToken);
                await commandContext.ExecuteCommandAsync(nameof(EndMeetingCommand), cancellationToken);
                await commandContext.ExecuteCommandAsync(nameof(InMeetingCommand), cancellationToken);
                await this.blobsStorageService.SetObjectAsync(
                    blobName,
                    blobContent,
                    cancellationToken
                );
            }
            this.logger.TimerEnded();
        }
        catch (Exception ex)
        {
            this.logger.UnhandledErrorOccurred(exception: ex);
        }
        finally
        {
            this.hostApplicationLifetime.StopApplication();
        }
    }

}
