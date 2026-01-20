//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Commands;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Functions;
using Karamem0.Commistant.Mappings;
using Karamem0.Commistant.Services;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Karamem0.Commistant;

public static class ConfigureServices
{

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        _ = services.AddTransient<MeetingStartedCommand>();
        _ = services.AddTransient<MeetingEndingCommand>();
        _ = services.AddTransient<MeetingInProgressCommand>();
        _ = services.AddTransient((provider) => new CommandSet()
            .Add(provider.GetService<MeetingStartedCommand>())
            .Add(provider.GetService<MeetingEndingCommand>())
            .Add(provider.GetService<MeetingInProgressCommand>())
        );
        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        _ = services.AddTransient(provier =>
            {
                TypeAdapterConfig.GlobalSettings.Apply(
                    [
                        new MapperConfiguration(),
                        new MeetingEndingCommand.MapperConfiguration(provier.GetRequiredService<IQRCodeService>()),
                        new MeetingInProgressCommand.MapperConfiguration(provier.GetRequiredService<IQRCodeService>()),
                        new MeetingStartedCommand.MapperConfiguration(provier.GetRequiredService<IQRCodeService>()),
                        new GetSettingsFunction.MapperConfiguration(),
                        new SetSettingsFunction.MapperConfiguration()
                    ]
                );
                return TypeAdapterConfig.GlobalSettings;
            }
        );
        _ = services.AddTransient<IMapper, ServiceMapper>();
        return services;
    }

}
