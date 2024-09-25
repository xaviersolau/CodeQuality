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
    }
}
