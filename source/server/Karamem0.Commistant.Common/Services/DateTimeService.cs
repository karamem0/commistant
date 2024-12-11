//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Services;

public interface IDateTimeService
{

    DateTime GetCurrentDateTime();

}

public class DateTimeService : IDateTimeService
{

    public DateTime GetCurrentDateTime()
    {
        return DateTime.UtcNow;
    }

}
