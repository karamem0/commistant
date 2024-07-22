//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AutoMapper;
using Azure.Storage.Blobs;
using Karamem0.Commistant.Extensions;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Karamem0.Commistant.Controllers;

[ApiController()]
[Authorize()]
[Route("api/property")]
public class PropertyController(
    ILoggerFactory loggerFactory,
    IMapper mapper,
    BlobContainerClient botStateClient,
    ServiceClientCredentials botCredentials
) : Controller
{

    private readonly ILogger logger = loggerFactory.CreateLogger<PropertyController>();

    private readonly IMapper mapper = mapper;

    private readonly BlobContainerClient botStateClient = botStateClient;

    private readonly ServiceClientCredentials botCredentials = botCredentials;

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetConversationPropertyResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync([FromQuery] GetConversationPropertyRequest? request)
    {
        try
        {
            if (request is null)
            {
                return this.BadRequest();
            }
            var response = this.mapper.Map<GetConversationPropertyResponse>(request);
            var blobName = HttpUtility.UrlEncode($"{request.ChannelId}/conversations/{request.MeetingId}");
            var blobClient = this.botStateClient.GetBlobClient(blobName);
            var blobContent = await blobClient.GetObjectAsync<Dictionary<string, object>>();
            if (blobContent.Data is null)
            {
                return this.BadRequest();
            }
            var reference = blobContent.Data.GetValueOrDefault<ConversationReference>(nameof(ConversationReference));
            if (reference is null)
            {
                return this.BadRequest();
            }
            var botClient = new TeamsConnectorClient(new Uri(reference.ServiceUrl), this.botCredentials);
            var meetingId = Convert.ToBase64String(Encoding.UTF8.GetBytes($"0#{request.MeetingId}#0"));
            var meetingInfo = await botClient.Teams.FetchMeetingInfoAsync(meetingId);
            var userId = this.User.Claims.SingleOrDefault(_ => _.Type == ClaimConstants.ObjectId);
            if (meetingInfo.Organizer.AadObjectId == userId?.Value)
            {
                response.IsOrganizer = true;
            }
            var property = blobContent.Data.GetValueOrDefault<ConversationProperty>(nameof(ConversationProperty));
            if (property is null)
            {
                return this.Ok(response);
            }
            return this.Ok(this.mapper.Map(property, response));
        }
        catch (Exception ex)
        {
            this.logger.UnhandledError(ex);
            throw;
        }
    }

    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SetConversationPropertyResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync([FromBody] SetConversationPropertyRequest? request)
    {
        try
        {
            if (request is null)
            {
                return this.BadRequest();
            }
            var response = this.mapper.Map<SetConversationPropertyResponse>(request);
            var blobName = HttpUtility.UrlEncode($"{request.ChannelId}/conversations/{request.MeetingId}");
            var blobClient = this.botStateClient.GetBlobClient(blobName);
            var blobContent = await blobClient.GetObjectAsync<Dictionary<string, object>>();
            if (blobContent.Data is null)
            {
                return this.BadRequest();
            }
            var reference = blobContent.Data.GetValueOrDefault<ConversationReference>(nameof(ConversationReference));
            if (reference is null)
            {
                return this.BadRequest();
            }
            var botClient = new TeamsConnectorClient(new Uri(reference.ServiceUrl), this.botCredentials);
            var meetingId = Convert.ToBase64String(Encoding.UTF8.GetBytes($"0#{request.MeetingId}#0"));
            var meetingInfo = await botClient.Teams.FetchMeetingInfoAsync(meetingId);
            var userId = this.User.Claims.SingleOrDefault(_ => _.Type == ClaimConstants.ObjectId);
            if (meetingInfo.Organizer.AadObjectId != userId?.Value)
            {
                return this.Unauthorized();
            }
            var property = blobContent.Data.GetValueOrDefault<ConversationProperty>(nameof(ConversationProperty));
            if (property is null)
            {
                property = this.mapper.Map<ConversationProperty>(request);
            }
            else
            {
                property = this.mapper.Map(request, property);
            }
            blobContent.Data[nameof(ConversationProperty)] = property;
            await blobClient.SetObjectAsync(blobContent);
            return this.Ok(this.mapper.Map(property, response));
        }
        catch (Exception ex)
        {
            this.logger.UnhandledError(ex);
            throw;
        }
    }

}
