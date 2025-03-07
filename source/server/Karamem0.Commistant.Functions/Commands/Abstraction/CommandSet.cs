//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Commands.Abstraction;

public class CommandSet
{

    private readonly Dictionary<string, Command> commands;

    public CommandSet()
    {
        this.commands = [];
    }

    public Task<CommandContext> CreateContextAsync(ConversationProperty property, ConversationReference reference)
    {
        return Task.Run(() => new CommandContext(this, property, reference));
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
