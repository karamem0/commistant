//
// Copyright (c) 2023 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Karamem0.Commistant.Services
{

    public class QrCodeService
    {

        private readonly QRCodeGenerator qrCodeGenerator;

        public QrCodeService()
        {
            this.qrCodeGenerator = new QRCodeGenerator();
        }

        public Task<byte[]> CreateAsync(string text)
        {
            return Task.FromResult(
                new PngByteQRCode(this.qrCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
                    .GetGraphic(10));
        }

    }

}
