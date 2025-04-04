//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AutoMapper;
using Azure.Core.Serialization;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Functions.Tests;

[Category("Karamem0.Commistant.Functions")]
public class SetSettingsFunctionTests
{

    [Test()]
    public async Task RunAsync_Success()
    {
        // Setup
        var blobService = Substitute.For<IBlobService>();
        _ = blobService
            .GetObjectAsync<Dictionary<string, object?>>(Arg.Any<string>())
            .Returns(
                new BlobContent<Dictionary<string, object?>>()
                {
                    Data = new Dictionary<string, object?>()
                    {
                        ["CommandSettings"] = new CommandSettings(),
                        ["ConversationReference"] = new ConversationReference()
                        {
                            ServiceUrl = "https://www.example.com",
                        }
                    }
                }
            );
        var botConnectorService = Substitute.For<IBotConnectorService>();
        _ = botConnectorService
            .GetMeetingInfoAsync(Arg.Any<Uri>(), Arg.Any<string>())
            .Returns(
                new MeetingInfo()
                {
                    Organizer = new TeamsChannelAccount()
                    {
                        AadObjectId = "48d31887-5fad-4d73-a9f5-3c356e68a038"
                    }
                }
            );
        var mapper = Substitute.For<IMapper>();
        _ = mapper
            .Map<SetSettingsResponse>(Arg.Any<SetSettingsRequest>())
            .Returns(new SetSettingsResponse());
        _ = mapper
            .Map(Arg.Any<CommandSettings>(), Arg.Any<SetSettingsResponse>())
            .Returns(new SetSettingsResponse());
        _ = mapper
            .Map(Arg.Any<SetSettingsRequest>(), Arg.Any<CommandSettings>())
            .Returns(new CommandSettings());
        var logger = Substitute.For<ILogger<SetSettingsFunction>>();
        // Execute
        var objectSerializer = Substitute.For<ObjectSerializer>();
        var workerOptions = Substitute.For<IOptions<WorkerOptions>>();
        _ = workerOptions.Value.Returns(
            new WorkerOptions()
            {
                Serializer = objectSerializer
            }
        );
        var serviceCollection = new ServiceCollection();
        _ = serviceCollection.AddSingleton(workerOptions);
        var functionContext = Substitute.For<FunctionContext>();
        _ = functionContext.InstanceServices.Returns(serviceCollection.BuildServiceProvider());
        var responseData = Substitute.For<HttpResponseData>(functionContext);
        _ = responseData.Headers.Returns([]);
        _ = responseData.Body.Returns(new MemoryStream());
        var requestData = Substitute.For<HttpRequestData>(functionContext);
        _ = requestData.Headers.Returns(
            new HttpHeadersCollection(
                [
                    new KeyValuePair<string, string>("X-MS-CLIENT-PRINCIPAL-ID", "48d31887-5fad-4d73-a9f5-3c356e68a038")
                ]
            )
        );
        _ = requestData
            .CreateResponse()
            .Returns(responseData);
        var requestBody = new SetSettingsRequest()
        {
            ChannelId = "19:1234567890",
            MeetingId = "1234567890"
        };
        var target = new SetSettingsFunction(
            blobService,
            botConnectorService,
            mapper,
            logger
        );
        var actual = await target.RunAsync(
            requestData,
            requestBody,
            default
        );
        // Assert
        Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

}
