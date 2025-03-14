//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant;
using Karamem0.Commistant.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

var builder = Host.CreateApplicationBuilder();

var configuration = builder.Configuration;
var services = builder.Services;

_ = services.AddApplicationInsightsTelemetryWorkerService();
_ = services.AddServices(configuration);
_ = services.AddCommands();
_ = services.AddHostedService<ExecuteCommandService>();

var app = builder.Build();

await app.RunAsync();
