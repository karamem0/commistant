//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AutoMapper;
using Azure.Storage.Blobs;
using Karamem0.Commistant.Extensions;
using Karamem0.Commistant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Karamem0.Commistant.Controllers
{

    [ApiController()]
    [Authorize()]
    [Route("api/setproperty")]
    public class SetPropertyController : Controller
    {

        private readonly ILogger logger;

        private readonly IMapper mapper;

        private readonly BlobContainerClient botStateClient;

        public SetPropertyController(
            ILoggerFactory loggerFactory,
            IMapper mapper,
            BlobContainerClient botStateClient)
        {
            this.logger = loggerFactory.CreateLogger<SetPropertyController>();
            this.mapper = mapper;
            this.botStateClient = botStateClient;
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ConversationSetPropertyResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync([FromBody] ConversationSetPropertyRequest? request)
        {
            if (request is null)
            {
                return this.BadRequest();
            }
            var response = this.mapper.Map<ConversationSetPropertyResponse>(request);
            var blobName = HttpUtility.UrlEncode($"{request.ChannelId}/conversations/{request.MeetingId}");
            var blobClient = this.botStateClient.GetBlobClient(blobName);
            var blobContent = await blobClient.GetObjectAsync<Dictionary<string, object>>();
            if (blobContent.Data is null)
            {
                blobContent.Data = new Dictionary<string, object>();
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

    }

}
