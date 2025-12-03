//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Options;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Rest;
using OpenAI;
using QRCoder;

namespace Karamem0.Commistant;

public static class ConfigureServices
{

    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<AzureStorageBlobsOptions>(configuration.GetSection("AzureStorageBlobs"));
        _ = services.Configure<AzureOpenAIOptions>(configuration.GetSection("AzureOpenAI"));
        _ = services.Configure<BotFrameworkOptions>(configuration.GetSection("BotFramework"));
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
                _ = options ?? throw new InvalidOperationException();
                return client.GetChatClient(options.DeploymentName);
            }
        );
        _ = services.AddSingleton<ServiceClientCredentials>(provider =>
            {
                var options = configuration
                    .GetSection("BotFramework")
                    .Get<BotFrameworkOptions>();
                _ = options ?? throw new InvalidOperationException();
                var credentials = new MicrosoftAppCredentials(options.MicrosoftAppId, options.MicrosoftAppPassword)
                {
                    ChannelAuthTenant = options.MicrosoftAppTenantId
                };
                return credentials;
            }
        );
        _ = services.AddSingleton<QRCodeGenerator>();
        _ = services.AddScoped<IBlobsService, BlobsService>();
        _ = services.AddScoped<IBotConnectorFactory, BotConnectorFactory>();
        _ = services.AddScoped<IBotConnectorService, BotConnectorService>();
        _ = services.AddScoped<IDateTimeService, DateTimeService>();
        _ = services.AddScoped<IMeetingService, MeetingService>();
        _ = services.AddScoped<IQRCodeService, QRCodeService>();
        _ = services.AddScoped<IOpenAIService, OpenAIService>();
        return services;
    }

}
