//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Extensions.Tests;

[Category("Karamem0.Commistant.Extensions")]
public class HttpRequestExtensionsTests
{

    [Test()]
    public void GetUserId_Success_WhenHeaderFound()
    {
        // Setup
        var request = Substitute.For<HttpRequest>();
        _ = request.Headers.Returns(
            new HeaderDictionary()
            {
                ["X-MS-CLIENT-PRINCIPAL-ID"] = "48d31887-5fad-4d73-a9f5-3c356e68a038"
            }
        );
        // Execute
        var actual = request.GetUserId();
        // Validate
        Assert.That(actual, Is.EqualTo("48d31887-5fad-4d73-a9f5-3c356e68a038"));
    }

    [Test()]
    public void GetUserId_Success_WhenHeaderNotFound()
    {
        // Setup
        var request = Substitute.For<HttpRequest>();
        _ = request.Headers.Returns(new HeaderDictionary());
        // Execute
        var actual = request.GetUserId();
        // Validate
        Assert.That(actual, Is.Null);
    }

    [Test()]
    public void GetUserId_Failure_WhenHeaderValueDuplicated()
    {
        // Setup
        var request = Substitute.For<HttpRequest>();
        _ = request.Headers.Returns(
            new HeaderDictionary()
            {
                ["X-MS-CLIENT-PRINCIPAL-ID"] = new StringValues(
                    [
                        "48d31887-5fad-4d73-a9f5-3c356e68a038",
                        "b3927dec-0f32-4828-a643-b45efe89ce44"
                    ]
                )
            }
        );
        // Execute
        _ = Assert.Throws<InvalidOperationException>(() => request.GetUserId());
    }

}
