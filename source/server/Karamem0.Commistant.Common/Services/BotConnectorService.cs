//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using System.Threading;

namespace Karamem0.Commistant.Services;

public interface IBotConnectorService
{

    Task<ResourceResponse> SendActivityAsync(
        Uri url,
        Activity activity,
        CancellationToken cancellationToken = default
    );

    Task<MeetingInfo> GetMeetingInfoAsync(
        Uri url,
        string meetingId,
        CancellationToken cancellationToken = default
    );

}

public class BotConnectorService(IBotConnectorFactory botConnectorFactory) : IBotConnectorService
{

    private readonly IBotConnectorFactory botConnectorFactory = botConnectorFactory;

    public async Task<ResourceResponse> SendActivityAsync(
        Uri url,
        Activity activity,
        CancellationToken cancellationToken = default
    )
    {
        var client = this.botConnectorFactory.CreateConnectorClient(url);
        return await client.Conversations.SendToConversationAsync(activity, cancellationToken);
    }

    public async Task<MeetingInfo> GetMeetingInfoAsync(
        Uri url,
        string meetingId,
        CancellationToken cancellationToken = default
    )
    {
        var client = this.botConnectorFactory.CreateTeamsConnectorClient(url);
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"0#{meetingId}#0"));
        return await client.Teams.FetchMeetingInfoAsync(base64, cancellationToken);
    }

}
