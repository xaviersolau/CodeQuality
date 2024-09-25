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
        /// <returns>async task.</returns>
        Task GotoPageAsync(string relativePath, Func<IPage, Task> testHandler, string? traceName = null);
    }
}
