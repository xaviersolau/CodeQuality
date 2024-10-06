// ----------------------------------------------------------------------
// <copyright file="IPlaywrightTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Playwright;

namespace SoloX.CodeQuality.Playwright
{
    /// <summary>
    /// Playwright test.
    /// </summary>
    public interface IPlaywrightTest : IAsyncDisposable
    {
        /// <summary>
        /// Browse Web host, navigate to the page and process the test handler.
        /// </summary>
        /// <param name="relativePath">Relative URL path.</param>
        /// <param name="testHandler">Test handler.</param>
        /// <param name="traceName">Trace name.</param>
        /// <param name="pageSetupHandler">Page setup handler to initialize page environment before it is display (or null).</param>
        /// <returns>async task.</returns>
        Task GotoPageAsync(string relativePath, Func<IPage, Task> testHandler, string? traceName = null, Func<IPage, Task>? pageSetupHandler = null);

        /// <summary>
        /// Get Url used to bind Playwright and host to test.
        /// </summary>
#pragma warning disable CA1056 // URI-like properties should not be strings
        string Url { get; }
#pragma warning restore CA1056 // URI-like properties should not be strings
    }
}
