//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System.Collections.ObjectModel;

namespace Karamem0.Commistant;

public static class Constants
{

    public static readonly ReadOnlyCollection<int> MeetingStartedSchedules = Array.AsReadOnly([0, 5, 10, 15]);

    public static readonly ReadOnlyCollection<int> MeetingEndingSchedules = Array.AsReadOnly([0, 5, 10, 15]);

    public static readonly ReadOnlyCollection<int> MeetingInProgressSchedules = Array.AsReadOnly([5, 10, 15, 30, 60]);

}
