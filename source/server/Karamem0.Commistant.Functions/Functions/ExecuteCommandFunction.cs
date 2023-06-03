//
// Copyright (c) 2023 karamem0
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Functions
{

    public class ExecuteCommandFunction
    {

        private readonly ILogger logger;

        private readonly BlobContainerClient botStateClient;

        private readonly CommandSet commandSet;

        public ExecuteCommandFunction(
            ILoggerFactory loggerFactory,
            BlobContainerClient botStateClient,
            CommandSet commandSet)
        {
            this.logger = loggerFactory.CreateLogger<ExecuteCommandFunction>();
            this.botStateClient = botStateClient;
            this.commandSet = commandSet;
        }

        [Function("ExecuteCommand")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] object timerInfo)
        {
            try
            {
                this.logger.TimerStarted();
                await foreach (var blobPage in this.botStateClient.GetBlobsAsync().AsPages())
                {
                    foreach (var blobItem in blobPage.Values)
                    {
                        var blobClient = this.botStateClient.GetBlobClient(blobItem.Name);
                        var blobContent = await blobClient.GetObjectAsync<Dictionary<string, object>>();
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
                        var cd = await this.commandSet.CreateContextAsync(property, reference);
                        await cd.ExecuteCommandAsync(nameof(StartMeetingCommand));
                        await cd.ExecuteCommandAsync(nameof(EndMeetingCommand));
                        await cd.ExecuteCommandAsync(nameof(InMeetingCommand));
                        await blobClient.SetObjectAsync(blobContent);
                    }
                }
                this.logger.TimerEnded();
            }
            catch (Exception ex)
            {
                this.logger.UnhandledError(ex);
            }
        }

    }

}
