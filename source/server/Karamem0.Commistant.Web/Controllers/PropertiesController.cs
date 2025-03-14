//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AutoMapper;
using Karamem0.Commistant.Extensions;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Controllers;

[ApiController()]
[Authorize(AuthenticationSchemes = "ApiAuthentication")]
[Route("api/properties")]
public class PropertiesController(
    IBlobsStorageService blobsStorageService,
    IBotConnectorService botConnectorService,
    IMapper mapper,
    ILogger<PropertiesController> logger
) : Controller
{

    private readonly IBlobsStorageService blobsStorageService = blobsStorageService;

    private readonly IBotConnectorService botConnectorService = botConnectorService;

    private readonly IMapper mapper = mapper;

    private readonly ILogger logger = logger;

    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetConversationPropertiesResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync([FromQuery] GetConversationPropertiesRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            this.logger.GetPropertyExecuting(channelId: request.ChannelId, meetingId: request.MeetingId);
            if (this.TryValidateModel(request) is false)
            {
                return this.BadRequest(this.ModelState);
            }
            var response = this.mapper.Map<GetConversationPropertiesResponse>(request);
            var blobContent = await this.blobsStorageService.GetObjectAsync<Dictionary<string, object?>>(
                request.ChannelId!,
                request.MeetingId!,
                cancellationToken
            );
            var conversationReference = blobContent?.Data?.GetValueOrDefault<ConversationReference>(nameof(ConversationReference));
            if (conversationReference is null)
            {
                return this.BadRequest("ConversationReference が見つかりません。");
            }
            var meetingInfo = await this.botConnectorService.GetMeetingInfoAsync(
                new Uri(conversationReference.ServiceUrl),
                request.MeetingId!,
                cancellationToken
            );
            var userId = this.User.Claims.SingleOrDefault(_ => _.Type == ClaimConstants.ObjectId);
            if (meetingInfo.Organizer.AadObjectId == userId?.Value)
            {
                response.IsOrganizer = true;
            }
            var conversationProperties = blobContent?.Data?.GetValueOrDefault<ConversationProperties>(nameof(ConversationProperties));
            this.logger.GetPropertyExecuted(channelId: request.ChannelId, meetingId: request.MeetingId);
            return this.Ok(conversationProperties is null ? response : this.mapper.Map(conversationProperties, response));
        }
        catch (Exception ex)
        {
            this.logger.UnhandledErrorOccurred(exception: ex);
            return this.Problem(ex.Message);
        }
    }

    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SetConversationPropertiesResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync([FromBody] SetConversationPropertiesRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            this.logger.SetPropertyExecuting(channelId: request.ChannelId, meetingId: request.MeetingId);
            if (this.TryValidateModel(request) is false)
            {
                return this.BadRequest(this.ModelState);
            }
            var response = this.mapper.Map<SetConversationPropertiesResponse>(request);
            var blobContent = await this.blobsStorageService.GetObjectAsync<Dictionary<string, object?>>(
                request.ChannelId!,
                request.MeetingId!,
                cancellationToken
            );
            _ = blobContent.Data ?? throw new InvalidOperationException();
            var conversationReference = blobContent.Data.GetValueOrDefault<ConversationReference>(nameof(ConversationReference));
            if (conversationReference is null)
            {
                return this.BadRequest("ConversationReference が見つかりません。");
            }
            var meetingInfo = await this.botConnectorService.GetMeetingInfoAsync(
                new Uri(conversationReference.ServiceUrl),
                request.MeetingId!,
                cancellationToken
            );
            var userId = this.User.Claims.SingleOrDefault(_ => _.Type == ClaimConstants.ObjectId);
            if (meetingInfo.Organizer.AadObjectId != userId?.Value)
            {
                return this.Unauthorized("開催者のみが設定を変更できます。");
            }
            var conversationProperties = blobContent.Data.GetValueOrDefault<ConversationProperties>(nameof(ConversationProperties));
            if (conversationProperties is null)
            {
                conversationProperties = this.mapper.Map<ConversationProperties>(request);
            }
            else
            {
                conversationProperties = this.mapper.Map(request, conversationProperties);
            }
            blobContent.Data[nameof(ConversationProperties)] = conversationProperties;
            await this.blobsStorageService.SetObjectAsync(
                request.ChannelId!,
                request.MeetingId!,
                blobContent,
                cancellationToken
            );
            this.logger.SetPropertyExecuted(channelId: request.ChannelId, meetingId: request.MeetingId);
            return this.Ok(this.mapper.Map(conversationProperties, response));
        }
        catch (Exception ex)
        {
            this.logger.UnhandledErrorOccurred(exception: ex);
            return this.Problem(ex.Message);
        }
    }

}
