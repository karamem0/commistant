//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var azurite = builder
    .AddAzureStorage("azurite")
    .RunAsEmulator(azurite => azurite
        .WithBlobPort(10000)
        .WithQueuePort(10001)
        .WithTablePort(10002)
    );

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

var openAIDeploymentName = builder.AddParameter("AzureOpenAIDeploymentName");

var botFrameworkMicrosoftAppId = builder.AddParameter("BotFrameworkMicrosoftAppId");
var botFrameworkMicrosoftAppPassword = builder.AddParameter("BotFrameworkMicrosoftAppPassword", secret: true);
var botFrameworkMicrosoftAppTenantId = builder.AddParameter("BotFrameworkMicrosoftAppTenantId");

var microsoftIdentityClientId = builder.AddParameter("MicrosoftIdentityClientId");
var microsoftIdentityClientSecret = builder.AddParameter("MicrosoftIdentityClientSecret");
var microsoftIdentityTenantId = builder.AddParameter("MicrosoftIdentityTenantId");

_ = builder
    .AddAzureFunctionsProject<Projects.Karamem0_Commistant_Function>("function-app")
    .WaitFor(storageContainer)
    .WaitFor(azurite)
    .WithHostStorage(azurite)
    .WithEnvironment("AzureStorageBlobs:Endpoint", storageAccount.Resource.BlobEndpoint)
    .WithEnvironment("AzureStorageBlobs:ContainerName", storageContainer.Resource.Name)
    .WithEnvironment("BotFramework:MicrosoftAppId", botFrameworkMicrosoftAppId)
    .WithEnvironment("BotFramework:MicrosoftAppPassword", botFrameworkMicrosoftAppPassword)
    .WithEnvironment("BotFramework:MicrosoftAppTenantId", botFrameworkMicrosoftAppTenantId)
    .WithEnvironment("MicrosoftIdentity:ClientId", microsoftIdentityClientId)
    .WithEnvironment("MicrosoftIdentity:ClientSecret", microsoftIdentityClientSecret)
    .WithEnvironment("MicrosoftIdentity:TenantId", microsoftIdentityTenantId);

_ = builder
    .AddProject<Projects.Karamem0_Commistant_Web>("web-app")
    .WaitFor(openAI)
    .WaitFor(storageContainer)
    .WithEnvironment("AzureOpenAI:Endpoint", $"https://{openAIName}.openai.azure.com/")
    .WithEnvironment("AzureOpenAI:DeploymentName", openAIDeploymentName)
    .WithEnvironment("AzureStorageBlobs:Endpoint", storageAccount.Resource.BlobEndpoint)
    .WithEnvironment("AzureStorageBlobs:ContainerName", storageContainer.Resource.BlobContainerName)
    .WithEnvironment("BotFramework:MicrosoftAppId", botFrameworkMicrosoftAppId)
    .WithEnvironment("BotFramework:MicrosoftAppPassword", botFrameworkMicrosoftAppPassword)
    .WithEnvironment("BotFramework:MicrosoftAppTenantId", botFrameworkMicrosoftAppTenantId);

var app = builder.Build();

await app.RunAsync();
