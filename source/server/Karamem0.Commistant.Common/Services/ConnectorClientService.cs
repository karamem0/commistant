//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Options;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Connector;
using Microsoft.Agents.Core.Models;
using Microsoft.Agents.Extensions.Teams.Connector;
using Microsoft.Agents.Extensions.Teams.Models;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading;

namespace Karamem0.Commistant.Services;

public interface IConnectorClientService
{

    Task<ResourceResponse> SendActivityAsync(
        string serviceUrl,
        IActivity activity,
        CancellationToken cancellationToken = default
    );

    Task<MeetingInfo> GetMeetingInfoAsync(
        string serviceUrl,
        string meetingId,
        CancellationToken cancellationToken = default
    );

}

public class ConnectorClientService(IChannelServiceClientFactory factory, IOptions<ConnectorClientOptions> options) : IConnectorClientService
{

    private readonly IChannelServiceClientFactory factory = factory;

    private readonly ConnectorClientOptions options = options.Value;

    public async Task<ResourceResponse> SendActivityAsync(
        string serviceUrl,
        IActivity activity,
        CancellationToken cancellationToken = default
    )
    {
        var claimsIdentity = new ClaimsIdentity(
            [
                new Claim("ver", "1.0"),
                new Claim("appid", this.options.ClientId)
            ]
        );
        var connectorClient = await this.factory.CreateConnectorClientAsync(
            claimsIdentity,
            serviceUrl,
            this.options.Audience,
            cancellationToken: cancellationToken
        );
        return await connectorClient.Conversations.SendToConversationAsync(activity, cancellationToken);
    }

    public async Task<MeetingInfo> GetMeetingInfoAsync(
        string serviceUrl,
        string meetingId,
        CancellationToken cancellationToken = default
    )
    {
        var claimsIdentity = new ClaimsIdentity(
            [
                new Claim("ver", "1.0"),
                new Claim("appid", this.options.ClientId)
            ]
        );
        var connectorClient = await this.factory.CreateConnectorClientAsync(
            claimsIdentity,
            serviceUrl,
            this.options.Audience,
            cancellationToken: cancellationToken
        );
        var teamsConnectorClient = new RestTeamsConnectorClient(connectorClient, (IRestTransport)connectorClient);
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"0#{meetingId}#0"));
        return await teamsConnectorClient.Teams.FetchMeetingInfoAsync(base64, cancellationToken: cancellationToken);
    }

}
