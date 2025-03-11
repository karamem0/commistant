//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

var builder = new HostBuilder();
_ = builder.ConfigureFunctionsWorkerDefaults();
_ = builder.ConfigureAppConfiguration(
    (context, builder) =>
    {
        _ = builder.AddJsonFile(
            "appsettings.json",
            true,
            true
        );
        _ = builder.AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("AZURE_FUNCTIONS_ENVIRONMENT")}.json",
            true,
            true
        );
        _ = builder.AddUserSecrets(typeof(Program).Assembly, true);
        _ = builder.AddEnvironmentVariables();
    }
);
_ = builder.ConfigureServices(
    (context, services) =>
    {
        _ = services.AddApplicationInsightsTelemetryWorkerService();
        _ = services.AddLogging(builder => builder.AddApplicationInsights());
        _ = services.AddHttpClient();
        _ = services.AddBlobContainerClient(context.Configuration);
        _ = services.AddServiceClientCredentials(context.Configuration);
        _ = services.AddServices();
        _ = services.AddCommands();
    }
);

var app = builder.Build();

app.Run();
