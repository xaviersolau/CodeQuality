// ----------------------------------------------------------------------
// <copyright file="HttpRequestsMockBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SoloX.CodeQuality.Test.Helpers.Http.Impl
{
    internal class HttpRequestsMockBuilder : IHttpClientRequestMockBuilder
    {
        private readonly Dictionary<string, Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>> responseBuilderMap = new Dictionary<string, Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>>();
        private readonly Uri baseAddress;

        public HttpRequestsMockBuilder(Uri baseAddress)
        {
            this.baseAddress = baseAddress;
        }

        public IHttpClientResponseFromJsonRequestMockBuilder<TRequestContent> WithJsonContentRequest<TRequestContent>(string absolutePath, HttpMethod httpMethod = null)
        {
            return new JsonRequestBuilder<TRequestContent>(responseBuilderAsync => Register(absolutePath, httpMethod, responseBuilderAsync));
        }

        public IHttpClientResponseFromRequestMockBuilder WithRequest(string absolutePath, HttpMethod httpMethod = null)
        {
            return new RequestBuilder(responseBuilderAsync => Register(absolutePath, httpMethod, responseBuilderAsync));
        }

        private IHttpClientRequestMockBuilder Register(string absolutePath, HttpMethod httpMethod, Func<HttpRequestMessage, Task<HttpResponseMessage>> responseBuilderAsync)
        {
            if (absolutePath is null)
            {
                throw new ArgumentNullException(nameof(absolutePath));
            }

            if (httpMethod is null)
            {
                httpMethod = HttpMethod.Get;
            }

            if (!absolutePath.StartsWith("/", StringComparison.Ordinal))
            {
                absolutePath = '/' + absolutePath;
            }

            var httpMethodName = httpMethod.Method.ToUpperInvariant();

            if (responseBuilderAsync is null)
            {
                throw new ArgumentNullException(nameof(responseBuilderAsync));
            }

            if (!this.responseBuilderMap.TryGetValue(httpMethodName, out var map))
            {
                map = new Dictionary<string, Func<HttpRequestMessage, Task<HttpResponseMessage>>>();
                this.responseBuilderMap.Add(httpMethodName, map);
            }

            map.Add(absolutePath, responseBuilderAsync);

            return this;
        }

        public HttpClient Build()
        {
#pragma warning disable CA2000 // Supprimer les objets avant la mise hors de portée
            var httpClient = new HttpClient(new HttpMessageHandlerMock(this.responseBuilderMap), true)
            {
                BaseAddress = this.baseAddress,
            };
#pragma warning restore CA2000 // Supprimer les objets avant la mise hors de portée

            return httpClient;
        }
    }
}
