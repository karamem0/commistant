//
// Copyright (c) 2022-2026 karamem0
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
using Microsoft.Agents.Core.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Karamem0.Commistant.Functions;

public class ExecuteCommandFunction(
    IBlobsService blobsService,
    ICommandSet commandSet,
    ILogger<ExecuteCommandFunction> logger
)
{

    private readonly IBlobsService blobsService = blobsService;

    private readonly ICommandSet commandSet = commandSet;

    private readonly ILogger<ExecuteCommandFunction> logger = logger;

#pragma warning disable IDE0060
    [Function("ExecuteCommand")]
    public async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo, CancellationToken cancellationToken = default)
    {
        try
        {
            this.logger.MethodExecuting();
            await foreach (var blobName in this.blobsService.GetBlobNamesAsync(cancellationToken))
            {
                var blobContent = await this.blobsService.GetObjectAsync<Dictionary<string, object?>>(blobName, cancellationToken);
                if (blobContent.Data is null)
                {
                    continue;
                }
                var commandSettings = blobContent.Data.GetValueOrDefault<CommandSettings>(nameof(CommandSettings));
                if (commandSettings is null)
                {
                    continue;
                }
                var conversationReference = blobContent.Data.GetValueOrDefault<ConversationReference>(nameof(ConversationReference));
                if (conversationReference is null)
                {
                    continue;
                }
                var commandContext = this.commandSet.CreateContext(commandSettings, conversationReference);
                await Task.WhenAll(
                    commandContext.ExecuteCommandAsync(nameof(MeetingStartedCommand), cancellationToken),
                    commandContext.ExecuteCommandAsync(nameof(MeetingEndingCommand), cancellationToken),
                    commandContext.ExecuteCommandAsync(nameof(MeetingInProgressCommand), cancellationToken)
                );
                blobContent.Data[nameof(CommandSettings)] = commandSettings;
                await this.blobsService.SetObjectAsync(
                    blobName,
                    blobContent,
                    cancellationToken
                );
            }
        }
        catch (Exception ex)
        {
            this.logger.UnhandledErrorOccurred(exception: ex);
        }
        finally
        {
            this.logger.MethodExecuted();
        }
    }
#pragma warning restore IDE0060

}
