//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.Identity;
using Azure.Storage;
using Karamem0.Commistant.Adapters;
using Karamem0.Commistant.Bots;
using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Options;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Azure.Blobs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant;

public static class ConfigureServices
{

    public static IServiceCollection AddBots(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddSingleton<BotFrameworkAuthentication>(provider => new ConfigurationBotFrameworkAuthentication(configuration.GetSection("BotFramework")));
        _ = services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
        _ = services.AddSingleton<IStorage>(provider =>
            {
                var options = configuration
                                  .GetSection("AzureStorageBlobs")
                                  .Get<AzureStorageBlobsOptions>() ??
                              throw new InvalidOperationException();
                return new BlobsStorage(
                    new Uri(options.Endpoint ?? throw new InvalidOperationException(), options.ContainerName),
                    new DefaultAzureCredential(),
                    new StorageTransferOptions()
                );
            }
        );
        _ = services.AddSingleton<ConversationState>();
        _ = services.AddScoped<IBot, TeamsBot>();
        return services;
    }

    public static IServiceCollection AddDialogs(this IServiceCollection services)
    {
        _ = services.AddScoped<MeetingStartDialog>();
        _ = services.AddScoped<MeetingEndDialog>();
        _ = services.AddScoped<MeetingRunDialog>();
        _ = services.AddScoped<ResetDialog>();
        _ = services.AddScoped(provider => new DialogSet(
                provider
                    .GetService<ConversationState>()
                    ?.CreateProperty<DialogState>(nameof(DialogState))
            )
            .Add(provider.GetService<MeetingStartDialog>())
            .Add(provider.GetService<MeetingEndDialog>())
            .Add(provider.GetService<MeetingRunDialog>())
            .Add(provider.GetService<ResetDialog>())
        );
        return services;
    }

}
