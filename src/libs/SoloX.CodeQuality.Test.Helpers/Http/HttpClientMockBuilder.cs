// ----------------------------------------------------------------------
// <copyright file="HttpClientMockBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using SoloX.CodeQuality.Test.Helpers.Http.Impl;
using System;

namespace SoloX.CodeQuality.Test.Helpers.Http
{
    /// <summary>
    /// HttpClient Mock Builder.
    /// </summary>
    public class HttpClientMockBuilder : IHttpClientMockBuilder
    {
        /// <inheritdoc/>
        public IHttpClientRequestMockBuilder WithBaseAddress(Uri baseAddress)
        {
            if (baseAddress is null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            if (!string.IsNullOrEmpty(baseAddress.Query))
            {
                throw new ArgumentException($"{nameof(baseAddress)} should not contain query data");
            }

            if (!baseAddress.AbsolutePath.EndsWith("/", StringComparison.Ordinal))
            {
                baseAddress = new Uri($"{baseAddress}/");
            }

            return new MockBuilder(baseAddress);
        }
    }
}
