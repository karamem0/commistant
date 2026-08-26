//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.Identity;
using Azure.Storage;
using Karamem0.Commistant.Adapters;
using Karamem0.Commistant.Options;
using Karamem0.Commistant.Services;
using Microsoft.Agents.Builder.Adapters;
using Microsoft.Agents.Builder.App;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Hosting.AspNetCore;
using Microsoft.Agents.Storage;
using Microsoft.Agents.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenAI;
using QRCoder;

namespace Karamem0.Commistant;

public static class ConfigureServices
{

    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<AzureStorageBlobsOptions>(configuration.GetSection("AzureStorageBlobs"));
        _ = services.Configure<AzureOpenAIOptions>(configuration.GetSection("AzureOpenAI"));
        return services;
    }

    public static void AddAgent<T>(this IHostApplicationBuilder builder, IConfiguration configuration) where T : AgentApplication
    {
        _ = builder.AddAgent<T, AdapterWithErrorHandler>();
        _ = builder.Services.AddSingleton((provider) => new AgentApplicationOptions(provider.GetRequiredService<IStorage>())
            {
                ChannelAdapterRegistry = provider.GetRequiredService<IChannelAdapterRegistry>(),
                TurnStateFactory = () => new TurnState(
                    provider.GetRequiredService<ConversationState>(),
                    provider.GetRequiredService<UserState>(),
                    new TempState()
                )
            }
        );
        var options = configuration
            .GetSection("AzureStorageBlobs")
            .Get<AzureStorageBlobsOptions>();
        _ = options ?? throw new InvalidOperationException($"{nameof(AzureStorageBlobsOptions)} を null にはできません");
        _ = builder.Services.AddSingleton<IStorage>(
            new BlobsStorage(
                new Uri(options.Endpoint, options.ContainerName),
                new DefaultAzureCredential(new DefaultAzureCredentialOptions()),
                new StorageTransferOptions()
            )
        );
        _ = builder.Services.AddSingleton<ConversationState>();
        _ = builder.Services.AddSingleton<UserState>();
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
