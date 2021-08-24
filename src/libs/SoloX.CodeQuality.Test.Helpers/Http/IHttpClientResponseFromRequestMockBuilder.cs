// ----------------------------------------------------------------------
// <copyright file="IHttpClientResponseFromRequestMockBuilder.cs" company="Xavier Solau">
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
    /// HttpClient Response mock builder from a HTTP request message.
    /// </summary>
    public interface IHttpClientResponseFromRequestMockBuilder
    {
        /// <summary>
        /// Setup a response with the given asynchronous response handler from a given HttpRequestMessage
        /// </summary>
        /// <param name="responseHandler">The response handler.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder Responding(Func<HttpRequestMessage, Task<HttpResponseMessage>> responseHandler);

        /// <summary>
        /// Setup a response with the given response handler from a given HttpRequestMessage
        /// </summary>
        /// <param name="responseHandler">The response handler.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder Responding(Func<HttpRequestMessage, HttpResponseMessage> responseHandler);

        /// <summary>
        /// Setup a response with the given status.
        /// </summary>
        /// <param name="status">The status to respond.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder RespondingStatus(HttpStatusCode status);

        /// <summary>
        /// Setup a response with the given asynchronous response status handler from a given HttpRequestMessage.
        /// </summary>
        /// <param name="responseHandler">The response status handler.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder RespondingStatus(Func<HttpRequestMessage, Task<HttpStatusCode>> responseHandler);

        /// <summary>
        /// Setup a response with the given response status handler from a given HttpRequestMessage.
        /// </summary>
        /// <param name="responseHandler">The response status handler.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder RespondingStatus(Func<HttpRequestMessage, HttpStatusCode> responseHandler);

        /// <summary>
        /// Setup a response with the given status and the given asynchronous content handler from a given HttpRequestMessage.
        /// </summary>
        /// <param name="responseHandler">The response status handler.</param>
        /// <param name="status">The status to setup in the response.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder RespondingJsonContent<TResponseContent>(Func<HttpRequestMessage, Task<TResponseContent>> responseHandler, HttpStatusCode status = HttpStatusCode.OK);

        /// <summary>
        /// Setup a response with the given status and the given content handler from a given HttpRequestMessage.
        /// </summary>
        /// <param name="responseHandler">The response status handler.</param>
        /// <param name="status">The status to setup in the response.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder RespondingJsonContent<TResponseContent>(Func<HttpRequestMessage, TResponseContent> responseHandler, HttpStatusCode status = HttpStatusCode.OK);
    }
}
