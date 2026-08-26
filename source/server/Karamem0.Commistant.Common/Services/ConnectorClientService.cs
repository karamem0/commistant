//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Agents.Authentication;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.App;
using Microsoft.Agents.Builder.App.Proactive;
using Microsoft.Agents.Core.Models;
using Microsoft.Agents.Extensions.Teams.Connector;
using Microsoft.Agents.Extensions.Teams.Models;
using System.Threading;

namespace Karamem0.Commistant.Services;

public interface IConnectorClientService
{

    Task SendActivityAsync(
        ConversationReference conversationReference,
        IActivity activity,
        CancellationToken cancellationToken = default
    );

    Task<MeetingInfo> GetMeetingInfoAsync(
        ConversationReference conversationReference,
        string meetingId,
        CancellationToken cancellationToken = default
    );

}

public class ConnectorClientService(IAgent agent, IConnections connections) : IConnectorClientService
{

    private readonly AgentApplication agent = (AgentApplication)agent;

    private readonly IConnections connections = connections;

    public async Task SendActivityAsync(
        ConversationReference conversationReference,
        IActivity activity,
        CancellationToken cancellationToken = default
    )
    {
        var connection = this.connections.GetDefaultConnection();
        var conversation = new Conversation(
            AgentClaims.CreateIdentity(connection.ConnectionSettings.ClientId),
            conversationReference
        );
        _ = await this.agent.Proactive.SendActivityAsync(
            conversation,
            activity,
            cancellationToken
        );
    }

    public async Task<MeetingInfo> GetMeetingInfoAsync(
        ConversationReference conversationReference,
        string meetingId,
        CancellationToken cancellationToken = default
    )
    {
        var completionSource = new TaskCompletionSource<MeetingInfo>(TaskCreationOptions.RunContinuationsAsynchronously);
        var connection = this.connections.GetDefaultConnection();
        var conversation = new Conversation(
            AgentClaims.CreateIdentity(connection.ConnectionSettings.ClientId),
            conversationReference
        );
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"0#{meetingId}#0"));
        await this.agent.Proactive.ContinueConversationAsync(
            conversation,
            async (
                turnContext,
                turnState,
                cancellationToken
            ) =>
            {
                try
                {
                    _ = completionSource.TrySetResult(
                        await TeamsInfo.GetMeetingInfoAsync(
                            turnContext,
                            base64,
                            cancellationToken
                        )
                    );
                }
                catch (Exception ex)
                {
                    _ = completionSource.TrySetException(ex);
                }
            },
            cancellationToken: cancellationToken
        );
        return await completionSource.Task.WaitAsync(cancellationToken);
    }

}
