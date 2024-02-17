//
// Copyright (c) 2022-2024 karamem0
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

namespace Karamem0.Commistant.Mappings
{

    public class AutoMapperProfile : Profile
    {

        private static readonly int[] StartMeetingSchedules = [0, 5, 10, 15];

        private static readonly int[] EndMeetingSchedules = [0, 5, 10, 15];

        private static readonly int[] InMeetingSchedules = [5, 10, 15, 30, 60];

        public AutoMapperProfile()
        {
            _ = this.CreateMap<ConversationPropertyArguments, ConversationProperty>()
                .ForMember(
                    d => d.StartMeetingSchedule,
                    o =>
                    {
                        o.Condition(s => s.Type == "会議開始後");
                        o.MapFrom((s, _) =>
                        {
                            if (s.Value is null)
                            {
                                return -1;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return -1;
                            }
                            if (StartMeetingSchedules.All(_ => _ != s.Value.Schedule))
                            {
                                return -1;
                            }
                            return s.Value.Schedule;
                        });
                    }
                )
                .ForMember(
                    d => d.StartMeetingMessage,
                    o =>
                    {
                        o.Condition(s => s.Type == "会議開始後");
                        o.MapFrom((s, _) =>
                        {
                            if (s.Value is null)
                            {
                                return null;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return null;
                            }
                            if (string.IsNullOrEmpty(s.Value.Message))
                            {
                                return null;
                            }
                            return s.Value.Message;
                        });
                    }
                )
                .ForMember(
                    d => d.StartMeetingUrl,
                    o =>
                    {
                        o.Condition(s => s.Type == "会議開始後");
                        o.MapFrom((s, _) =>
                        {
                            if (s.Value is null)
                            {
                                return null;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return null;
                            }
                            if (string.IsNullOrEmpty(s.Value.Url))
                            {
                                return null;
                            }
                            return s.Value.Url;
                        });
                    }
                )
                .ForMember(
                    d => d.EndMeetingSchedule,
                    o =>
                    {
                        o.Condition(s => s.Type == "会議終了前");
                        o.MapFrom((s, _) =>
                        {
                            if (s.Value is null)
                            {
                                return -1;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return -1;
                            }
                            if (EndMeetingSchedules.All(_ => _ != s.Value.Schedule))
                            {
                                return -1;
                            }
                            return s.Value.Schedule;
                        });
                    }
                )
                .ForMember(
                    d => d.EndMeetingMessage,
                    o =>
                    {
                        o.Condition(s => s.Type == "会議終了前");
                        o.MapFrom((s, _) =>
                        {
                            if (s.Value is null)
                            {
                                return null;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return null;
                            }
                            if (string.IsNullOrEmpty(s.Value.Message))
                            {
                                return null;
                            }
                            return s.Value.Message;
                        });
                    }
                )
                .ForMember(
                    d => d.EndMeetingUrl,
                    o =>
                    {
                        o.Condition(s => s.Type == "会議終了前");
                        o.MapFrom((s, _) =>
                        {
                            if (s.Value is null)
                            {
                                return null;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return null;
                            }
                            if (string.IsNullOrEmpty(s.Value.Url))
                            {
                                return null;
                            }
                            return s.Value.Url;
                        });
                    }
                )
                .ForMember(
                    d => d.InMeetingSchedule,
                    o =>
                    {
                        o.Condition(s => s.Type == "会議中");
                        o.MapFrom((s, _) =>
                        {
                            if (s.Value is null)
                            {
                                return -1;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return -1;
                            }
                            if (InMeetingSchedules.All(_ => _ != s.Value.Schedule))
                            {
                                return -1;
                            }
                            return s.Value.Schedule;
                        });
                    }
                )
                .ForMember(
                    d => d.InMeetingMessage,
                    o =>
                    {
                        o.Condition(s => s.Type == "会議中");
                        o.MapFrom((s, _) =>
                        {
                            if (s.Value is null)
                            {
                                return null;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return null;
                            }
                            if (string.IsNullOrEmpty(s.Value.Message))
                            {
                                return null;
                            }
                            return s.Value.Message;
                        });
                    }
                )
                .ForMember(
                    d => d.InMeetingUrl,
                    o =>
                    {
                        o.Condition(s => s.Type == "会議中");
                        o.MapFrom((s, _) =>
                        {
                            if (s.Value is null)
                            {
                                return null;
                            }
                            if (s.Value.Enabled is false)
                            {
                                return null;
                            }
                            if (string.IsNullOrEmpty(s.Value.Url))
                            {
                                return null;
                            }
                            return s.Value.Url;
                        });
                    }
                );
        }

    }

}
