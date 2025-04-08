// ----------------------------------------------------------------------
// <copyright file="RequestBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SoloX.CodeQuality.Test.Helpers.Http.Impl
{
    internal class RequestBuilder : IHttpClientResponseFromRequestMockBuilder
    {
        private readonly Func<Func<HttpRequestMessage, Task<HttpResponseMessage>>, IHttpClientRequestMockBuilder> callback;

        public RequestBuilder(Func<Func<HttpRequestMessage, Task<HttpResponseMessage>>, IHttpClientRequestMockBuilder> callback)
        {
            this.callback = callback;
        }

        public IHttpClientRequestMockBuilder Responding(Func<HttpRequestMessage, Task<HttpResponseMessage>> responseHandler)
        {
            return this.callback(responseHandler);
        }

        public IHttpClientRequestMockBuilder Responding(Func<HttpRequestMessage, HttpResponseMessage> responseHandler)
        {
            ArgumentNullException.ThrowIfNull(responseHandler);

            return Responding(request => Task.FromResult(responseHandler(request)));
        }

        public IHttpClientRequestMockBuilder RespondingStatus(HttpStatusCode status)
        {
            return RespondingStatus(r => status);
        }

        public IHttpClientRequestMockBuilder RespondingJsonContent<TResponseContent>(TResponseContent content, HttpStatusCode status = HttpStatusCode.OK)
        {
            return RespondingJsonContent(r => content, status);
        }

        public IHttpClientRequestMockBuilder RespondingStatus(Func<HttpRequestMessage, Task<HttpStatusCode>> responseHandler)
        {
            ArgumentNullException.ThrowIfNull(responseHandler);

            return Responding(async request =>
            {
                var reponseStatus = await responseHandler(request).ConfigureAwait(false);

                return new HttpResponseMessage(reponseStatus);
            });
        }

        public IHttpClientRequestMockBuilder RespondingStatus(Func<HttpRequestMessage, HttpStatusCode> responseHandler)
        {
            ArgumentNullException.ThrowIfNull(responseHandler);

            return RespondingStatus(request => Task.FromResult(responseHandler(request)));
        }

        public IHttpClientRequestMockBuilder RespondingJsonContent<TResponseContent>(Func<HttpRequestMessage, Task<TResponseContent>> responseHandler, HttpStatusCode status = HttpStatusCode.OK)
        {
            ArgumentNullException.ThrowIfNull(responseHandler);

            return Responding(async request =>
            {
                var reponseContent = await responseHandler(request).ConfigureAwait(false);

                var httpResponse = new HttpResponseMessage(status);
                httpResponse.Content = JsonContent.Create(reponseContent);

                return httpResponse;
            });
        }

        public IHttpClientRequestMockBuilder RespondingJsonContent<TResponseContent>(Func<HttpRequestMessage, TResponseContent> responseHandler, HttpStatusCode status = HttpStatusCode.OK)
        {
            ArgumentNullException.ThrowIfNull(responseHandler);

            return RespondingJsonContent(request => Task.FromResult(responseHandler(request)), status);
        }
    }
}
