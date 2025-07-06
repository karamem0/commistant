//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

#pragma warning disable IDE0059

using Aspire.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

var builder = DistributedApplication.CreateBuilder(args);

var openai = builder.AddAzureOpenAI("openai")
    .AsExisting(
        builder.AddParameter("AzureOpenAIResourceName"),
        builder.AddParameter("AzureOpenAIResourceGroupName")
    );

var storage = builder
    .AddAzureStorage("storage")
    .RunAsEmulator();
var blobs = storage.AddBlobs("blobs");
var container = blobs.AddBlobContainer("container", "azure-bot-states");

var functionApp = builder
    .AddAzureFunctionsProject<Projects.Karamem0_Commistant_Function>("function-app")
    .WithHostStorage(storage)
    .WithReference(container)
    .WaitFor(container)
    .WithEnvironment("AZURE_FUNCTIONS_ENVIRONMENT", builder.Environment.EnvironmentName);

var webApp = builder
    .AddProject<Projects.Karamem0_Commistant_Web>("web-app")
    .WithReference(openai)
    .WaitFor(openai)
    .WithReference(container)
    .WaitFor(container);

var app = builder.Build();

await app.RunAsync();

#pragma warning restore IDE0059
