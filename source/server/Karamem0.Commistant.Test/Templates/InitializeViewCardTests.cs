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
using System.Text.Json.Nodes;

namespace Karamem0.Commistant.Templates.Test;

[Category("Karamem0.Commistant.Templates")]
public class InitializeViewCardTests
{

    [Test()]
    public void Create_Success()
    {
        // Setup
        var rootData = new InitializeViewCardData()
        {
            Value = "Yes"
        };
        // Execute
        var actual = InitializeViewCard.Create(rootData);
        // Assert
        Assert.That(actual, Is.Not.Null);
        using var json = JsonDocument.Parse(actual);
        var root = json.RootElement;
        Assert.That(
            root
                .GetProperty("body")
                .EnumerateArray()
                .ElementAt(0)
                .GetProperty("facts")
                .EnumerateArray()
                .ElementAt(0)
                .GetProperty("value")
                .GetString(),
            Is.EqualTo("Yes")
        );
    }

}
