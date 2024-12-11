//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Bot.Connector;
using Microsoft.Bot.Schema;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Services;

public interface IConnectorClientService
{

    Task<ResourceResponse> SendActivityAsync(
        Uri url,
        Activity activity,
        CancellationToken cancellationToken = default
    );

}

public class ConnectorClientService(ServiceClientCredentials credentials) : IConnectorClientService
{

    private readonly ServiceClientCredentials credentials = credentials;

    public async Task<ResourceResponse> SendActivityAsync(
        Uri url,
        Activity activity,
        CancellationToken cancellationToken = default
    )
    {
        var client = new ConnectorClient(url, this.credentials);
        return await client.Conversations.SendToConversationAsync(activity, cancellationToken);
    }

}
