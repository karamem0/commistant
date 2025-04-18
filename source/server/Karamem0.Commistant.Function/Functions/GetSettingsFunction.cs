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
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Karamem0.Commistant.Functions;

public class GetSettingsFunction(
    IBlobService blobService,
    IBotConnectorService botConnectorService,
    IMapper mapper,
    ILogger<GetSettingsFunction> logger
)
{

    private readonly IBlobService blobService = blobService;

    private readonly IBotConnectorService botConnectorService = botConnectorService;

    private readonly IMapper mapper = mapper;

    private readonly ILogger<GetSettingsFunction> logger = logger;

    [Function("GetSettings")]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST")] HttpRequestData requestData,
        [FromBody()] GetSettingsRequest requestBody,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            this.logger.FunctionExecuting();
            _ = requestBody.ChannelId ?? throw new InvalidOperationException();
            _ = requestBody.MeetingId ?? throw new InvalidOperationException();
            var responseBody = this.mapper.Map<GetSettingsResponse>(requestBody);
            var blobName = HttpUtility.UrlEncode($"{requestBody.ChannelId}/conversations/{requestBody.MeetingId}");
            var blobContent = await this.blobService.GetObjectAsync<Dictionary<string, object?>>(blobName, cancellationToken);
            _ = blobContent.Data ?? throw new InvalidOperationException();
            var conversationReference = blobContent.Data.GetValueOrDefault<ConversationReference>(nameof(ConversationReference));
            _ = conversationReference?.ServiceUrl ?? throw new InvalidOperationException();
            var meetingInfo = await this.botConnectorService.GetMeetingInfoAsync(
                new Uri(conversationReference.ServiceUrl),
                requestBody.MeetingId,
                cancellationToken
            );
            var userId = requestData.GetUserId();
            if (meetingInfo.Organizer?.AadObjectId == userId)
            {
                responseBody.IsOrganizer = true;
            }
            var commandSettings = blobContent.Data.GetValueOrDefault<CommandSettings>(nameof(CommandSettings));
            var responseData = requestData.CreateResponse();
            responseData.StatusCode = HttpStatusCode.OK;
            await responseData.WriteAsJsonAsync(this.mapper.Map(commandSettings, responseBody), cancellationToken);
            return responseData;
        }
        catch (InvalidOperationException ex)
        {
            this.logger.FunctionFailed(exception: ex);
            var responseData = requestData.CreateResponse();
            responseData.StatusCode = HttpStatusCode.BadRequest;
            return responseData;
        }
        catch (Exception ex)
        {
            this.logger.UnhandledErrorOccurred(exception: ex);
            var responseData = requestData.CreateResponse();
            responseData.StatusCode = HttpStatusCode.InternalServerError;
            return responseData;
        }
        finally
        {
            this.logger.FunctionExecuted();
        }
    }

}
