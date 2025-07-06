//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant;
using Karamem0.Commistant.Mappings;
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
services.AddHttpClient();
services
    .AddControllers()
    .AddNewtonsoftJson();
services.AddAutoMapper(config => config.AddProfile<AutoMapperProfile>());
services.AddApplicationInsightsTelemetry();
services.ConfigureOptions(configuration);
services.AddServices(configuration);
services.AddBots(configuration);
services.AddDialogs();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseWebSockets();
app.UseRouting();
app.MapControllers();
app.MapFallbackToFile("index.html");

await app.RunAsync();
