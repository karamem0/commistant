//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Services.Tests;

[Category("Karamem0.Commistant.Services")]
public class AdaptiveCardServiceTests
{

    [Test()]
    public async Task GetCardAsync_EndMeetingBefore_ReturnsCard()
    {
        // Setup
        var service = new AdaptiveCardService();
        var arguments = new MeetingCardTemplateArguments()
        {
            Schedule = 15,
            Message = "Hello, World!",
            Url = "https://www.example.com"
        };
        // Execute
        var actual = await service.GetCardAsync(AdaptiveCardTemplateTypes.EndMeetingBeforeConfirm, arguments);
        // Assert
        Assert.That(actual, Is.Not.Null);
    }

    [Test()]
    public async Task GetCardAsync_EndMeetingAfter_ReturnsCard()
    {
        // Setup
        var service = new AdaptiveCardService();
        var arguments = new MeetingCardTemplateArguments()
        {
            Schedule = 15,
            Message = "Hello, World!",
            Url = "https://www.example.com"
        };
        // Execute
        var actual = await service.GetCardAsync(AdaptiveCardTemplateTypes.EndMeetingAfterConfirm, arguments);
        // Assert
        Assert.That(actual, Is.Not.Null);
    }

    [Test()]
    public void GetCardAsync_InvalidName_ThrowsException()
    {
        // Setup
        var service = new AdaptiveCardService();
        var arguments = new MeetingCardTemplateArguments()
        {
            Schedule = 15,
            Message = "Hello, World!",
            Url = "https://www.example.com"
        };
        // Execute
        _ = Assert.ThrowsAsync<InvalidOperationException>(() => service.GetCardAsync("", arguments));
    }

}
