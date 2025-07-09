//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AutoMapper;
using Azure.Storage.Blobs;
using Karamem0.Commistant.Adapters;
using Karamem0.Commistant.Bots;
using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Mappings;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant;

public static class ConfigureServices
{

    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        _ = services.AddScoped<AutoMapperProfile>();
        _ = services.AddScoped<MeetingEndDialog.AutoMapperProfile>();
        _ = services.AddScoped<MeetingRunDialog.AutoMapperProfile>();
        _ = services.AddScoped<MeetingStartDialog.AutoMapperProfile>();
        _ = services.AddScoped<ResetDialog.AutoMapperProfile>();
        _ = services.AddScoped((provider) =>
            {
                var mapperConfig = new MapperConfiguration(config =>
                    {
                        config.AddProfile(provider.GetService<AutoMapperProfile>());
                        config.AddProfile(provider.GetService<MeetingEndDialog.AutoMapperProfile>());
                        config.AddProfile(provider.GetService<MeetingRunDialog.AutoMapperProfile>());
                        config.AddProfile(provider.GetService<MeetingStartDialog.AutoMapperProfile>());
                        config.AddProfile(provider.GetService<ResetDialog.AutoMapperProfile>());
                    }
                );
                return mapperConfig.CreateMapper();
            }
        );
        return services;
    }

    public static IServiceCollection AddBots(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddSingleton<BotFrameworkAuthentication>(provider => new ConfigurationBotFrameworkAuthentication(configuration.GetSection("BotFramework")));
        _ = services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
        _ = services.AddSingleton<IStorage>(provider =>
            {
                var storage = (BlobsStorage?)Activator.CreateInstance(
                    typeof(BlobsStorage),
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    [provider.GetRequiredService<BlobContainerClient>(), null],
                    null
                );
                _ = storage ?? throw new InvalidOperationException();
                return storage;
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
                    .GetRequiredService<ConversationState>()
                    .CreateProperty<DialogState>(nameof(DialogState))
            )
            .Add(provider.GetService<MeetingStartDialog>())
            .Add(provider.GetService<MeetingEndDialog>())
            .Add(provider.GetService<MeetingRunDialog>())
            .Add(provider.GetService<ResetDialog>())
        );
        return services;
    }

}
