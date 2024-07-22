//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

#pragma warning disable CA1852

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
var services = builder.Services;
_ = services.AddHttpClient();
_ = services
    .AddControllers()
    .AddNewtonsoftJson();
_ = services.AddAutoMapper(config => config.AddProfile<AutoMapperProfile>());
_ = services.AddApplicationInsightsTelemetry();
_ = services.AddBots(configuration);
_ = services.AddDialogs();
_ = services.AddServices(configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}
_ = app.UseDefaultFiles();
_ = app.UseStaticFiles();
_ = app.UseWebSockets();
_ = app.UseRouting();
_ = app.UseAuthorization();
_ = app.MapControllers();

await app.RunAsync();

#pragma warning restore CA1852
