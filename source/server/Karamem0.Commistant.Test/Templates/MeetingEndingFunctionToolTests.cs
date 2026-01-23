//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using NUnit.Framework;

namespace Karamem0.Commistant.Templates.Test;

[Category("Karamem0.Commistant.Templates")]
public class MeetingEndingFunctionToolTests
{

    [Test()]
    public void Create_Success()
    {
        // Execute
        var actual = MeetingEndingFunctionTool.Create();
        // Assert
        Assert.That(actual, Is.Not.Null);
    }

}
