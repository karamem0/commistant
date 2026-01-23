//
// Copyright (c) 2022-2026 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using NUnit.Framework;
using QRCoder;

namespace Karamem0.Commistant.Services.Tests;

[Category("Karamem0.Commistant.Services")]
public class QRCodeServiceTests
{

    [Test()]
    public async Task CreateAsync_Success()
    {
        // Setup
        var qrCodeGenerator = new QRCodeGenerator();
        // Execute
        var target = new QRCodeService(qrCodeGenerator);
        var actual = target.CreateAsync("Hello, World!");
        // Assert
        Assert.That(actual, Is.Not.Null);
    }

}
