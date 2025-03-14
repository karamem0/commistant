//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.Identity;
using Azure.Storage.Blobs;
using Karamem0.Commistant.Commands;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Options;
using Karamem0.Commistant.Services;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Rest;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant;

public static class ConfigureServices
{

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddTransient<IDateTimeService, DateTimeService>();
        _ = services.AddSingleton(
            provider =>
            {
                var options = configuration.Get<AzureBlobsStorageOptions>() ?? throw new InvalidOperationException();
                return new BlobContainerClient(options.AzureBlobsContainerEndpoint, new DefaultAzureCredential());
            }
        );
        _ = services.AddTransient<IBlobsStorageService, BlobsStorageService>();
        _ = services.AddSingleton<ServiceClientCredentials>(
            provider =>
            {
                var options = configuration.Get<MicrosoftBotFrameworkOptions>() ?? throw new InvalidOperationException();
                return new MicrosoftAppCredentials(options.MicrosoftAppId, options.MicrosoftAppPassword);
            }
        );
        _ = services.AddTransient<IBotConnectorFactory, BotConnectorFactory>();
        _ = services.AddTransient<IBotConnectorService, BotConnectorService>();
        _ = services.AddSingleton<QRCodeGenerator>();
        _ = services.AddTransient<IQRCodeService, QRCodeService>();
        return services;
    }

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        _ = services.AddSingleton<StartMeetingCommand>();
        _ = services.AddSingleton<EndMeetingCommand>();
        _ = services.AddSingleton<InMeetingCommand>();
        _ = services.AddSingleton(
            (provider) => new CommandSet()
                .Add(provider.GetService<StartMeetingCommand>())
                .Add(provider.GetService<EndMeetingCommand>())
                .Add(provider.GetService<InMeetingCommand>())
        );
        return services;
    }

}
