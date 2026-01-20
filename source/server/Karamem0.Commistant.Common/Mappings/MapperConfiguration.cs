//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using Karamem0.Commistant.Types;
using Mapster;

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
                        case CommandTypes.MeetingStarted:
                            if (s.Value?.Enabled is true)
                            {
                                if (Constants.MeetingStartedSchedules.Any(_ => _ == s.Value.Schedule))
                                {
                                    d.MeetingStartedSchedule = s.Value.Schedule;
                                }
                                if (s.Value.Message?.Length > 0)
                                {
                                    d.MeetingStartedMessage = s.Value.Message;
                                }
                                if (s.Value.Url?.Length > 0)
                                {
                                    d.MeetingStartedUrl = s.Value.Url;
                                }
                            }
                            break;
                        case CommandTypes.MeetingEnding:
                            if (s.Value?.Enabled is true)
                            {
                                if (Constants.MeetingEndingSchedules.Any(_ => _ == s.Value.Schedule))
                                {
                                    d.MeetingEndingSchedule = s.Value.Schedule;
                                }
                                if (s.Value.Message?.Length > 0)
                                {
                                    d.MeetingEndingMessage = s.Value.Message;
                                }
                                if (s.Value.Url?.Length > 0)
                                {
                                    d.MeetingEndingUrl = s.Value.Url;
                                }
                            }
                            break;
                        case CommandTypes.MeetingInProgress:
                            if (s.Value?.Enabled is true)
                            {
                                if (Constants.MeetingInProgressSchedules.Any(_ => _ == s.Value.Schedule))
                                {
                                    d.MeetingInProgressSchedule = s.Value.Schedule;
                                }
                                if (s.Value.Message?.Length > 0)
                                {
                                    d.MeetingInProgressMessage = s.Value.Message;
                                }
                                if (s.Value.Url?.Length > 0)
                                {
                                    d.MeetingInProgressUrl = s.Value.Url;
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
