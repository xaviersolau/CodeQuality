// ----------------------------------------------------------------------
// <copyright file="IHttpClientRequestMockBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Net.Http;

namespace SoloX.CodeQuality.Test.Helpers.Http
{
    /// <summary>
    /// HttpClient Request mock builder.
    /// </summary>
    public interface IHttpClientRequestMockBuilder
    {
        /// <summary>
        /// Setup a HTTP request to be handled by the mock.
        /// </summary>
        /// <param name="absolutePath">Absolute path to mock.</param>
        /// <param name="httpMethod">HTTP method to mock.</param>
        /// <returns>The response mock builder.</returns>
        IHttpClientResponseFromRequestMockBuilder WithRequest(string absolutePath, HttpMethod? httpMethod = null);

        /// <summary>
        /// Setup a HTTP request with a JSON content to be handled by the mock.
        /// </summary>
        /// <typeparam name="TRequestContent">Type of the JSON data to be sent as request content.</typeparam>
        /// <param name="absolutePath">Absolute path to mock.</param>
        /// <param name="httpMethod">HTTP method to mock.</param>
        /// <returns>The response mock builder.</returns>
        IHttpClientResponseFromJsonRequestMockBuilder<TRequestContent> WithJsonContentRequest<TRequestContent>(string absolutePath, HttpMethod? httpMethod = null);

        /// <summary>
        /// Build the mocked HttpClient.
        /// </summary>
        /// <returns>The mocked HttpClient.</returns>
        HttpClient Build();
    }
}
