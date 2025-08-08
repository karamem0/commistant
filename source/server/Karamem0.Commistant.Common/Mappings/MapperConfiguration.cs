//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Mappings;

public class MapperConfiguration : IRegister
{

    public void Register(TypeAdapterConfig config)
    {
        _ = config
            .NewConfig<CommandOptions, CommandSettings>()
            .AfterMapping((s, d) =>
                {
                    switch (s.Type)
                    {
                        case Constants.MeetingStartCommand:
                            if (s.Value.Enabled is true)
                            {
                                if (Constants.MeetingStartSchedules.Any(_ => _ == s.Value.Schedule))
                                {
                                    d.MeetingStartSchedule = s.Value.Schedule;
                                }
                                if (s.Value.Message?.Length > 0)
                                {
                                    d.MeetingStartMessage = s.Value.Message;
                                }
                                if (s.Value.Url?.Length > 0)
                                {
                                    d.MeetingStartUrl = s.Value.Url;
                                }
                            }
                            break;
                        case Constants.MeetingEndCommand:
                            if (s.Value.Enabled is true)
                            {
                                if (Constants.MeetingEndSchedules.Any(_ => _ == s.Value.Schedule))
                                {
                                    d.MeetingEndSchedule = s.Value.Schedule;
                                }
                                if (s.Value.Message?.Length > 0)
                                {
                                    d.MeetingEndMessage = s.Value.Message;
                                }
                                if (s.Value.Url?.Length > 0)
                                {
                                    d.MeetingEndUrl = s.Value.Url;
                                }
                            }
                            break;
                        case Constants.MeetingRunCommand:
                            if (s.Value.Enabled is true)
                            {
                                if (Constants.MeetingRunSchedules.Any(_ => _ == s.Value.Schedule))
                                {
                                    d.MeetingRunSchedule = s.Value.Schedule;
                                }
                                if (s.Value.Message?.Length > 0)
                                {
                                    d.MeetingRunMessage = s.Value.Message;
                                }
                                if (s.Value.Url?.Length > 0)
                                {
                                    d.MeetingRunUrl = s.Value.Url;
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            );
    }

}
