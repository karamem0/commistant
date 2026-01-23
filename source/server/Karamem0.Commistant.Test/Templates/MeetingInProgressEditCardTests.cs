//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using NUnit.Framework;
using System.Text.Json;

namespace Karamem0.Commistant.Templates.Test;

[Category("Karamem0.Commistant.Templates")]
public class MeetingInProgressEditCardTests
{

    [Test()]
    public void Create_Success()
    {
        // Setup
        var rootData = new MeetingInProgressEditCardData()
        {
            Schedule = "5",
            Message = "The meeting is in progress.",
            Url = "https://www.example.com/"
        };
        // Execute
        var actual = MeetingInProgressEditCard.Create(rootData);
        // Assert
        Assert.That(actual, Is.Not.Null);
        using var json = JsonDocument.Parse(actual);
        var root = json.RootElement;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(
                root
                    .GetProperty("body")
                    .EnumerateArray()
                    .ElementAt(1)
                    .GetProperty("value")
                    .GetString(),
                Is.EqualTo("The meeting is in progress.")
            );
            Assert.That(
                root
                    .GetProperty("body")
                    .EnumerateArray()
                    .ElementAt(2)
                    .GetProperty("value")
                    .GetString(),
                Is.EqualTo("https://www.example.com/")
            );
        }
    }

}
