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
using Microsoft.Azure.Functions.Worker;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Functions;

public class ExecuteCommandFunction(
    IBlobService blobService,
    ICommandSet commandSet,
    ILogger<ExecuteCommandFunction> logger
)
{

    private readonly IBlobService blobService = blobService;

    private readonly ICommandSet commandSet = commandSet;

    private readonly ILogger<ExecuteCommandFunction> logger = logger;

#pragma warning disable IDE0060
    [Function("ExecuteCommand")]
    public async Task RunAsync([TimerTrigger("0 */1 * * * *")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        try
        {
            this.logger.FunctionExecuting();
            await foreach (var blobName in this.blobService.GetBlobNamesAsync(cancellationToken))
            {
                var blobContent = await this.blobService.GetObjectAsync<Dictionary<string, object?>>(blobName, cancellationToken);
                if (blobContent.Data is null)
                {
                    continue;
                }
                var property = blobContent.Data.GetValueOrDefault<CommandSettings>(nameof(CommandSettings));
                if (property is null)
                {
                    continue;
                }
                var reference = blobContent.Data.GetValueOrDefault<ConversationReference>(nameof(ConversationReference));
                if (reference is null)
                {
                    continue;
                }
                var commandContext = this.commandSet.CreateContext(property, reference);
                await Task.WhenAll(
                    commandContext.ExecuteCommandAsync(nameof(StartMeetingCommand), cancellationToken),
                    commandContext.ExecuteCommandAsync(nameof(EndMeetingCommand), cancellationToken),
                    commandContext.ExecuteCommandAsync(nameof(InMeetingCommand), cancellationToken)
                );
                await this.blobService.SetObjectAsync(
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
            this.logger.FunctionExecuted();
        }
    }
#pragma warning restore IDE0060

}
