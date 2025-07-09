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

    public const string MeetingStartCommand = "会議開始後";

    public const string MeetingEndCommand = "会議終了前";

    public const string MeetingRunCommand = "会議中";

    public const string ResetCommand = "初期化";

    public const string SubmitButton = "Submit";

    public const string CancelButton = "Cancel";

    public const string YesButton = "Yes";

    public const string NoButton = "No";

    public static readonly ReadOnlyCollection<int> MeetingStartSchedules = Array.AsReadOnly([0, 5, 10, 15]);

    public static readonly ReadOnlyCollection<int> MeetingEndSchedules = Array.AsReadOnly([0, 5, 10, 15]);

    public static readonly ReadOnlyCollection<int> MeetingRunSchedules = Array.AsReadOnly([5, 10, 15, 30, 60]);

}
