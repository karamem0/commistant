//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Teams;
using Microsoft.Rest;

namespace Karamem0.Commistant.Services;

public interface IBotConnectorFactory
{

    ConnectorClient CreateConnectorClient(Uri url);

    TeamsConnectorClient CreateTeamsConnectorClient(Uri url);

}

public class BotConnectorFactory(ServiceClientCredentials credentials) : IBotConnectorFactory
{

    private readonly ServiceClientCredentials credentials = credentials;

    public ConnectorClient CreateConnectorClient(Uri url)
    {
        return new ConnectorClient(url, this.credentials);
    }

    public TeamsConnectorClient CreateTeamsConnectorClient(Uri url)
    {
        return new TeamsConnectorClient(url, this.credentials);
    }

}
