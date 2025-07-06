//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant;
using Karamem0.Commistant.Mappings;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

var builder = FunctionsApplication.CreateBuilder(args);

var environmentName = Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT");
var configuration = builder.Configuration;
configuration.AddJsonFile(
    "appsettings.json",
    true,
    true
);
configuration.AddJsonFile(
    $"appsettings.{environmentName}.json",
    true,
    true
);
configuration.AddUserSecrets(typeof(Program).Assembly, true);
configuration.AddEnvironmentVariables();

builder.ConfigureFunctionsWebApplication();
builder.AddAzureBlobContainerClient(configuration);

var services = builder.Services;
services.AddAutoMapper(config => config.AddProfile<AutoMapperProfile>());
services.AddApplicationInsightsTelemetryWorkerService();
services.ConfigureFunctionsApplicationInsights();
services.ConfigureOptions(configuration);
services.AddServices(configuration);
services.AddCommands();

var app = builder.Build();

await app.RunAsync();
