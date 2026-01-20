//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;

var builder = FunctionsApplication.CreateBuilder(args);

_ = builder.ConfigureFunctionsWebApplication();

var environmentName = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
var configuration = builder.Configuration;
_ = configuration.AddJsonFile(
    "appsettings.json",
    true,
    true
);
_ = configuration.AddJsonFile(
    $"appsettings.{environmentName}.json",
    true,
    true
);
_ = configuration.AddUserSecrets(typeof(Program).Assembly, true);
_ = configuration.AddEnvironmentVariables();

builder.AddAzureBlobContainerClient(configuration);

var services = builder.Services;
_ = services.AddApplicationInsightsTelemetryWorkerService();
_ = services.ConfigureFunctionsApplicationInsights();
_ = services.AddMicrosoftIdentityWebApiAuthentication(configuration, "MicrosoftIdentity");
_ = services.ConfigureOptions(configuration);
_ = services.AddMapper();
_ = services.AddConnectorClient();
_ = services.AddServices(configuration);
_ = services.AddCommands();

var app = builder.Build();

await app.RunAsync();
