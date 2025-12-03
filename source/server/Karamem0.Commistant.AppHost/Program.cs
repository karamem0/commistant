//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

#pragma warning disable IDE0059

using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var openAIName = builder.AddParameter("AzureOpenAIName");
var openAIResourceGroup = builder.AddParameter("AzureOpenAIResourceGroup");
var openAI = builder
    .AddAzureOpenAI("openai")
    .AsExisting(openAIName, openAIResourceGroup);

var storageAccount = builder
    .AddAzureStorage("storage")
    .RunAsEmulator(azurite => azurite
        .WithBlobPort(10000)
        .WithQueuePort(10001)
        .WithTablePort(10002)
    );
var storageContainer = storageAccount.AddBlobContainer("container", "azure-bot-states");

var functionApp = builder
    .AddAzureFunctionsProject<Projects.Karamem0_Commistant_Function>("function-app")
    .WithReference(storageContainer)
    .WaitFor(storageContainer)
    .WithHostStorage(storageAccount);

var webApp = builder
    .AddProject<Projects.Karamem0_Commistant_Web>("web-app")
    .WithReference(openAI)
    .WaitFor(openAI)
    .WithReference(storageContainer)
    .WaitFor(storageContainer);

var app = builder.Build();

await app.RunAsync();

#pragma warning restore IDE0059
