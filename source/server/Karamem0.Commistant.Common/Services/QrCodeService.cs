//
// Copyright (c) 2022 karamem0
//
// This software is released under the MIT License.
//
// https://github.com/karamem0/commistant/blob/main/LICENSE
//

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

        private readonly HttpClient httpClient;

        public QrCodeService(IHttpClientFactory httpClientFactory)
        {
            this.httpClient = httpClientFactory.CreateClient();
        }

        public async Task<byte[]> CreateAsync(string text)
        {
            var requestUrl = $"https://chart.apis.google.com/chart?chs=160x160&cht=qr&chl={Uri.EscapeDataString(text)}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            var responseMessage = await this.httpClient.SendAsync(requestMessage);
            return await responseMessage.Content.ReadAsByteArrayAsync();
        }

    }

}
