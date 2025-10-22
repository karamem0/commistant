//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Karamem0.Commistant.Extensions.Test;

[Category("Karamem0.Commistant.Extensions")]
public class JObjectExtensionTests
{

    [Test()]
    public void Value_Success_WhenKeyExists()
    {
        // Setup
        var target = JObject.FromObject(
            new Dictionary<string, object>()
            {
                ["value"] = 123
            }
        );
        // Execute
        var actual = JObjectExtension.Value(
            target,
            "value",
            456
        );
        // Assert
        Assert.That(actual, Is.EqualTo(123));
    }

    [Test()]
    public void Value_Success_WhenKeyDoesNotExist()
    {
        // Setup
        var target = JObject.FromObject(
            new Dictionary<string, object>()
            {
                ["value"] = 123
            }
        );
        // Execute
        var actual = JObjectExtension.Value(
            target,
            "dummy",
            456
        );
        // Assert
        Assert.That(actual, Is.EqualTo(456));
    }

}
