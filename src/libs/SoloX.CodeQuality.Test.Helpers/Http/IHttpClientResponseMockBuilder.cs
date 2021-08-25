// ----------------------------------------------------------------------
// <copyright file="IHttpClientResponseMockBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Net;

namespace SoloX.CodeQuality.Test.Helpers.Http
{
    /// <summary>
    /// HttpClient Response mock builder.
    /// </summary>
    public interface IHttpClientResponseMockBuilder
    {
        /// <summary>
        /// Setup a response with the given status.
        /// </summary>
        /// <param name="status">The status to respond.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder RespondingStatus(HttpStatusCode status);

        /// <summary>
        /// Setup a response with the given status and the given content.
        /// </summary>
        /// <param name="content">The response content.</param>
        /// <param name="status">The status to setup in the response.</param>
        /// <returns>The HttpClient request mock builder.</returns>
        IHttpClientRequestMockBuilder RespondingJsonContent<TResponseContent>(TResponseContent content, HttpStatusCode status = HttpStatusCode.OK);
    }
}
