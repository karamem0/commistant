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

public interface ICommand
{

    Task ExecuteAsync(
        ConversationProperty property,
        ConversationReference reference,
        CancellationToken cancellationToken = default
    );

}

public abstract class Command : ICommand
{

    protected Command()
    {
    }

    public abstract Task ExecuteAsync(
        ConversationProperty property,
        ConversationReference reference,
        CancellationToken cancellationToken = default
    );

}
