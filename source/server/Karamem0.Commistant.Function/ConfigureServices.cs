//
// Copyright (c) 2022-2025 karamem0
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant;

public static class ConfigureServices
{

    public static IServiceCollection AddCommands(this IServiceCollection services)
    {
        _ = services.AddSingleton<MeetingStartCommand>();
        _ = services.AddSingleton<MeetingEndCommand>();
        _ = services.AddSingleton<MeetingRunCommand>();
        _ = services.AddSingleton((provider) => new CommandSet()
            .Add(provider.GetService<MeetingStartCommand>())
            .Add(provider.GetService<MeetingEndCommand>())
            .Add(provider.GetService<MeetingRunCommand>())
        );
        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        _ = services.AddSingleton(provier =>
        {
            TypeAdapterConfig.GlobalSettings.Apply([
                new MapperConfiguration(),
                new MeetingEndCommand.MapperConfiguration(provier.GetRequiredService<IQRCodeService>()),
                new MeetingRunCommand.MapperConfiguration(provier.GetRequiredService<IQRCodeService>()),
                new MeetingStartCommand.MapperConfiguration(provier.GetRequiredService<IQRCodeService>()),
                new GetSettingsFunction.MapperConfiguration(),
                new SetSettingsFunction.MapperConfiguration()
            ]);
            return TypeAdapterConfig.GlobalSettings;
        });
        _ = services.AddSingleton<IMapper, ServiceMapper>();
        return services;
    }

}
