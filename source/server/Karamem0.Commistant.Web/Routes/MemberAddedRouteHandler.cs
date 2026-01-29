//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Logging;
using Karamem0.Commistant.Routes.Abstraction;
using Karamem0.Commistant.Services;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.State;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Karamem0.Commistant.Routes;

public class MemberAddedRouteHandler(
    ConversationState conversationState,
    IDialogService<MainDialog> dialogService,
    ILogger<MemberAddedRouteHandler> logger
) : RouteHandler<MemberAddedRouteHandler>(logger)
{

    private readonly ConversationState conversationState = conversationState;

    private readonly IDialogService<MainDialog> dialogService = dialogService;

    private readonly ILogger<MemberAddedRouteHandler> logger = logger;

    protected override async Task InvokeAsyncCore(
        ITurnContext turnContext,
        ITurnState turnState,
        CancellationToken cancellationToken = default
    )
    {
        this.logger.MembersAdded(conversationId: turnContext.Activity.Conversation.Id);
        foreach (var member in turnContext.Activity.MembersAdded)
        {
            if (member.Id == turnContext.Activity.Recipient.Id)
            {
                _ = await turnContext.SendActivityAsync(
                    """
                    <b>Commistant にようこそ！</b>
                    <br/>
                    Commistant は Microsoft Teams 会議によるコミュニティ イベントをサポートするアシスタント ボットです。
                    会議の開始時、終了時、または会議中に定型のメッセージ通知を送信します。
                    通知にはテキストおよび QR コードつきの URL を添付することができます。
                    """,
                    cancellationToken: cancellationToken
                );
            }
            _ = await this.dialogService.RunAsync(
                turnContext,
                this.conversationState,
                cancellationToken
            );
        }
    }

}
