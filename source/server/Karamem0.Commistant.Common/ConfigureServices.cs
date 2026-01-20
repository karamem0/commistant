//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Options;
using Karamem0.Commistant.Services;
using Microsoft.Agents.Authentication;
using Microsoft.Agents.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using QRCoder;

namespace Karamem0.Commistant;

public static class ConfigureServices
{

    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<AzureStorageBlobsOptions>(configuration.GetSection("AzureStorageBlobs"));
        _ = services.Configure<AzureOpenAIOptions>(configuration.GetSection("AzureOpenAI"));
        _ = services.Configure<ConnectorClientOptions>(configuration.GetSection("ConnectorClient"));
        return services;
    }

    public static IServiceCollection AddConnectorClient(this IServiceCollection services)
    {
        _ = services.AddSingleton<IConnections, ConfigurationConnections>();
        _ = services.AddSingleton<IChannelServiceClientFactory, RestChannelServiceClientFactory>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddSingleton(provider =>
            {
                var client = provider.GetRequiredService<OpenAIClient>();
                var options = configuration
                    .GetSection("AzureOpenAI")
                    .Get<AzureOpenAIOptions>();
                _ = options ?? throw new InvalidOperationException($"{nameof(AzureOpenAIOptions)} を null にはできません");
                return client.GetChatClient(options.DeploymentName);
            }
        );
        _ = services.AddSingleton<QRCodeGenerator>();
        _ = services.AddTransient<IBlobsService, BlobsService>();
        _ = services.AddTransient<IConnectorClientService, ConnectorClientService>();
        _ = services.AddTransient<IDateTimeService, DateTimeService>();
        _ = services.AddTransient<IMeetingService, MeetingService>();
        _ = services.AddTransient<IQRCodeService, QRCodeService>();
        _ = services.AddTransient<IOpenAIService, OpenAIService>();
        return services;
    }

}
