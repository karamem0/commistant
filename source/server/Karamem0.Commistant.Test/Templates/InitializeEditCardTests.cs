//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Karamem0.Commistant.Models;
using NUnit.Framework;

namespace Karamem0.Commistant.Templates.Test;

[Category("Karamem0.Commistant.Templates")]
public class InitializeEditCardTests
{

    [Test()]
    public void Create_Success()
    {
        // Setup
        var rootData = new InitializeCardData()
        {
        };
        // Execute
        var actual = InitializeEditCard.Create(rootData);
        // Assert
        Assert.That(actual, Is.Not.Null);
    }

}
