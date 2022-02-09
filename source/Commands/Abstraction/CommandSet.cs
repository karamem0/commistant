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

    public class CommandSet
    {

        private readonly IDictionary<string, Command> commands;

        public CommandSet()
        {
            this.commands = new Dictionary<string, Command>();
        }

        public Task<CommandContext> CreateContextAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            return Task.Run(() => new CommandContext(this, turnContext), cancellationToken);
        }

        public CommandSet Add(Command? command)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));
            this.commands.Add(command.GetType().Name, command);
            return this;
        }

        public Command? Find(string? commandId)
        {
            _ = commandId ?? throw new ArgumentNullException(nameof(commandId));
            if (this.commands.TryGetValue(commandId, out var command))
            {
                return command;
            }
            return null;
        }

    }

}
