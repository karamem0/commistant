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

var openAIName = builder.AddParameter("AzureOpenAIName");
var openAIResourceGroup = builder.AddParameter("AzureOpenAIResourceGroup");
var openAI = builder
    .AddAzureOpenAI("openai")
    .AsExisting(openAIName, openAIResourceGroup);

var storageAccountName = builder.AddParameter("AzureStorageAccountName");
var storageAccountResourceGroup = builder.AddParameter("AzureStorageAccountResourceGroup");
var storageAccount = builder
    .AddAzureStorage("storage")
    .AsExisting(storageAccountName, storageAccountResourceGroup);
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
