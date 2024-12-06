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
        var service = new AdaptiveCardService();
        var arguments = new AdaptiveCardTemplateArguments()
        {
            Schedule = 15,
            Message = "Hello, World!",
            Url = "https://www.example.com"
        };
        var actual = await service.GetCardAsync(AdaptiveCardTemplateNames.EndMeetingBefore, arguments);
        Assert.That(actual, Is.Not.Null);
    }

    [Test()]
    public async Task GetCardAsync_EndMeetingAfter_ReturnsCard()
    {
        var service = new AdaptiveCardService();
        var arguments = new AdaptiveCardTemplateArguments()
        {
            Schedule = 15,
            Message = "Hello, World!",
            Url = "https://www.example.com"
        };
        var actual = await service.GetCardAsync(AdaptiveCardTemplateNames.EndMeetingAfter, arguments);
        Assert.That(actual, Is.Not.Null);
    }

    [Test()]
    public void GetCardAsync_InvalidName_ThrowsException()
    {
        var service = new AdaptiveCardService();
        var arguments = new AdaptiveCardTemplateArguments()
        {
            Schedule = 15,
            Message = "Hello, World!",
            Url = "https://www.example.com"
        };
        _ = Assert.ThrowsAsync<InvalidOperationException>(() => service.GetCardAsync("", arguments));
    }

}
