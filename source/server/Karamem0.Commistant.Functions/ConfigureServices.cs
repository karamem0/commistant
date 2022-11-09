//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

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
using System.Threading.Tasks;

namespace Karamem0.Commistant
{

    public static class ConfigureServices
    {

        public static IServiceCollection AddBlobContainerClient(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton(provider => new BlobServiceClient(
                    configuration.GetValue<string>("AzureBlobStogageConnectionString")))
                .AddSingleton(provider => provider
                    .GetService<BlobServiceClient>()?
                    .GetBlobContainerClient(configuration.GetValue<string>("AzureBlobStogageContainerName"))
                    ?? throw new InvalidOperationException());
        }

        public static IServiceCollection AddCommands(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton<ServiceClientCredentials>(new MicrosoftAppCredentials(
                    configuration.GetValue<string>("MicrosoftAppId"),
                    configuration.GetValue<string>("MicrosoftAppPassword")))
                .AddSingleton<StartMeetingCommand>()
                .AddSingleton<EndMeetingCommand>()
                .AddSingleton<InMeetingCommand>()
                .AddSingleton((provider) => new CommandSet()
                    .Add(provider.GetService<StartMeetingCommand>())
                    .Add(provider.GetService<EndMeetingCommand>())
                    .Add(provider.GetService<InMeetingCommand>()));
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services.AddTransient<QrCodeService>();
        }

    }

}
