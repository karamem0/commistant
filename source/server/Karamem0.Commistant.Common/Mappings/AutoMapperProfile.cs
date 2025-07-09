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
        _ = this
            .CreateMap<CommandOptions, CommandSettings>()
            .ForMember(
                d => d.MeetingStartSchedule,
                o =>
                {
                    o.Condition(s => s.Type == Constants.MeetingStartCommand);
                    o.MapFrom((s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.MeetingStartSchedule;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.MeetingStartSchedule;
                            }
                            if (Constants.MeetingStartSchedules.Any(_ => _ == s.Value.Schedule))
                            {
                                return d.MeetingStartSchedule;
                            }
                            return s.Value.Schedule;
                        }
                    );
                }
            )
            .ForMember(
                d => d.MeetingStartMessage,
                o =>
                {
                    o.Condition(s => s.Type == Constants.MeetingStartCommand);
                    o.MapFrom((s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.MeetingStartMessage;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.MeetingStartMessage;
                            }
                            if (string.IsNullOrEmpty(s.Value.Message))
                            {
                                return d.MeetingStartMessage;
                            }
                            return s.Value.Message;
                        }
                    );
                }
            )
            .ForMember(
                d => d.MeetingStartUrl,
                o =>
                {
                    o.Condition(s => s.Type == Constants.MeetingStartCommand);
                    o.MapFrom((s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.MeetingStartUrl;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.MeetingStartUrl;
                            }
                            if (string.IsNullOrEmpty(s.Value.Url))
                            {
                                return d.MeetingStartUrl;
                            }
                            return s.Value.Url;
                        }
                    );
                }
            )
            .ForMember(
                d => d.MeetingEndSchedule,
                o =>
                {
                    o.Condition(s => s.Type == Constants.MeetingEndCommand);
                    o.MapFrom((s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.MeetingEndSchedule;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.MeetingEndSchedule;
                            }
                            if (Constants.MeetingEndSchedules.Any(_ => _ == s.Value.Schedule))
                            {
                                return d.MeetingEndSchedule;
                            }
                            return s.Value.Schedule;
                        }
                    );
                }
            )
            .ForMember(
                d => d.MeetingEndMessage,
                o =>
                {
                    o.Condition(s => s.Type == Constants.MeetingEndCommand);
                    o.MapFrom((s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.MeetingEndMessage;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.MeetingEndMessage;
                            }
                            if (string.IsNullOrEmpty(s.Value.Message))
                            {
                                return d.MeetingEndMessage;
                            }
                            return s.Value.Message;
                        }
                    );
                }
            )
            .ForMember(
                d => d.MeetingEndUrl,
                o =>
                {
                    o.Condition(s => s.Type == Constants.MeetingEndCommand);
                    o.MapFrom((s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.MeetingEndUrl;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.MeetingEndUrl;
                            }
                            if (string.IsNullOrEmpty(s.Value.Url))
                            {
                                return d.MeetingEndUrl;
                            }
                            return s.Value.Url;
                        }
                    );
                }
            )
            .ForMember(
                d => d.MeetingRunSchedule,
                o =>
                {
                    o.Condition(s => s.Type == Constants.MeetingRunCommand);
                    o.MapFrom((s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.MeetingRunSchedule;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.MeetingRunSchedule;
                            }
                            if (Constants.MeetingRunSchedules.Any(_ => _ == s.Value.Schedule))
                            {
                                return d.MeetingRunSchedule;
                            }
                            return s.Value.Schedule;
                        }
                    );
                }
            )
            .ForMember(
                d => d.MeetingRunMessage,
                o =>
                {
                    o.Condition(s => s.Type == Constants.MeetingRunCommand);
                    o.MapFrom((s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.MeetingRunMessage;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.MeetingRunMessage;
                            }
                            if (string.IsNullOrEmpty(s.Value.Message))
                            {
                                return d.MeetingRunMessage;
                            }
                            return s.Value.Message;
                        }
                    );
                }
            )
            .ForMember(
                d => d.MeetingRunUrl,
                o =>
                {
                    o.Condition(s => s.Type == Constants.MeetingRunCommand);
                    o.MapFrom((s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.MeetingRunUrl;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.MeetingRunUrl;
                            }
                            if (string.IsNullOrEmpty(s.Value.Url))
                            {
                                return d.MeetingRunUrl;
                            }
                            return s.Value.Url;
                        }
                    );
                }
            );
    }

}
