//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

#pragma warning disable IDE0053
#pragma warning disable IDE0075

using Karamem0.Commistant;
using Microsoft.Agents.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

_ = builder.WebHost.ConfigureKestrel(options =>
    {
        options.AddServerHeader = false;
    }
);

builder.AddAgent(configuration);
builder.AddAzureOpenAIClient(configuration);
builder.AddAzureBlobContainerClient(configuration);

_ = services.AddHttpClient();
_ = services.AddApplicationInsightsTelemetry();
_ = services.AddAuthentication(configuration);
_ = services.AddAuthorization();
_ = services.ConfigureOptions(configuration);
_ = services.AddMapper();
_ = services.AddDialogs();
_ = services.AddRoutes();
_ = services.AddServices(configuration);

var app = builder.Build();
_ = app.UseHttpsRedirection();
_ = app.UseHsts();
_ = app.UseStaticFiles();
_ = app.MapFallbackToFile("/index.html");
_ = app.Use(async (context, next) =>
    {
        var headers = context.Response.Headers;
        headers.ContentSecurityPolicy = "default-src 'self'; " +
                                        $"connect-src 'self' *.{context.Request.Host.Host} *.azure.com *.microsoft.com *.office.net *.visualstudio.com; " +
                                        "frame-ancestors 'self' *.cloud.microsoft *.microsoft365.com *.office.com teams.microsoft.com; " +
                                        "img-src 'self' blob: data:; " +
                                        "style-src 'self' 'unsafe-inline'";
        headers.XContentTypeOptions = "nosniff";
        headers.XFrameOptions = "ALLOW-FROM https://teams.microsoft.com/";
        headers["Permissions-Policy"] = "camera=(), fullscreen=(), geolocation=(), microphone=()";
        headers["Referrer-Policy"] = "same-origin";
        await next();
    }
);
_ = app.MapAgentEndpoints(app.Environment.IsDevelopment() ? false : true);

await app.RunAsync();

#pragma warning restore IDE0053
#pragma warning restore IDE0075
