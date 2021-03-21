// ----------------------------------------------------------------------
// <copyright file="HttpClientBuilder.cs" company="Xavier Solau">
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

namespace SoloX.CodeQuality.Test.Helpers.Http
{
    /// <summary>
    /// HttpClientMock Builder.
    /// </summary>
    public class HttpClientMockBuilder
    {
        private Dictionary<string, Action<HttpRequestMessage, HttpResponseMessage>> responseMap = new Dictionary<string, Action<HttpRequestMessage, HttpResponseMessage>>();
        private Uri baseAddress;

        /// <summary>
        /// Setup client base address.
        /// </summary>
        /// <param name="baseAddress">Base address to use.</param>
        /// <returns>The current builder.</returns>
        public HttpClientMockBuilder WithBaseAddress(Uri baseAddress)
        {
            this.baseAddress = baseAddress;
            return this;
        }

        /// <summary>
        /// Setup a response builder for the given absolute path request.
        /// </summary>
        /// <param name="absolutePath">The absolute path request to match.</param>
        /// <param name="responseBuilder">The response builder.</param>
        /// <returns>The current builder.</returns>
        public HttpClientMockBuilder WithResponse(string absolutePath, Action<HttpRequestMessage, HttpResponseMessage> responseBuilder)
        {
            this.responseMap.Add(absolutePath, responseBuilder);
            return this;
        }

        /// <summary>
        /// Setup a OK response for the given absolute path request.
        /// </summary>
        /// <param name="absolutePath">The absolute path request to match.</param>
        /// <param name="responseContentBuilder">The response content builder.</param>
        /// <returns>The current builder.</returns>
        public HttpClientMockBuilder WithOkResponse(string absolutePath, Func<HttpRequestMessage, HttpContent> responseContentBuilder)
        {
            return WithResponse(
                absolutePath,
                (request, response) =>
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = responseContentBuilder(request);
                });
        }

        /// <summary>
        /// Create the HttpClient Mock from the current builder setup and reset the builder.
        /// </summary>
        /// <returns>The HttpClient.</returns>
        public HttpClient Build()
        {
#pragma warning disable CA2000 // Supprimer les objets avant la mise hors de portée
            var httpClient = new HttpClient(new HttpMessageHandlerMock(this.responseMap), true)
            {
                BaseAddress = this.baseAddress,
            };
#pragma warning restore CA2000 // Supprimer les objets avant la mise hors de portée

            Reset();
            return httpClient;
        }

        private void Reset()
        {
            this.baseAddress = null;
            this.responseMap = new Dictionary<string, Action<HttpRequestMessage, HttpResponseMessage>>();
        }

        private class HttpMessageHandlerMock : HttpMessageHandler
        {
            private readonly Dictionary<string, Action<HttpRequestMessage, HttpResponseMessage>> responseMap;

            public HttpMessageHandlerMock(Dictionary<string, Action<HttpRequestMessage, HttpResponseMessage>> responseMap)
            {
                this.responseMap = responseMap;
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage();
                if (this.responseMap.TryGetValue(request.RequestUri.AbsolutePath, out var responseBuilder))
                {
                    responseBuilder(request, response);
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                }

                return Task.FromResult(response);
            }
        }
    }
}
