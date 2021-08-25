// ----------------------------------------------------------------------
// <copyright file="IHttpClientResponseFromJsonRequestMockBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SoloX.CodeQuality.Test.Helpers.Http
{
    /// <summary>
    /// HttpClient Response mock builder from a request with JSON content.
    /// </summary>
    /// <typeparam name="TRequestContent">Request JSON content type.</typeparam>
    public interface IHttpClientResponseFromJsonRequestMockBuilder<TRequestContent> : IHttpClientResponseMockBuilder
    {
        /// <summary>
        /// Setup a response with the given asynchronous response handler from a request content.
        /// </summary>
        /// <param name="responseHandler">The response handler.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder Responding(Func<TRequestContent, Task<HttpResponseMessage>> responseHandler);

        /// <summary>
        /// Setup a response with the given handler from a request content.
        /// </summary>
        /// <param name="responseHandler">The response handler.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder Responding(Func<TRequestContent, HttpResponseMessage> responseHandler);

        /// <summary>
        /// Setup a response with the given asynchronous response status handler from a request content.
        /// </summary>
        /// <param name="responseHandler">The response status handler.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder RespondingStatus(Func<TRequestContent, Task<HttpStatusCode>> responseHandler);

        /// <summary>
        /// Setup a response with the given response status handler from a request content.
        /// </summary>
        /// <param name="responseHandler">The response status handler.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder RespondingStatus(Func<TRequestContent, HttpStatusCode> responseHandler);

        /// <summary>
        /// Setup a response with the given status and the given asynchronous content handler from a request content.
        /// </summary>
        /// <param name="responseHandler">The response content handler.</param>
        /// <param name="status">The status to setup in the response.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder RespondingJsonContent<TResponseContent>(Func<TRequestContent, Task<TResponseContent>> responseHandler, HttpStatusCode status = HttpStatusCode.OK);

        /// <summary>
        /// Setup a response with the given status and the given content handler from a request content.
        /// </summary>
        /// <param name="responseHandler">The response content handler.</param>
        /// <param name="status">The status to setup in the response.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder RespondingJsonContent<TResponseContent>(Func<TRequestContent, TResponseContent> responseHandler, HttpStatusCode status = HttpStatusCode.OK);
    }
}
