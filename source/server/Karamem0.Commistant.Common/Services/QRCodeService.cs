//
// Copyright (c) 2022-2025 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using QRCoder;
using System.Threading;

namespace Karamem0.Commistant.Services;

public interface IQRCodeService
{

    Task<BinaryData> CreateAsync(string text, CancellationToken cancellationToken = default);

}

public class QRCodeService(QRCodeGenerator qrCodeGenerator) : IQRCodeService
{

    private readonly QRCodeGenerator qrCodeGenerator = qrCodeGenerator;

    public Task<BinaryData> CreateAsync(string text, CancellationToken cancellationToken = default)
    {
        var qrCodeData = this.qrCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
        var qrCodePngByte = new PngByteQRCode(qrCodeData);
        return Task.FromResult(BinaryData.FromBytes(qrCodePngByte.GetGraphic(10)));
    }

}
