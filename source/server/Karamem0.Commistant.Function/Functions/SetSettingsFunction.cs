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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Karamem0.Commistant.Functions;

public class SetSettingsFunction(
    IBlobsService blobsService,
    IBotConnectorService botConnectorService,
    IMapper mapper,
    ILogger<SetSettingsFunction> logger
)
{

    private readonly IBlobsService blobsService = blobsService;

    private readonly IBotConnectorService botConnectorService = botConnectorService;

    private readonly IMapper mapper = mapper;

    private readonly ILogger<SetSettingsFunction> logger = logger;

    [Function("SetSettings")]
    public async Task<IActionResult> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "POST")] HttpRequest requestData,
        [Microsoft.Azure.Functions.Worker.Http.FromBody()] SetSettingsRequest requestBody,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            this.logger.FunctionExecuting();
            var (authenticationStatus, authenticationResponse) = await requestData.HttpContext.AuthenticateAzureFunctionAsync();
            if (authenticationStatus is false)
            {
                this.logger.FunctionUnauthorized();
                return authenticationResponse ?? new StatusCodeResult(StatusCodes.Status401Unauthorized);
            }
            var responseBody = this.mapper.Map<SetSettingsResponse>(requestBody);
            var blobName = HttpUtility.UrlEncode($"{requestBody.ChannelId}/conversations/{requestBody.MeetingId}");
            var blobContent = await this.blobsService.GetObjectAsync<Dictionary<string, object?>>(blobName, cancellationToken);
            _ = blobContent.Data ?? throw new InvalidOperationException();
            var conversationReference = blobContent.Data.GetValueOrDefault<ConversationReference>(nameof(ConversationReference));
            _ = conversationReference?.ServiceUrl ?? throw new InvalidOperationException();
            var meetingInfo = await this.botConnectorService.GetMeetingInfoAsync(
                new Uri(conversationReference.ServiceUrl),
                requestBody.MeetingId,
                cancellationToken
            );
            var userId = requestData.HttpContext.User.GetObjectId();
            if (meetingInfo.Organizer?.AadObjectId != userId)
            {
                throw new InvalidOperationException();
            }
            var commandSettings = blobContent.Data.GetValueOrDefault<CommandSettings>(nameof(CommandSettings));
            if (commandSettings is null)
            {
                commandSettings = this.mapper.Map<CommandSettings>(requestBody);
            }
            else
            {
                commandSettings = this.mapper.Map(requestBody, commandSettings);
            }
            blobContent.Data[nameof(CommandSettings)] = commandSettings;
            await this.blobsService.SetObjectAsync(
                blobName,
                blobContent,
                cancellationToken
            );
            return new OkObjectResult(this.mapper.Map(commandSettings, responseBody));
        }
        catch (InvalidOperationException ex)
        {
            this.logger.FunctionFailed(exception: ex);
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }
        catch (Exception ex)
        {
            this.logger.UnhandledErrorOccurred(exception: ex);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        finally
        {
            this.logger.FunctionExecuted();
        }
    }

    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            _ = this.CreateMap<SetSettingsRequest, SetSettingsResponse>();
            _ = this.CreateMap<SetSettingsRequest, CommandSettings>();
            _ = this.CreateMap<CommandSettings, SetSettingsResponse>();
        }

    }

}
