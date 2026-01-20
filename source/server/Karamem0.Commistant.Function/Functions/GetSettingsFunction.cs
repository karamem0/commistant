//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Extensions;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Models;
using Karamem0.Commistant.Serialization;
using Karamem0.Commistant.Services;
using Mapster;
using MapsterMapper;
using Microsoft.Agents.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System.Threading;
using System.Web;

namespace Karamem0.Commistant.Functions;

public class GetSettingsFunction(
    IBlobsService blobsService,
    IConnectorClientService connectorClientService,
    IMapper mapper,
    ILogger<GetSettingsFunction> logger
)
{

    private readonly IBlobsService blobsService = blobsService;

    private readonly IConnectorClientService connectorClientService = connectorClientService;

    private readonly IMapper mapper = mapper;

    private readonly ILogger<GetSettingsFunction> logger = logger;

    [Function("GetSettings")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST")] HttpRequest requestData,
        [Microsoft.Azure.Functions.Worker.Http.FromBody()] GetSettingsRequest requestBody,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            this.logger.MethodExecuting();
            var (authenticationStatus, authenticationResponse) = await requestData.HttpContext.AuthenticateAzureFunctionAsync();
            if (authenticationStatus is false)
            {
                this.logger.AuthorizationFailed();
                return authenticationResponse ?? new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            var responseBody = this.mapper.Map<GetSettingsResponse>(requestBody);
            var blobName = HttpUtility.UrlEncode($"{requestBody.ChannelId}/conversations/{requestBody.MeetingId}");
            var blobContent = await this.blobsService.GetObjectAsync<Dictionary<string, object?>>(blobName, cancellationToken);
            _ = blobContent.Data ?? throw new InvalidOperationException("Data を null にはできません");
            var conversationReference = blobContent.Data.GetValueOrDefault<ConversationReference>(nameof(ConversationReference));
            _ = conversationReference?.ServiceUrl ?? throw new InvalidOperationException("ServiceUrl を null にはできません");
            var meetingInfo = await this.connectorClientService.GetMeetingInfoAsync(
                conversationReference.ServiceUrl,
                requestBody.MeetingId,
                cancellationToken
            );
            var userId = requestData.HttpContext.User.GetObjectId();
            if (meetingInfo.Organizer?.AadObjectId == userId)
            {
                responseBody.IsOrganizer = true;
            }
            var commandSettings = blobContent.Data.GetValueOrDefault<CommandSettings>(nameof(CommandSettings));
            return new OkObjectResult(this.mapper.Map(commandSettings, responseBody));
        }
        catch (InvalidOperationException ex)
        {
            this.logger.MethodFailed(exception: ex);
            return new ContentResult()
            {
                StatusCode = StatusCodes.Status400BadRequest,
                Content = JsonConverter.Serialize(
                    new ErrorResponse()
                    {
                        Error = ex.Message
                    }
                )
            };
        }
        catch (Exception ex)
        {
            this.logger.UnhandledErrorOccurred(exception: ex);
            return new ContentResult()
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Content = JsonConverter.Serialize(
                    new ErrorResponse()
                    {
                        Error = ex.Message
                    }
                )
            };
        }
        finally
        {
            this.logger.MethodExecuted();
        }
    }

    public class MapperConfiguration : IRegister
    {

        public void Register(TypeAdapterConfig config)
        {
            _ = config.NewConfig<GetSettingsRequest, GetSettingsResponse>();
            _ = config.NewConfig<CommandSettings, GetSettingsResponse>();
        }

    }

}
