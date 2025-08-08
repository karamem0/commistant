//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.AddAzureOpenAIClient(configuration);
builder.AddAzureBlobContainerClient(configuration);

var services = builder.Services;
_ = services.AddHttpClient();
_ = services
    .AddControllers()
    .AddNewtonsoftJson();
_ = services.AddApplicationInsightsTelemetry();
_ = services.ConfigureOptions(configuration);
_ = services.AddMapper();
_ = services.AddServices(configuration);
_ = services.AddBots(configuration);
_ = services.AddDialogs();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}
_ = app.UseDefaultFiles();
_ = app.UseStaticFiles();
_ = app.UseWebSockets();
_ = app.UseRouting();
_ = app.MapControllers();
_ = app.MapFallbackToFile("index.html");

await app.RunAsync();
