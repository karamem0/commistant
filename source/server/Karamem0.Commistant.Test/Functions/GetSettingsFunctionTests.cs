//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AutoMapper;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Functions.Tests;

[Category("Karamem0.Commistant.Functions")]
public class GetSettingsFunctionTests
{

    [Test()]
    public async Task RunAsync_Success()
    {
        // Setup
        var blobsService = Substitute.For<IBlobsService>();
        _ = blobsService
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
            .Map<GetSettingsResponse>(Arg.Any<GetSettingsRequest>())
            .Returns(
                new GetSettingsResponse()
                {
                    ChannelId = "19:1234567890",
                    MeetingId = "1234567890"
                }
            );
        _ = mapper
            .Map(Arg.Any<CommandSettings>(), Arg.Any<GetSettingsResponse>())
            .Returns(
                new GetSettingsResponse()
                {
                    ChannelId = "19:1234567890",
                    MeetingId = "1234567890"
                }
            );
        var logger = Substitute.For<ILogger<GetSettingsFunction>>();
        // Execute
        var authenticationService = Substitute.For<IAuthenticationService>();
        _ = authenticationService
            .AuthenticateAsync(Arg.Any<HttpContext>(), Arg.Any<string>())
            .Returns(
                AuthenticateResult.Success(
                    new AuthenticationTicket(
                        new ClaimsPrincipal(
                            new ClaimsIdentity(
                                [
                                    new Claim("oid", "48d31887-5fad-4d73-a9f5-3c356e68a038"),
                                ],
                                "Bearer"
                            )
                        ),
                        null,
                        "Bearer"
                    )
                )
            );
        var serviceCollection = new ServiceCollection();
        _ = serviceCollection.AddSingleton(authenticationService);
        var httpContext = Substitute.For<HttpContext>();
        _ = httpContext.RequestServices.Returns(serviceCollection.BuildServiceProvider());
        var requestData = Substitute.For<HttpRequest>();
        _ = requestData.HttpContext.Returns(httpContext);
        var requestBody = new GetSettingsRequest()
        {
            ChannelId = "19:1234567890",
            MeetingId = "1234567890"
        };
        var target = new GetSettingsFunction(
            blobsService,
            botConnectorService,
            mapper,
            logger
        );
        var actual = await target.RunAsync(
            requestData,
            requestBody,
            default
        ) as OkObjectResult;
        // Assert
        Assert.That(actual?.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
    }

}
