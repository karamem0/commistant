//
// Copyright (c) 2022-2025 karamem0
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
                _ = options ?? throw new InvalidOperationException();
                settings.Endpoint = options.Endpoint;
                settings.Credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions());
            }
        );
    }

    public static void AddAzureBlobContainerClient(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        builder.AddAzureBlobContainerClient(
            "container",
            configureSettings: settings =>
            {
                var options = configuration
                    .GetSection("AzureStorageBlobs")
                    .Get<AzureStorageBlobsOptions>();
                _ = options ?? throw new InvalidOperationException();
                settings.ServiceUri = options.Endpoint;
                settings.BlobContainerName = options.ContainerName;
                settings.Credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions());
            }
        );
    }

}
