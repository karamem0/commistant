//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.Dialogs;
using Microsoft.Agents.Builder.State;
using System.Threading;

namespace Karamem0.Commistant.Services;

public interface IDialogService<T> where T : Dialog
{

    Task<DialogTurnResult> RunAsync(
        ITurnContext turnContext,
        AgentState agentState,
        CancellationToken cancellationToken
    );

}

public class DialogService<T>(T dialog) : IDialogService<T> where T : Dialog
{

    private readonly T dialog = dialog;

    public async Task<DialogTurnResult> RunAsync(
        ITurnContext turnContext,
        AgentState agentState,
        CancellationToken cancellationToken
    )
    {
        return await this.dialog.RunAsync(
            turnContext,
            agentState,
            cancellationToken
        );
    }

}
