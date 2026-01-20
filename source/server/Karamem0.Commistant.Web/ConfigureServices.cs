//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Azure.Identity;
using Azure.Storage;
using Karamem0.Commistant.Adapters;
using Karamem0.Commistant.Agents;
using Karamem0.Commistant.Dialogs;
using Karamem0.Commistant.Mappings;
using Karamem0.Commistant.Options;
using Karamem0.Commistant.Routes;
using Karamem0.Commistant.Services;
using Mapster;
using MapsterMapper;
using Microsoft.Agents.Builder.App;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Hosting.AspNetCore;
using Microsoft.Agents.Storage;
using Microsoft.Agents.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Karamem0.Commistant;

public static class ConfigureServices
{

    public static void AddAgent(this IHostApplicationBuilder builder, IConfiguration configuration)
    {
        _ = builder.AddAgent<TeamsAgentApplication, AdapterWithErrorHandler>();
        _ = builder.Services.AddSingleton((provider) => new AgentApplicationOptions(provider.GetRequiredService<IStorage>())
            {
                TurnStateFactory = () => new TurnState(
                    provider.GetRequiredService<ConversationState>(),
                    provider.GetRequiredService<UserState>(),
                    new TempState()
                )
            }
        );
        var options = configuration
            .GetSection("AzureStorageBlobs")
            .Get<AzureStorageBlobsOptions>();
        _ = options ?? throw new InvalidOperationException($"{nameof(AzureStorageBlobsOptions)} を null にはできません");
        _ = builder.Services.AddSingleton<IStorage>(
            new BlobsStorage(
                new Uri(options.Endpoint, options.ContainerName),
                new DefaultAzureCredential(new DefaultAzureCredentialOptions()),
                new StorageTransferOptions()
            )
        );
        _ = builder.Services.AddSingleton<ConversationState>();
        _ = builder.Services.AddSingleton<UserState>();
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration
            .GetSection("TokenValidation")
            .Get<TokenValidationOptions>();
        _ = options ?? throw new InvalidOperationException($"{nameof(TokenValidationOptions)} を null にはできません");
        _ = services
            .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            )
            .AddJwtBearer(jwtBearerOptions =>
                {
                    jwtBearerOptions.SaveToken = true;
                    jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromMinutes(5),
                        RequireSignedTokens = true,
                        SignatureValidator = (token, parameters) => new JsonWebToken(token),
                        ValidIssuers =
                        [
                            "https://api.botframework.com",
                            "https://sts.windows.net/d6d49420-f39b-4df7-a1dc-d59a935871db/",
                            "https://login.microsoftonline.com/d6d49420-f39b-4df7-a1dc-d59a935871db/v2.0",
                            "https://sts.windows.net/f8cdef31-a31e-4b4a-93e4-5f571e91255a/",
                            "https://login.microsoftonline.com/f8cdef31-a31e-4b4a-93e4-5f571e91255a/v2.0",
                            $"https://sts.windows.net/{options.TenantId}/",
                            $"https://login.microsoftonline.com/{options.TenantId}/v2.0",
                        ],
                        ValidAudiences = options.Audiences ?? [],
                    };
                }
            );
        return services;
    }

    public static IServiceCollection AddDialogs(this IServiceCollection services)
    {
        _ = services.AddTransient<MeetingStartedDialog>();
        _ = services.AddTransient<MeetingEndingDialog>();
        _ = services.AddTransient<MeetingInProgressDialog>();
        _ = services.AddTransient<InitializeDialog>();
        _ = services.AddTransient<IDialogSetFactory, DialogSetFactory>();
        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        _ = services.AddTransient(provider =>
            {
                TypeAdapterConfig.GlobalSettings.Apply(
                    [
                        new MapperConfiguration(),
                        new MeetingEndingDialog.MapperConfiguration(provider.GetRequiredService<IQRCodeService>()),
                        new MeetingInProgressDialog.MapperConfiguration(provider.GetRequiredService<IQRCodeService>()),
                        new MeetingStartedDialog.MapperConfiguration(provider.GetRequiredService<IQRCodeService>()),
                        new InitializeDialog.MapperConfiguration()
                    ]
                );
                return TypeAdapterConfig.GlobalSettings;
            }
        );
        _ = services.AddTransient<IMapper, ServiceMapper>();
        return services;
    }

    public static IServiceCollection AddRoutes(this IServiceCollection services)
    {
        _ = services.AddTransient<BeforeTurnRouteHandler>();
        _ = services.AddTransient<MemberAddedRouteHandler>();
        _ = services.AddTransient<MemberRemovedRouteHandler>();
        _ = services.AddTransient<MessageRouteHandler>();
        _ = services.AddTransient<MeetingStartedRouteHandler>();
        _ = services.AddTransient<MeetingEndedRouteHandler>();
        return services;
    }

}
