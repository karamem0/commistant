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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var services = builder.Services;
_ = services.AddApplicationInsightsTelemetry();
_ = services.AddAutoMapper(config => config.AddProfile<AutoMapperProfile>());
_ = services.AddBlobContainerClient(configuration);
_ = services.AddServiceClientCredentials(configuration);
_ = services.AddMicrosoftIdentityWebApiAuthentication(configuration, "AzureAD");
_ = services.AddControllers();
_ = services.AddHttpClient();
_ = services.AddCors(options =>
    options.AddDefaultPolicy(builder => builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
    _ = app.UseCors();
}
_ = app.UseHttpsRedirection();
_ = app.UseDefaultFiles();
_ = app.UseStaticFiles();
_ = app.UseRouting();
_ = app.UseAuthentication();
_ = app.UseAuthorization();
_ = app.MapControllers();
_ = app.MapFallbackToFile("/index.html");

await app.RunAsync();

#pragma warning restore CA1852
