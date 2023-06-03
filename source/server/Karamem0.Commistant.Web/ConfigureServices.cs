//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

    }

}
