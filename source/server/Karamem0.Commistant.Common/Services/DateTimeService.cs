//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

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
