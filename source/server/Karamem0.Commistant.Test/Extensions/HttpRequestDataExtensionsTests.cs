//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Extensions.Tests;

[Category("Karamem0.Commistant.Extensions")]
public class HttpRequestDataExtensionsTests
{

    [Test()]
    public void GetUserId_Success_WhenHeaderFound()
    {
        // Setup
        var functionContext = Substitute.For<FunctionContext>();
        var requestData = Substitute.For<HttpRequestData>(functionContext);
        _ = requestData.Headers.Returns(
            new HttpHeadersCollection(
                [
                    new KeyValuePair<string, string>("X-MS-CLIENT-PRINCIPAL-ID", "48d31887-5fad-4d73-a9f5-3c356e68a038")
                ]
            )
        );
        // Execute
        var actual = HttpRequestDataExtensions.GetUserId(requestData);
        // Validate
        Assert.That(actual, Is.EqualTo("48d31887-5fad-4d73-a9f5-3c356e68a038"));
    }

    [Test()]
    public void GetUserId_Success_WhenHeaderNotFound()
    {
        // Setup
        var functionContext = Substitute.For<FunctionContext>();
        var requestData = Substitute.For<HttpRequestData>(functionContext);
        _ = requestData.Headers.Returns([]);
        // Execute
        var actual = HttpRequestDataExtensions.GetUserId(requestData);
        // Validate
        Assert.That(actual, Is.Null);
    }

    [Test()]
    public void GetUserId_Failure_WhenHeaderValueDuplicated()
    {
        // Setup
        var functionContext = Substitute.For<FunctionContext>();
        var requestData = Substitute.For<HttpRequestData>(functionContext);
        _ = requestData.Headers.Returns(
            new HttpHeadersCollection(
                [
                    new KeyValuePair<string, string>("X-MS-CLIENT-PRINCIPAL-ID", "48d31887-5fad-4d73-a9f5-3c356e68a038"),
                    new KeyValuePair<string, string>("X-MS-CLIENT-PRINCIPAL-ID", "b3927dec-0f32-4828-a643-b45efe89ce44")
                ]
            )
        );
        // Execute
        var actual = Assert.Throws<InvalidOperationException>(() => HttpRequestDataExtensions.GetUserId(requestData));
        // Validate
        Assert.That(actual, Is.Not.Null);
    }

}
