//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.Identity;
using Karamem0.Commistant.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Karamem0.Commistant;

public static class ConfigureAzureResources
{

    public static void AddAzureOpenAIClient(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        _ = builder.AddAzureOpenAIClient(
            "openai",
            configureSettings: settings =>
            {
                var options = configuration
                    .GetSection("AzureOpenAI")
                    .Get<AzureOpenAIOptions>();
                _ = options ?? throw new InvalidOperationException($"{nameof(AzureOpenAIOptions)} を null にはできません");
                settings.Endpoint = options.Endpoint;
                settings.Credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions());
            }
        );
    }

    public static void AddAzureBlobContainerClient(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        builder.AddAzureBlobContainerClient(
            "azure-bot-states",
            configureSettings: settings =>
            {
                var options = configuration
                    .GetSection("AzureStorageBlobs")
                    .Get<AzureStorageBlobsOptions>();
                _ = options ?? throw new InvalidOperationException($"{nameof(AzureStorageBlobsOptions)} を null にはできません");
                settings.ServiceUri = options.Endpoint;
                settings.BlobContainerName = options.ContainerName;
                settings.Credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions());
            }
        );
    }

}
