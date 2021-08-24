// ----------------------------------------------------------------------
// <copyright file="HttpMessageHandlerMock.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SoloX.CodeQuality.Test.Helpers.Http.Impl
{
    public class HttpMessageHandlerMock : HttpMessageHandler
    {
        private readonly Dictionary<string, Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>> responseMap;

        public HttpMessageHandlerMock(Dictionary<string, Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>> responseMap)
        {
            this.responseMap = responseMap;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this.responseMap.TryGetValue(request.Method.Method.ToUpperInvariant(), out var map)
                && map.TryGetValue(request.RequestUri.AbsolutePath, out var responseBuilder))
            {
                return responseBuilder(request);
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));
        }
    }
}
