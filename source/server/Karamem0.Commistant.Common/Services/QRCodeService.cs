//
// Copyright (c) 2022-2024 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Services;

public interface IQRCodeService
{

    Task<byte[]> CreateAsync(string text, CancellationToken cancellationToken = default);

}

public class QRCodeService(QRCodeGenerator qrCodeGenerator) : IQRCodeService
{

    private readonly QRCodeGenerator qrCodeGenerator = qrCodeGenerator;

    public Task<byte[]> CreateAsync(string text, CancellationToken cancellationToken = default)
    {
        var qrCodeData = this.qrCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var qrCodePngByte = new PngByteQRCode(qrCodeData);
        return Task.FromResult(qrCodePngByte.GetGraphic(10));
    }

}
