//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Microsoft.Agents.Core.Models;
using System.Threading;

namespace Karamem0.Commistant.Commands.Abstraction;

public interface ICommand
{

    Task ExecuteAsync(
        CommandSettings property,
        ConversationReference reference,
        CancellationToken cancellationToken = default
    );

}

public abstract class Command : ICommand
{

    public abstract Task ExecuteAsync(
        CommandSettings property,
        ConversationReference reference,
        CancellationToken cancellationToken = default
    );

}
