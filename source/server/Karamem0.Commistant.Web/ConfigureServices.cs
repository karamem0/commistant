//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Adapters;
using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Webs;
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
using System.Threading.Tasks;

namespace Karamem0.Commistant
{

    public static class ConfigureServices
    {

        public static IServiceCollection AddBots(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>()
                .AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>()
                // .AddSingleton<IStorage>(new MemoryStorage())
                .AddSingleton<IStorage>(new BlobsStorage(
                    configuration.GetValue<string>("AzureBlobStogageConnectionString"),
                    configuration.GetValue<string>("AzureBlobStogageContainerName")))
                .AddSingleton<ConversationState>()
                .AddScoped<IBot, ActivityBot>();
        }

        public static IServiceCollection AddDialogs(this IServiceCollection services)
        {
            return services
                .AddScoped<StartMeetingDialog>()
                .AddScoped<EndMeetingDialog>()
                .AddScoped<InMeetingDialog>()
                .AddScoped<ResetDialog>()
                .AddScoped(provider => new DialogSet(provider
                    .GetService<ConversationState>()?
                    .CreateProperty<DialogState>(nameof(DialogState)))
                    .Add(provider.GetService<StartMeetingDialog>())
                    .Add(provider.GetService<EndMeetingDialog>())
                    .Add(provider.GetService<InMeetingDialog>())
                    .Add(provider.GetService<ResetDialog>()));
        }

    }

}
