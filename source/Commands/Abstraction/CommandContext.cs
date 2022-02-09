//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Bot.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Commands.Abstraction
{

    public class CommandContext
    {

        private readonly CommandSet commandSet;

        private readonly ITurnContext turnContext;

        public CommandContext(CommandSet commandSet, ITurnContext turnContext)
        {
            this.commandSet = commandSet;
            this.turnContext = turnContext;
        }

        public async Task ExecuteCommandAsync(string commandId, CancellationToken cancellationToken)
        {
            var command = this.commandSet.Find(commandId);
            if (command != null)
            {
                await command.ExecuteAsync(this.turnContext, cancellationToken);
            }
        }

    }

}
