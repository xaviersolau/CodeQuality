// ----------------------------------------------------------------------
// <copyright file="IPlaywrightTracesBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Playwright;

namespace SoloX.CodeQuality.Playwright
{
    /// <summary>
    /// Builder to configure traces.
    /// </summary>
    public interface IPlaywrightTracesBuilder
    {
        /// <summary>
        /// Configure TracingStartOptions.
        /// </summary>
        /// <param name="configuration">configuration handler.</param>
        /// <returns>Self.</returns>
        IPlaywrightTracesBuilder UseTraceOptions(Action<TracingStartOptions> configuration);

        /// <summary>
        /// Configure Traces file pattern.
        /// </summary>
        /// <param name="configuration">Traces File Path configuration handler.</param>
        /// <returns></returns>
        IPlaywrightTracesBuilder UseFilePattern(Func<string?, string?> configuration);

        /// <summary>
        /// Configure Traces output file.
        /// </summary>
        /// <param name="tracesOutputFile">Traces Output File Path where to store the traces.</param>
        /// <returns></returns>
        IPlaywrightTracesBuilder UseOutputFile(string tracesOutputFile);
    }
}
