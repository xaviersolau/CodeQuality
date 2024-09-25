// ----------------------------------------------------------------------
// <copyright file="Browser.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.CodeQuality.Playwright
{
    /// <summary>
    /// Browser types we can use to select the Browser implementation.
    /// </summary>
    public enum Browser
    {
        /// <summary>
        /// Chromium browser implementation.
        /// </summary>
        Chromium,

        /// <summary>
        /// Firefox browser implementation.
        /// </summary>
        Firefox,

        /// <summary>
        /// Webkit browser implementation.
        /// </summary>
        /// <remarks>Webkit doesn't support WebAssembly on Linux.</remarks>
        Webkit,
    }
}
