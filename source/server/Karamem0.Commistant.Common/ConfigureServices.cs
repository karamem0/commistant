//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.AI.OpenAI;
using Azure.Identity;
using Azure.Storage.Blobs;
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

    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.Configure<AzureBlobsStorageOptions>(configuration.GetSection("AzureBlobsStorage"));
        _ = services.Configure<AzureOpenAIOptions>(configuration.GetSection("AzureOpenAI"));
        _ = services.Configure<BotFrameworkOptions>(configuration.GetSection("BotFramework"));
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        _ = services.AddSingleton(
            provider =>
            {
                var options = configuration
                                  .GetSection("AzureBlobsStorage")
                                  .Get<AzureBlobsStorageOptions>() ??
                              throw new InvalidOperationException();
                return new BlobContainerClient(new Uri(options.Endpoint ?? throw new InvalidOperationException(), options.ContainerName), new DefaultAzureCredential());
            }
        );
        _ = services.AddScoped<IBlobService, BlobService>();
        _ = services.AddSingleton<ServiceClientCredentials>(
            provider =>
            {
                var options = configuration
                                  .GetSection("BotFramework")
                                  .Get<BotFrameworkOptions>() ??
                              throw new InvalidOperationException();
                return new MicrosoftAppCredentials(options.MicrosoftAppId, options.MicrosoftAppPassword);
            }
        );
        _ = services.AddScoped<IBotConnectorFactory, BotConnectorFactory>();
        _ = services.AddScoped<IBotConnectorService, BotConnectorService>();
        _ = services.AddScoped<IDateTimeService, DateTimeService>();
        _ = services.AddSingleton<QRCodeGenerator>();
        _ = services.AddScoped<IQRCodeService, QRCodeService>();
        _ = services.AddSingleton(
            provider =>
            {
                var options = configuration
                                  .GetSection("AzureOpenAI")
                                  .Get<AzureOpenAIOptions>() ??
                              throw new InvalidOperationException();
                var client = new AzureOpenAIClient(options.Endpoint, new DefaultAzureCredential());
                return client.GetChatClient(options.ModelName);
            }
        );
        _ = services.AddScoped<IOpenAIService, OpenAIService>();
        _ = services.AddSingleton<QRCodeGenerator>();
        _ = services.AddScoped<IQRCodeService, QRCodeService>();
        return services;
    }

}
