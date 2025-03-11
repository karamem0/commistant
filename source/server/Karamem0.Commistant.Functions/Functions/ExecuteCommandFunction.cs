//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.Storage.Blobs;
using Karamem0.Commistant.Commands;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Extensions;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Functions;

public class ExecuteCommandFunction(
    ILoggerFactory loggerFactory,
    BlobContainerClient botStateClient,
    ICommandSet commandSet
)
{

    private readonly ILogger logger = loggerFactory.CreateLogger<ExecuteCommandFunction>();

    private readonly BlobContainerClient botStateClient = botStateClient;

    private readonly ICommandSet commandSet = commandSet;

#pragma warning disable IDE0060

    [Function("ExecuteCommand")]
    public async Task RunAsync([TimerTrigger("0 */1 * * * *")] object timerInfo)
    {
        try
        {
            this.logger.TimerStarted();
            await foreach (var blobItem in this.botStateClient.GetBlobsAsync())
            {
                var blobClient = this.botStateClient.GetBlobClient(blobItem.Name);
                var blobContent = await blobClient.GetObjectAsync<Dictionary<string, object?>>();
                if (blobContent.Data is null)
                {
                    continue;
                }
                var property = blobContent.Data.GetValueOrDefault<ConversationProperty>(nameof(ConversationProperty));
                if (property is null)
                {
                    continue;
                }
                var reference = blobContent.Data.GetValueOrDefault<ConversationReference>(nameof(ConversationReference));
                if (reference is null)
                {
                    continue;
                }
                var cd = this.commandSet.CreateContext(property, reference);
                await cd.ExecuteCommandAsync(nameof(StartMeetingCommand));
                await cd.ExecuteCommandAsync(nameof(EndMeetingCommand));
                await cd.ExecuteCommandAsync(nameof(InMeetingCommand));
                await blobClient.SetObjectAsync(blobContent);
            }
            this.logger.TimerEnded();
        }
        catch (Exception ex)
        {
            this.logger.UnhandledError(ex);
        }
    }

#pragma warning restore IDE0060

}
