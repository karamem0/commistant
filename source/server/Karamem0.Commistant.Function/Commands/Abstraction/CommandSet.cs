//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Microsoft.Bot.Schema;

namespace Karamem0.Commistant.Commands.Abstraction;

public interface ICommandSet
{

    ICommandContext CreateContext(CommandSettings property, ConversationReference reference);

    ICommandSet Add(ICommand? command);

    ICommand? Find(string? commandId);

}

public class CommandSet : ICommandSet
{

    private readonly Dictionary<string, ICommand> commands;

    public CommandSet()
    {
        this.commands = [];
    }

    public ICommandContext CreateContext(CommandSettings property, ConversationReference reference)
    {
        return new CommandContext(
            this,
            property,
            reference
        );
    }

    public ICommandSet Add(ICommand? command)
    {
        _ = command ?? throw new ArgumentNullException(nameof(command));
        var commandType = command.GetType();
        this.commands.Add(commandType.Name, command);
        return this;
    }

    public ICommand? Find(string? commandId)
    {
        _ = commandId ?? throw new ArgumentNullException(nameof(commandId));
        if (this.commands.TryGetValue(commandId, out var command))
        {
            return command;
        }
        return null;
    }

}
