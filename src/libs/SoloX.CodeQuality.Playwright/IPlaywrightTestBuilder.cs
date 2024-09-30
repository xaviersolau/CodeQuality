// ----------------------------------------------------------------------
// <copyright file="IPlaywrightTestBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Playwright;

namespace SoloX.CodeQuality.Playwright
{
    /// <summary>
    /// Playwright test builder.
    /// Set up Web host and Playwright infrastructure.
    /// </summary>
    public interface IPlaywrightTestBuilder
    {
        /// <summary>
        /// Specify the Browser implementation to use. It may be overridden by BuildAsync method.
        /// </summary>
        /// <param name="browser">Browser implementation to use.</param>
        /// <remarks>Can be override in the BuildAsync method.</remarks>
        /// <returns>Self.</returns>
        IPlaywrightTestBuilder WithBrowser(Browser browser);

        /// <summary>
        /// Setup the local host.
        /// </summary>
        /// <param name="configuration">Local host configuration.</param>
        /// <returns>Self.</returns>
        IPlaywrightTestBuilder WithLocalHost(Action<ILocalHostBuilder> configuration);

        /// <summary>
        /// Setup the on-line web host.
        /// </summary>
        /// <param name="onLineHost">On-line Web host url.</param>
        /// <returns>Self.</returns>
        IPlaywrightTestBuilder WithOnLineHost(string onLineHost);

        /// <summary>
        /// Specify number of retry to apply on GoToPage method.
        /// </summary>
        /// <param name="retryCount">Retry count.</param>
        /// <returns>Self.</returns>
        IPlaywrightTestBuilder WithGoToPageRetry(int retryCount);

        /// <summary>
        /// Specify Playwright options to use.
        /// </summary>
        /// <param name="configuration">Playwright options configuration.</param>
        /// <returns>Self.</returns>
        IPlaywrightTestBuilder WithPlaywrightOptions(Action<BrowserTypeLaunchOptions> configuration);

        /// <summary>
        /// Specify Playwright New Context options to use.
        /// </summary>
        /// <param name="configuration">Playwright new context options configuration.</param>
        /// <returns>Self.</returns>
        IPlaywrightTestBuilder WithPlaywrightNewContextOptions(Action<BrowserNewContextOptions> configuration);

        /// <summary>
        /// Specify how the playwright traces are generated. 
        /// </summary>
        /// <param name="configuration">Traces configuration.</param>
        /// <returns>Self.</returns>
        IPlaywrightTestBuilder WithTraces(Action<IPlaywrightTracesBuilder>? configuration = null);

        /// <summary>
        /// Build and set up Web host and Playwright test driver.
        /// </summary>
        /// <param name="browser">Browser override (optional).</param>
        /// <returns>The Playwright test.</returns>
        Task<IPlaywrightTest> BuildAsync(Browser? browser = null);
    }
}
