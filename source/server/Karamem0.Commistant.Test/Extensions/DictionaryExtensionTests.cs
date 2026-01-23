//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using NUnit.Framework;
using System.Text.Json;

namespace Karamem0.Commistant.Extensions.Test;

[Category("Karamem0.Commistant.Extensions")]
public class DictionaryExtensionTests
{

    [Test()]
    public void GetValueOrDefault_Success_WhenJsonElement()
    {
        // Setup
        var target = new Dictionary<string, object?>()
        {
            ["value"] = JsonElement.Parse("123")
        };
        // Execute
        var actual = DictionaryExtension.GetValueOrDefault<int>(target, "value");
        // Assert
        Assert.That(actual, Is.EqualTo(123));
    }

    [Test()]
    public void GetValueOrDefault_Success_WhenObject()
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

    [Test()]
    public void GetValueOrDefault_Failure_WhenTypeMismatch()
    {
        // Setup
        var target = new Dictionary<string, object?>()
        {
            ["value"] = 123
        };
        // Execute
        _ = Assert.Throws<InvalidOperationException>(() => DictionaryExtension.GetValueOrDefault<string>(target, "value"));
    }

}
