// ----------------------------------------------------------------------
// <copyright file="JsonRequestBuilder.cs" company="Xavier Solau">
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
    internal class JsonRequestBuilder<TRequestContent> : RequestBuilder, IHttpClientResponseFromJsonRequestMockBuilder<TRequestContent>
    {
        public JsonRequestBuilder(Func<Func<HttpRequestMessage, Task<HttpResponseMessage>>, IHttpClientRequestMockBuilder> callback)
            : base(callback)
        {
        }

        public IHttpClientRequestMockBuilder Responding(Func<TRequestContent, Task<HttpResponseMessage>> responseHandler)
        {
            if (responseHandler is null)
            {
                throw new ArgumentNullException(nameof(responseHandler));
            }

            return base.Responding(async request =>
            {
                var content = await request.Content.ReadFromJsonAsync<TRequestContent>().ConfigureAwait(false);
                return await responseHandler(content).ConfigureAwait(false);
            });
        }

        public IHttpClientRequestMockBuilder Responding(Func<TRequestContent, HttpResponseMessage> responseHandler)
        {
            if (responseHandler is null)
            {
                throw new ArgumentNullException(nameof(responseHandler));
            }

            return Responding(request => Task.FromResult(responseHandler(request)));
        }

        public IHttpClientRequestMockBuilder RespondingStatus(Func<TRequestContent, Task<HttpStatusCode>> responseHandler)
        {
            if (responseHandler is null)
            {
                throw new ArgumentNullException(nameof(responseHandler));
            }

            return Responding(async request =>
            {
                var reponseStatus = await responseHandler(request).ConfigureAwait(false);

                return new HttpResponseMessage(reponseStatus);
            });
        }

        public IHttpClientRequestMockBuilder RespondingStatus(Func<TRequestContent, HttpStatusCode> responseHandler)
        {
            if (responseHandler is null)
            {
                throw new ArgumentNullException(nameof(responseHandler));
            }

            return RespondingStatus(request => Task.FromResult(responseHandler(request)));
        }

        public IHttpClientRequestMockBuilder RespondingJsonContent<TResponseContent>(Func<TRequestContent, Task<TResponseContent>> responseHandler, HttpStatusCode status = HttpStatusCode.OK)
        {
            if (responseHandler is null)
            {
                throw new ArgumentNullException(nameof(responseHandler));
            }

            return Responding(async request =>
            {
                var reponseContent = await responseHandler(request).ConfigureAwait(false);

                var httpResponse = new HttpResponseMessage(status);
                httpResponse.Content = JsonContent.Create(reponseContent);

                return httpResponse;
            });
        }

        public IHttpClientRequestMockBuilder RespondingJsonContent<TResponseContent>(Func<TRequestContent, TResponseContent> responseHandler, HttpStatusCode status = HttpStatusCode.OK)
        {
            if (responseHandler is null)
            {
                throw new ArgumentNullException(nameof(responseHandler));
            }

            return RespondingJsonContent(request => Task.FromResult(responseHandler(request)), status);
        }
    }
}
