//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant;

public static class Constants
{

    public const string StartMeetingCommand = "会議開始後";

    public const string EndMeetingCommand = "会議終了前";

    public const string InMeetingCommand = "会議中";

    public const string ResetCommand = "初期化";

    public static readonly ReadOnlyCollection<int> StartMeetingSchedules = Array.AsReadOnly([0, 5, 10, 15]);

    public static readonly ReadOnlyCollection<int> EndMeetingSchedules = Array.AsReadOnly([0, 5, 10, 15]);

    public static readonly ReadOnlyCollection<int> InMeetingSchedules = Array.AsReadOnly([5, 10, 15, 30, 60]);

}
