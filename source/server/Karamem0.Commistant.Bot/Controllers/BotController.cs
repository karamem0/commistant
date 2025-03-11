//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Controllers;

[Route("api/messages")]
[ApiController()]
public class BotController(IBotFrameworkHttpAdapter adapter, IBot bot) : ControllerBase
{

    private readonly IBotFrameworkHttpAdapter adapter = adapter;

    private readonly IBot bot = bot;

    [HttpPost()]
    public async Task PostAsync()
    {
        await this.adapter.ProcessAsync(
            this.Request,
            this.Response,
            this.bot
        );
    }

}
