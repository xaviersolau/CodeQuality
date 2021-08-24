// ----------------------------------------------------------------------
// <copyright file="IHttpClientMockBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.CodeQuality.Test.Helpers.Http
{
    /// <summary>
    /// Base interface of the HttpClient Mock Builder.
    /// </summary>
    public interface IHttpClientMockBuilder
    {
        /// <summary>
        /// Specify a base address for the mocked HttpClient.
        /// </summary>
        /// <param name="baseAddress">HTTP client base address.</param>
        /// <returns></returns>
        IHttpClientRequestMockBuilder WithBaseAddress(Uri baseAddress);
    }
}
