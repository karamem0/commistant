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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Services;

public interface IBotConnectorFactory
{

    ConnectorClient CreateConnectorClient(Uri serviceUrl);

    TeamsConnectorClient CreateTeamsConnectorClient(Uri serviceUrl);

}

public class BotConnectorFactory(ServiceClientCredentials credentials) : IBotConnectorFactory
{

    private readonly ServiceClientCredentials credentials = credentials;

    public ConnectorClient CreateConnectorClient(Uri serviceUrl)
    {
        return new ConnectorClient(serviceUrl, this.credentials);
    }

    public TeamsConnectorClient CreateTeamsConnectorClient(Uri serviceUrl)
    {
        return new TeamsConnectorClient(serviceUrl, this.credentials);
    }

}
