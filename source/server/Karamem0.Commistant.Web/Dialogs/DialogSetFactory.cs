//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Agents.Builder.Dialogs;
using Microsoft.Agents.Builder.State;
using Microsoft.Extensions.DependencyInjection;

namespace Karamem0.Commistant.Dialogs;

public interface IDialogSetFactory
{

    DialogSet Create();

}

public class DialogSetFactory(IServiceProvider provider) : IDialogSetFactory
{

    private readonly IServiceProvider provider = provider;

    public DialogSet Create()
    {
        var conversationState = this.provider.GetRequiredService<ConversationState>();
        var dialogState = conversationState.GetValue<DialogState>(nameof(DialogState), () => new());
        var dialogSet = new DialogSet(dialogState);
        _ = dialogSet.Add(this.provider.GetRequiredService<MeetingStartedDialog>());
        _ = dialogSet.Add(this.provider.GetRequiredService<MeetingEndingDialog>());
        _ = dialogSet.Add(this.provider.GetRequiredService<MeetingInProgressDialog>());
        _ = dialogSet.Add(this.provider.GetRequiredService<InitializeDialog>());
        return dialogSet;
    }

}
