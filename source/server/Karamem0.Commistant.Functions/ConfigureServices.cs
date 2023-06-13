//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.Identity;
using Azure.Storage.Blobs;
using Karamem0.Commistant.Commands;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant
{

    public static class ConfigureServices
    {

        public static IServiceCollection AddBlobContainerClient(this IServiceCollection services, IConfiguration configuration)
        {
            var blobContainerUrl = configuration.GetValue<string>("AzureBlobStogageContainerUrl") ?? throw new InvalidOperationException();
            _ = services.AddSingleton(provider => new BlobContainerClient(new Uri(blobContainerUrl), new DefaultAzureCredential()));
            return services;
        }

        public static IServiceCollection AddServiceClientCredentials(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddSingleton<ServiceClientCredentials>(new MicrosoftAppCredentials(
                configuration.GetValue<string>("MicrosoftAppId"),
                configuration.GetValue<string>("MicrosoftAppPassword")));
            return services;
        }

        public static IServiceCollection AddCommands(this IServiceCollection services)
        {
            _ = services.AddSingleton<StartMeetingCommand>();
            _ = services.AddSingleton<EndMeetingCommand>();
            _ = services.AddSingleton<InMeetingCommand>();
            _ = services.AddSingleton((provider) => new CommandSet()
                .Add(provider.GetService<StartMeetingCommand>())
                .Add(provider.GetService<EndMeetingCommand>())
                .Add(provider.GetService<InMeetingCommand>()));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            _ = services.AddTransient<QrCodeService>();
            return services;
        }

    }

}
