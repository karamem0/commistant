//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.AI.OpenAI;
using Azure.Identity;
using Azure.Storage;
using Karamem0.Commistant.Adapters;
using Karamem0.Commistant.Bots;
using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Azure.Blobs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QRCoder;
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
            var blobContainerUrl = configuration.GetValue<string>("AzureBlobStogageContainerUrl") ?? throw new InvalidOperationException();
            _ = services.AddSingleton<BotFrameworkAuthentication, ConfigurationBotFrameworkAuthentication>();
            _ = services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();
            // _ = services.AddSingleton<IStorage>(new MemoryStorage());
            _ = services.AddSingleton<IStorage>(new BlobsStorage(
                new Uri(blobContainerUrl),
                new DefaultAzureCredential(),
                new StorageTransferOptions()
            ));
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
            _ = services.AddScoped(provider => new DialogSet(provider
                .GetService<ConversationState>()?
                .CreateProperty<DialogState>(nameof(DialogState)))
                .Add(provider.GetService<StartMeetingDialog>())
                .Add(provider.GetService<EndMeetingDialog>())
                .Add(provider.GetService<InMeetingDialog>())
                .Add(provider.GetService<ResetDialog>()));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var openAIEndpointUrl = configuration.GetValue<string>("AzureOpenAIEndpointUrl") ?? throw new InvalidOperationException();
            _ = services.AddScoped(provider => new OpenAIClient(
                new Uri(openAIEndpointUrl),
                new DefaultAzureCredential()
            ));
            _ = services.AddScoped<OpenAIService>();
            _ = services.AddScoped<QRCodeGenerator>();
            _ = services.AddScoped<QrCodeService>();
            return services;
        }

    }

}
