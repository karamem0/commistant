//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using AutoMapper;
using Karamem0.Commistant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Mappings;

public class AutoMapperProfile : Profile
{

    public AutoMapperProfile()
    {
        // GetSettings
        _ = this.CreateMap<GetSettingsRequest, GetSettingsResponse>();
        _ = this.CreateMap<CommandSettings, GetSettingsResponse>();
        // SetSettings
        _ = this.CreateMap<SetSettingsRequest, SetSettingsResponse>();
        _ = this.CreateMap<SetSettingsRequest, CommandSettings>();
        _ = this.CreateMap<CommandSettings, SetSettingsResponse>();
    }

}
