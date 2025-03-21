//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Commands;
using Karamem0.Commistant.Commands.Abstraction;
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
        _ = services.AddSingleton<StartMeetingCommand>();
        _ = services.AddSingleton<EndMeetingCommand>();
        _ = services.AddSingleton<InMeetingCommand>();
        _ = services.AddSingleton(
            (provider) => new CommandSet()
                .Add(provider.GetService<StartMeetingCommand>())
                .Add(provider.GetService<EndMeetingCommand>())
                .Add(provider.GetService<InMeetingCommand>())
        );
        return services;
    }

}
