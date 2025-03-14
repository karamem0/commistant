//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.AI.OpenAI;
using Azure.Identity;
using Azure.Storage;
using Azure.Storage.Blobs;
using Karamem0.Commistant.Adapters;
using Karamem0.Commistant.Bots;
using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Options;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Azure.Blobs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Rest;
using OpenAI;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant;

public static class ConfigureServices
{

    public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<AzureBlobsStorageOptions>(configuration.GetSection(nameof(AzureBlobsStorageOptions)));
        _ = services.Configure<AzureOpenAIOptions>(configuration.GetSection(nameof(AzureOpenAIOptions)));
        _ = services.Configure<MicrosoftAppCredentials>(configuration.GetSection(nameof(MicrosoftAppCredentials)));
        return services;
    }

    public static IServiceCollection AddBots(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.Get<AzureBlobsStorageOptions>() ?? throw new InvalidOperationException();
        _ = services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();
        _ = services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
        _ = services.AddSingleton<IStorage>(
            new BlobsStorage(
                options.AzureBlobsContainerEndpoint,
                new DefaultAzureCredential(),
                new StorageTransferOptions()
            )
        );
        _ = services.AddSingleton<ConversationState>();
        _ = services.AddScoped<IBot, ActivityBot>();
        return services;
    }

    public static IServiceCollection AddDialogs(this IServiceCollection services)
    {
        _ = services.AddScoped<StartMeetingDialog>();
        _ = services.AddScoped<EndMeetingDialog>();
        _ = services.AddScoped<InMeetingDialog>();
        _ = services.AddScoped<ResetDialog>();
        _ = services.AddScoped(
            provider => new DialogSet(
                    provider
                        .GetRequiredService<ConversationState>()
                        .CreateProperty<DialogState>(nameof(DialogState))
                )
                .Add(provider.GetService<StartMeetingDialog>())
                .Add(provider.GetService<EndMeetingDialog>())
                .Add(provider.GetService<InMeetingDialog>())
                .Add(provider.GetService<ResetDialog>())
        );
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddSingleton<OpenAIClient>(
            provider =>
            {
                var options = configuration.Get<AzureOpenAIOptions>() ?? throw new InvalidOperationException();
                return new AzureOpenAIClient(options.AzureOpenAIEndpoint, new DefaultAzureCredential());
            }
        );
        _ = services.AddScoped<IOpenAIService, OpenAIService>();
        _ = services.AddSingleton(
            provider =>
            {
                var options = configuration.Get<AzureBlobsStorageOptions>() ?? throw new InvalidOperationException();
                return new BlobContainerClient(options.AzureBlobsContainerEndpoint, new DefaultAzureCredential());
            }
        );
        _ = services.AddScoped<IBlobsStorageService, BlobsStorageService>();
        _ = services.AddSingleton<ServiceClientCredentials>(
            provider =>
            {
                var options = configuration.Get<MicrosoftBotFrameworkOptions>() ?? throw new InvalidOperationException();
                return new MicrosoftAppCredentials(options.MicrosoftAppId, options.MicrosoftAppPassword);
            }
        );
        _ = services.AddScoped<IBotConnectorFactory, BotConnectorFactory>();
        _ = services.AddScoped<IBotConnectorService, BotConnectorService>();
        _ = services.AddSingleton<QRCodeGenerator>();
        _ = services.AddScoped<IQRCodeService, QRCodeService>();
        return services;
    }

}
