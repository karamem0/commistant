//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Karamem0.Commistant.Commands;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        private readonly BlobContainerClient blobClient;

        private readonly CommandSet commandSet;

        public ExecuteCommandFunction(
            ILoggerFactory loggerFactory,
            BlobContainerClient blobClient,
            CommandSet commandSet)
        {
            this.logger = loggerFactory.CreateLogger<ExecuteCommandFunction>();
            this.blobClient = blobClient;
            this.commandSet = commandSet;
        }

        [Function("ExecuteCommand")]
        public async Task Run([TimerTrigger("0 */1 * * * *")] object timerInfo)
        {
            try
            {
                this.logger.TimerStarted();
                await foreach (var blobPage in this.blobClient.GetBlobsAsync().AsPages())
                {
                    foreach (var blobItem in blobPage.Values)
                    {
                        var blobClient = this.blobClient.GetBlobClient(blobItem.Name);
                        var inputStream = await blobClient.DownloadStreamingAsync();
                        var inputETag = inputStream.GetRawResponse().Headers.ETag;
                        var inputString = await new StreamReader(inputStream.Value.Content).ReadToEndAsync();
                        var jsonContent = JsonConvert.DeserializeObject<JObject>(inputString);
                        if (jsonContent == null)
                        {
                            continue;
                        }
                        var property = jsonContent.Value<JToken>("ConversationProperty")?.ToObject<ConversationProperty>();
                        if (property == null)
                        {
                            continue;
                        }
                        var reference = jsonContent.Value<JToken>("ConversationReference")?.ToObject<ConversationReference>();
                        if (reference == null)
                        {
                            continue;
                        }
                        var cd = await this.commandSet.CreateContextAsync(property, reference);
                        await cd.ExecuteCommandAsync(nameof(StartMeetingCommand));
                        await cd.ExecuteCommandAsync(nameof(EndMeetingCommand));
                        await cd.ExecuteCommandAsync(nameof(InMeetingCommand));
                        jsonContent["ConversationProperty"] = JToken.FromObject(property);
                        var outoutString = JsonConvert.SerializeObject(jsonContent);
                        var outputStream = new MemoryStream(Encoding.UTF8.GetBytes(outoutString));
                        _ = await blobClient.UploadAsync(outputStream, new BlobUploadOptions()
                        {
                            Conditions = new BlobRequestConditions()
                            {
                                IfMatch = inputETag
                            }
                        });
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
