// ----------------------------------------------------------------------
// <copyright file="PlaywrightDriver.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Microsoft.Playwright;

namespace SoloX.CodeQuality.Playwright
{
    /// <summary>
    /// Playwright driver.
    /// </summary>
    public class PlaywrightDriver : IAsyncDisposable
    {
        private static readonly string[] INSTALL_DEPS_ARGUMENTS = new[] { "install-deps" };

        private static readonly string[] INSTALL_ARGUMENTS = new[] { "install" };

        private static readonly object LockSync = new object();
        private static bool playwrightInstalled;

        private BrowserNewContextOptions? browserNewContextOptions;
        private readonly TracingStartOptions? tracingStartOptions;
        private readonly int goToPageRetryCount;

        /// <summary>
        /// Playwright module.
        /// </summary>
        private IPlaywright Playwright { get; set; } = default!;

        /// <summary>
        /// Chromium lazy initializer.
        /// </summary>
        private Lazy<Task<IBrowser>> ChromiumBrowser { get; set; } = default!;
        /// <summary>
        /// Firefox lazy initializer.
        /// </summary>
        private Lazy<Task<IBrowser>> FirefoxBrowser { get; set; } = default!;
        /// <summary>
        /// Webkit lazy initializer.
        /// </summary>
        private Lazy<Task<IBrowser>> WebkitBrowser { get; set; } = default!;

        /// <summary>
        /// Build PlaywrightDriver instance.
        /// </summary>
        /// <param name="goToPageRetryCount">Goto page retry count.</param>
        /// <param name="tracingStartOptions">Tracing start options.</param>
        public PlaywrightDriver(int goToPageRetryCount = 3, TracingStartOptions? tracingStartOptions = null)
        {
            this.goToPageRetryCount = goToPageRetryCount;
            this.tracingStartOptions = tracingStartOptions;
        }

        /// <summary>
        /// Initialize the Playwright module.
        /// </summary>
        /// <returns>The initialization task.</returns>
        public async Task InitializeAsync(BrowserTypeLaunchOptions? browserTypeLaunchOptions = null)
        {
            InstallPlaywright();

            var options = browserTypeLaunchOptions ?? new BrowserTypeLaunchOptions
            {
                //Headless = false,
                //SlowMo = 1000,
                //Timeout = 60000,
            };

            // Create Playwright module.
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync().ConfigureAwait(false);

            // Setup Browser lazy initializers.
            ChromiumBrowser = new Lazy<Task<IBrowser>>(Playwright.Chromium.LaunchAsync(options));
            FirefoxBrowser = new Lazy<Task<IBrowser>>(Playwright.Firefox.LaunchAsync(options));
            WebkitBrowser = new Lazy<Task<IBrowser>>(Playwright.Webkit.LaunchAsync(options));
        }

        /// <summary>
        /// Install and deploy all binaries Playwright may need.
        /// </summary>
        private static void InstallPlaywright()
        {
            lock (LockSync)
            {
                if (playwrightInstalled)
                {
                    return;
                }

                var localApplicationData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PlaywrightDriver");

                if (!Directory.Exists(localApplicationData))
                {
                    Directory.CreateDirectory(localApplicationData);
                }

                var lockFile = Path.Combine(localApplicationData, "PlaywrightInstallLock");

                var timeout = 60;

                while (!TryWriteLockFile(lockFile))
                {
                    Thread.Sleep(1000);
                    timeout--;
                    if (timeout < 0)
                    {
                        throw new PlaywrightException($"Unable to lock for Playwright install (Timeout)");
                    }
                }

                try
                {
                    InstallPlaywrightDeps();
                    InstallPlaywrightBin();
                }
                finally
                {
                    playwrightInstalled = true;
                    File.Delete(lockFile);
                }
            }
        }

        private static bool TryWriteLockFile(string lockFile)
        {
            try
            {
                using var file = new FileStream(lockFile, FileMode.CreateNew, FileAccess.Write);
                file.WriteByte(0);
                file.Flush();
                file.Close();
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        private static void InstallPlaywrightDeps()
        {
            var exitCode = Microsoft.Playwright.Program.Main(INSTALL_DEPS_ARGUMENTS);
            if (exitCode != 0)
            {
                throw new PlaywrightException($"Playwright exited with code {exitCode} on install-deps");
            }
        }

        private static void InstallPlaywrightBin()
        {
            var exitCode = Microsoft.Playwright.Program.Main(INSTALL_ARGUMENTS);
            if (exitCode != 0)
            {
                throw new PlaywrightException($"Playwright exited with code {exitCode} on install");
            }
        }

        /// <summary>
        /// Setup BrowserNewContextOptions for the given device name and/or the given builder.
        /// </summary>
        /// <param name="deviceName">Device name to get the options from.</param>
        /// <param name="browserNewContextOptionsBuilder">BrowserNewContextOptions builder.</param>
        public void SetupBrowserNewContextOptions(string? deviceName = null, Action<BrowserNewContextOptions>? browserNewContextOptionsBuilder = null)
        {
            if (string.IsNullOrEmpty(deviceName) || !Playwright.Devices.TryGetValue(deviceName, out var options))
            {
                options = new BrowserNewContextOptions();
            }

            options.IgnoreHTTPSErrors = true;

            browserNewContextOptionsBuilder?.Invoke(options);

            this.browserNewContextOptions = options;
        }

        /// <summary>
        /// Open a Browser page and navigate to the given URL before applying the given test handler.
        /// </summary>
        /// <param name="url">URL to navigate to.</param>
        /// <param name="testHandler">Test handler to apply on the page.</param>
        /// <param name="browserType">The Browser to use to open the page.</param>
        /// <param name="traceFile">Trace file where to store traces. No trace are generated if file is null.</param>
        /// <param name="pageSetupHandler">Page setup handler to initialize page environment before it is display (or null).</param>
        /// <returns>The GotoPage task.</returns>
#pragma warning disable CA1054 // URI-like parameters should not be strings
        public async Task GotoPageAsync(string url, Func<IPage, Task> testHandler, Browser browserType = Browser.Chromium, string? traceFile = null, Func<IPage, Task>? pageSetupHandler = null)
#pragma warning restore CA1054 // URI-like parameters should not be strings
        {
            ArgumentNullException.ThrowIfNull(testHandler);

            var retry = this.goToPageRetryCount;
            while (retry > 0)
            {
                try
                {
                    await GotoPageInternalAsync(url, testHandler, browserType, traceFile, pageSetupHandler).ConfigureAwait(false);
                    retry = 0;
                }
                catch (PlaywrightException)
                {
                    retry--;

                    if (retry == 0)
                    {
                        throw;
                    }

                    await Task.Delay(1000).ConfigureAwait(false);
                }
            }
        }

        private async Task GotoPageInternalAsync(string url, Func<IPage, Task> testHandler, Browser browserType, string? traceFile, Func<IPage, Task>? pageSetupHandler = null)
        {
            var browser = await SelectBrowserAsync(browserType).ConfigureAwait(false);

            var contextOptions = this.browserNewContextOptions ?? new BrowserNewContextOptions { IgnoreHTTPSErrors = true };

            var context = await browser.NewContextAsync(contextOptions).ConfigureAwait(false);

            await using var _ = context.ConfigureAwait(false);

            if (!string.IsNullOrEmpty(traceFile))
            {
                // Start tracing before creating the page.
                await context.Tracing.StartAsync(this.tracingStartOptions).ConfigureAwait(false);
            }

            var page = await context.NewPageAsync().ConfigureAwait(false);

            page.Should().NotBeNull();

            if (pageSetupHandler != null)
            {
                await pageSetupHandler(page).ConfigureAwait(false);
            }

            try
            {
                var gotoResult = await page.GotoAsync(url, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle, Timeout = 60000 }).ConfigureAwait(false);
                gotoResult.Should().NotBeNull();

                await gotoResult!.FinishedAsync().ConfigureAwait(false);

                gotoResult.Ok.Should().BeTrue();

                await testHandler(page).ConfigureAwait(false);
            }
            finally
            {
                await page.CloseAsync().ConfigureAwait(false);

                if (!string.IsNullOrEmpty(traceFile))
                {
                    // Stop tracing and save data into a zip archive.
                    await context.Tracing.StopAsync(new TracingStopOptions()
                    {
                        Path = traceFile
                    }).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Select the IBrowser instance depending on the given browser enumeration value.
        /// </summary>
        /// <param name="browser">The browser to select.</param>
        /// <returns>The selected IBrowser instance.</returns>
        /// <exception cref="NotImplementedException"></exception>
        private Task<IBrowser> SelectBrowserAsync(Browser browser)
        {
            return browser switch
            {
                Browser.Chromium => ChromiumBrowser.Value,
                Browser.Firefox => FirefoxBrowser.Value,
                Browser.Webkit => WebkitBrowser.Value,
                _ => throw new NotImplementedException()
            };
        }

        /// <summary>
        /// Make URL from host name and port number.
        /// </summary>
        /// <param name="hostName">The host name.</param>
        /// <param name="isHttps">Tells if we use HTTPS.</param>
        /// <param name="port">Port number to use.</param>
        /// <returns>The configured URL.</returns>
#pragma warning disable CA1055 // URI-like return values should not be strings
        public static string MakeUrl(string hostName, bool isHttps = true, int? port = null)
#pragma warning restore CA1055 // URI-like return values should not be strings
        {
            if (!port.HasValue)
            {
                port = 5000;
            }

            var http = isHttps ? "https" : "http";
            return $"{http}://{hostName}:{port}";
        }

        /// <summary>
        /// Dispose all Playwright module resources.
        /// </summary>
        /// <returns>The disposal task.</returns>
        public async ValueTask DisposeAsync()
        {
            if (Playwright != null)
            {
                if (ChromiumBrowser != null && ChromiumBrowser.IsValueCreated)
                {
                    var browser = await ChromiumBrowser.Value.ConfigureAwait(false);
                    await browser.DisposeAsync().ConfigureAwait(false);
                }
                if (FirefoxBrowser != null && FirefoxBrowser.IsValueCreated)
                {
                    var browser = await FirefoxBrowser.Value.ConfigureAwait(false);
                    await browser.DisposeAsync().ConfigureAwait(false);
                }
                if (WebkitBrowser != null && WebkitBrowser.IsValueCreated)
                {
                    var browser = await WebkitBrowser.Value.ConfigureAwait(false);
                    await browser.DisposeAsync().ConfigureAwait(false);
                }

                Playwright.Dispose();
                Playwright = default!;
            }

            GC.SuppressFinalize(this);
        }
    }
}
