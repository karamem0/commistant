//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using NUnit.Framework;

namespace Karamem0.Commistant.Extensions.Test;

[Category("Karamem0.Commistant.Extensions")]
public class DictionaryExtensionTests
{

    [Test()]
    public void GetValueOrDefault_Success_WhenKeyExists()
    {
        // Setup
        var target = new Dictionary<string, object?>()
        {
            ["value"] = 123
        };
        // Execute
        var actual = DictionaryExtension.GetValueOrDefault<int>(target, "value");
        // Assert
        Assert.That(actual, Is.EqualTo(123));
    }

    [Test()]
    public void GetValueOrDefault_Success_WhenKeyDoesNotExist()
    {
        // Setup
        var target = new Dictionary<string, object?>()
        {
            ["value"] = 123
        };
        // Execute
        var actual = DictionaryExtension.GetValueOrDefault<int>(target, "dummy");
        // Assert
        Assert.That(actual, Is.Zero);
    }

}
