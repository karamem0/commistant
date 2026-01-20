//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Hosting.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.AddAgent(builder.Configuration);
builder.AddAzureOpenAIClient(configuration);
builder.AddAzureBlobContainerClient(configuration);

var services = builder.Services;

_ = services.AddHttpClient();
_ = services.AddApplicationInsightsTelemetry();
_ = services.AddAuthentication(configuration);
_ = services.AddAuthorization();
_ = services.ConfigureOptions(configuration);
_ = services.AddMapper();
_ = services.AddRoutes();
_ = services.AddDialogs();
_ = services.AddServices(configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    _ = app.UseDeveloperExceptionPage();
}
_ = app.UseHttpsRedirection();
_ = app.UseHsts();
_ = app.UseStaticFiles();
_ = app.MapFallbackToFile("/index.html");

_ = app
    .MapPost(
        "/api/messages",
        async (
            HttpRequest request,
            HttpResponse response,
            IAgentHttpAdapter adapter,
            IAgent agent,
            CancellationToken cancellationToken
        ) => await adapter.ProcessAsync(
            request,
            response,
            agent,
            cancellationToken
        )
    )
    .RequireAuthorization();

await app.RunAsync();
