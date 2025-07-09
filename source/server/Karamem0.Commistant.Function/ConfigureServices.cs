//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AutoMapper;
using Karamem0.Commistant.Commands;
using Karamem0.Commistant.Commands.Abstraction;
using Karamem0.Commistant.Functions;
using Karamem0.Commistant.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant;

public static class ConfigureServices
{

    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        _ = services.AddScoped<AutoMapperProfile>();
        _ = services.AddScoped<GetSettingsFunction.AutoMapperProfile>();
        _ = services.AddScoped<SetSettingsFunction.AutoMapperProfile>();
        _ = services.AddScoped<MeetingEndCommand.AutoMapperProfile>();
        _ = services.AddScoped<MeetingRunCommand.AutoMapperProfile>();
        _ = services.AddScoped<MeetingStartCommand.AutoMapperProfile>();
        _ = services.AddScoped((provider) =>
            {
                var mapperConfig = new MapperConfiguration(config =>
                    {
                        config.AddProfile(provider.GetService<AutoMapperProfile>());
                        config.AddProfile(provider.GetService<GetSettingsFunction.AutoMapperProfile>());
                        config.AddProfile(provider.GetService<SetSettingsFunction.AutoMapperProfile>());
                        config.AddProfile(provider.GetService<MeetingEndCommand.AutoMapperProfile>());
                        config.AddProfile(provider.GetService<MeetingRunCommand.AutoMapperProfile>());
                        config.AddProfile(provider.GetService<MeetingStartCommand.AutoMapperProfile>());
                    }
                );
                return mapperConfig.CreateMapper();
            }
        );
        return services;
    }

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

}
