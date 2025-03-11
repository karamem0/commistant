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
            .CreateMap<ConversationPropertyOptions, ConversationProperty>()
            .ForMember(
                d => d.StartMeetingSchedule,
                o =>
                {
                    o.Condition(s => s.Type == Constants.StartMeetingCommand);
                    o.MapFrom(
                        (s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.StartMeetingSchedule;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.StartMeetingSchedule;
                            }
                            if (Constants.StartMeetingSchedules.Any(_ => _ == s.Value.Schedule))
                            {
                                return d.StartMeetingSchedule;
                            }
                            return s.Value.Schedule;
                        }
                    );
                }
            )
            .ForMember(
                d => d.StartMeetingMessage,
                o =>
                {
                    o.Condition(s => s.Type == Constants.StartMeetingCommand);
                    o.MapFrom(
                        (s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.StartMeetingMessage;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.StartMeetingMessage;
                            }
                            if (string.IsNullOrEmpty(s.Value.Message))
                            {
                                return d.StartMeetingMessage;
                            }
                            return s.Value.Message;
                        }
                    );
                }
            )
            .ForMember(
                d => d.StartMeetingUrl,
                o =>
                {
                    o.Condition(s => s.Type == Constants.StartMeetingCommand);
                    o.MapFrom(
                        (s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.StartMeetingUrl;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.StartMeetingUrl;
                            }
                            if (string.IsNullOrEmpty(s.Value.Url))
                            {
                                return d.StartMeetingUrl;
                            }
                            return s.Value.Url;
                        }
                    );
                }
            )
            .ForMember(
                d => d.EndMeetingSchedule,
                o =>
                {
                    o.Condition(s => s.Type == Constants.EndMeetingCommand);
                    o.MapFrom(
                        (s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.EndMeetingSchedule;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.EndMeetingSchedule;
                            }
                            if (Constants.EndMeetingSchedules.Any(_ => _ == s.Value.Schedule))
                            {
                                return d.EndMeetingSchedule;
                            }
                            return s.Value.Schedule;
                        }
                    );
                }
            )
            .ForMember(
                d => d.EndMeetingMessage,
                o =>
                {
                    o.Condition(s => s.Type == Constants.EndMeetingCommand);
                    o.MapFrom(
                        (s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.EndMeetingMessage;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.EndMeetingMessage;
                            }
                            if (string.IsNullOrEmpty(s.Value.Message))
                            {
                                return d.EndMeetingMessage;
                            }
                            return s.Value.Message;
                        }
                    );
                }
            )
            .ForMember(
                d => d.EndMeetingUrl,
                o =>
                {
                    o.Condition(s => s.Type == Constants.EndMeetingCommand);
                    o.MapFrom(
                        (s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.EndMeetingUrl;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.EndMeetingUrl;
                            }
                            if (string.IsNullOrEmpty(s.Value.Url))
                            {
                                return d.EndMeetingUrl;
                            }
                            return s.Value.Url;
                        }
                    );
                }
            )
            .ForMember(
                d => d.InMeetingSchedule,
                o =>
                {
                    o.Condition(s => s.Type == Constants.InMeetingCommand);
                    o.MapFrom(
                        (s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.InMeetingSchedule;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.InMeetingSchedule;
                            }
                            if (Constants.InMeetingSchedules.Any(_ => _ == s.Value.Schedule))
                            {
                                return d.InMeetingSchedule;
                            }
                            return s.Value.Schedule;
                        }
                    );
                }
            )
            .ForMember(
                d => d.InMeetingMessage,
                o =>
                {
                    o.Condition(s => s.Type == Constants.InMeetingCommand);
                    o.MapFrom(
                        (s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.InMeetingMessage;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.InMeetingMessage;
                            }
                            if (string.IsNullOrEmpty(s.Value.Message))
                            {
                                return d.InMeetingMessage;
                            }
                            return s.Value.Message;
                        }
                    );
                }
            )
            .ForMember(
                d => d.InMeetingUrl,
                o =>
                {
                    o.Condition(s => s.Type == Constants.InMeetingCommand);
                    o.MapFrom(
                        (s, d) =>
                        {
                            if (s.Value is null)
                            {
                                return d.InMeetingUrl;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return d.InMeetingUrl;
                            }
                            if (string.IsNullOrEmpty(s.Value.Url))
                            {
                                return d.InMeetingUrl;
                            }
                            return s.Value.Url;
                        }
                    );
                }
            );
    }

}
