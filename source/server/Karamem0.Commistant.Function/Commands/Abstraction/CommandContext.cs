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

public interface ICommandContext
{

    Task ExecuteCommandAsync(string commandId, CancellationToken cancellationToken = default);

}

public class CommandContext(
    ICommandSet commandSet,
    CommandSettings property,
    ConversationReference reference
) : ICommandContext
{

    private readonly ICommandSet commandSet = commandSet;

    private readonly CommandSettings property = property;

    private readonly ConversationReference reference = reference;

    public async Task ExecuteCommandAsync(string commandId, CancellationToken cancellationToken = default)
    {
        var command = this.commandSet.Find(commandId);
        if (command is not null)
        {
            await command.ExecuteAsync(
                this.property,
                this.reference,
                cancellationToken
            );
        }
    }

}
