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
public class MeetingEndingViewCardTests
{

    [Test()]
    public void Create_Success()
    {
        // Setup
        var rootData = new MeetingEndingViewCardData()
        {
            Schedule = "5",
            Message = "The meeting will end in 5 minutes.",
            Url = "https://www.example.com/",
            QrCode = "QRCODEENDING"
        };
        // Execute
        var actual = MeetingEndingViewCard.Create(rootData);
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
                    .ElementAt(0)
                    .GetProperty("columns")
                    .EnumerateArray()
                    .ElementAt(1)
                    .GetProperty("items")
                    .EnumerateArray()
                    .ElementAt(0)
                    .GetProperty("text")
                    .GetString(),
                Is.EqualTo("5")
            );
            Assert.That(
                root
                    .GetProperty("body")
                    .EnumerateArray()
                    .ElementAt(1)
                    .GetProperty("columns")
                    .EnumerateArray()
                    .ElementAt(1)
                    .GetProperty("items")
                    .EnumerateArray()
                    .ElementAt(0)
                    .GetProperty("text")
                    .GetString(),
                Is.EqualTo("The meeting will end in 5 minutes.")
            );
            Assert.That(
                root
                    .GetProperty("body")
                    .EnumerateArray()
                    .ElementAt(2)
                    .GetProperty("columns")
                    .EnumerateArray()
                    .ElementAt(1)
                    .GetProperty("items")
                    .EnumerateArray()
                    .ElementAt(0)
                    .GetProperty("text")
                    .GetString(),
                Is.EqualTo("https://www.example.com/")
            );
            Assert.That(
                root
                    .GetProperty("body")
                    .EnumerateArray()
                    .ElementAt(3)
                    .GetProperty("url")
                    .GetString(),
                Is.EqualTo("data:image/png;base64,QRCODEENDING")
            );
        }
    }

}
