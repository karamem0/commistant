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
public class MeetingStartedNotifyCardTests
{

    [Test()]
    public void Create_Success()
    {
        // Setup
        var rootData = new MeetingStartedNotifyCardData()
        {
            Schedule = "0",
            Message = "The meeting started 5 minutes ago.",
            Url = "https://www.example.com/",
            QrCode = "QRCODEBASE64"
        };
        // Execute
        var actual = MeetingStartedNotifyCard.Create(rootData);
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
                    .GetProperty("text")
                    .GetString(),
                Is.EqualTo("The meeting started 5 minutes ago.")
            );
            Assert.That(
                root
                    .GetProperty("body")
                    .EnumerateArray()
                    .ElementAt(1)
                    .GetProperty("url")
                    .GetString(),
                Is.EqualTo("data:image/png;base64,QRCODEBASE64")
            );
            Assert.That(
                root
                    .GetProperty("actions")
                    .EnumerateArray()
                    .ElementAt(0)
                    .GetProperty("url")
                    .GetString(),
                Is.EqualTo("https://www.example.com/")
            );
        }
    }

}
