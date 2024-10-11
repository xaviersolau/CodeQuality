// ----------------------------------------------------------------------
// <copyright file="PlaywrightTestBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Hosting;
using Microsoft.Playwright;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SoloX.CodeQuality.Playwright
{
    /// <summary>
    /// Builder class to create PlaywrightTest instance.
    /// </summary>
    public static class PlaywrightTestBuilder
    {
        /// <summary>
        /// Create a builder.
        /// </summary>
        /// <returns>The created builder instance.</returns>
        public static IPlaywrightTestBuilder Create()
        {
            return new PlaywrightTestBuilderInternal();
        }

        private sealed class PlaywrightTestBuilderInternal : IPlaywrightTestBuilder, IPlaywrightTracesBuilder, ILocalHostBuilder
        {
            private static readonly PortStore SharedPortStore = new PortStore();

            private Func<string, Action<IWebHostBuilder>, IAsyncDisposable> createTestingWebHostFactoryHandle = CreateTestingWebHostFactory<WebHost.Program>;

            private PortRange portRange = new PortRange(5000, 6000);
            private int goToPageRetryCount = 3;
            private Browser browser;
            private Action<BrowserTypeLaunchOptions> browserTypeLaunchOptionsBuilder = options => { };
            private Action<IWebHostBuilder> webHostBuilderConfiguration = _ => { };

            private bool useLocalHost = true;
            private bool useHttps = true;
            private string onLineHost = string.Empty;

            private Func<string?, string?> traceFilePatternHandler = (f) => null;
            private Action<TracingStartOptions> traceFileOptionsBuilder = options => { };
            private Action<BrowserNewContextOptions> browserNewContextOptionsBuilder = options => { };

            public async Task<IPlaywrightTest> BuildAsync(Browser? browser = null)
            {
                var port = SharedPortStore.GetPort(this.portRange);

                var browserNewContextOptions = new BrowserNewContextOptions { IgnoreHTTPSErrors = true };

                this.browserNewContextOptionsBuilder(browserNewContextOptions);

                var traceFileOptions = new TracingStartOptions()
                {
                    Screenshots = true,
                    Snapshots = true,
                    Sources = true
                };

                this.traceFileOptionsBuilder(traceFileOptions);

                var playwrightDriver = new PlaywrightDriver(this.goToPageRetryCount, browserNewContextOptions, traceFileOptions);

                var browserTypeLaunchOptions = new BrowserTypeLaunchOptions();

                this.browserTypeLaunchOptionsBuilder(browserTypeLaunchOptions);

                await playwrightDriver.InitializeAsync(browserTypeLaunchOptions).ConfigureAwait(false);

                var disposable = (IAsyncDisposable?)null;
                var url = this.onLineHost;

                if (this.useLocalHost)
                {
                    url = PlaywrightDriver.MakeUrl("localhost", isHttps: this.useHttps, port: port);
                    disposable = this.createTestingWebHostFactoryHandle(url, this.webHostBuilderConfiguration);
                }

                var test = new PlaywrightTest(
                    browser ?? this.browser,
                    url,
                    disposable,
                    playwrightDriver,
                    () =>
                    {
                        SharedPortStore.Release(port);
                    },
                    this.traceFilePatternHandler);

                return test;
            }

            public ILocalHostBuilder UseApplication<TEntryPoint>() where TEntryPoint : class
            {
                this.createTestingWebHostFactoryHandle = CreateTestingWebHostFactory<TEntryPoint>;

                return this;
            }

            public ILocalHostBuilder UsePortRange(PortRange portRange)
            {
                this.portRange = portRange;

                return this;
            }

            public ILocalHostBuilder UseHttps(bool useHttps = true)
            {
                this.useHttps = useHttps;

                return this;
            }

            public ILocalHostBuilder UseWebHostBuilder(Action<IWebHostBuilder> configuration)
            {
                this.webHostBuilderConfiguration = configuration;

                return this;
            }

            public IPlaywrightTestBuilder WithBrowser(Browser browser)
            {
                this.browser = browser;

                return this;
            }

            public IPlaywrightTestBuilder WithGoToPageRetry(int retryCount)
            {
                this.goToPageRetryCount = retryCount;

                return this;
            }

            public IPlaywrightTestBuilder WithPlaywrightOptions(Action<BrowserTypeLaunchOptions> configuration)
            {
                this.browserTypeLaunchOptionsBuilder = configuration;

                return this;
            }

            public IPlaywrightTestBuilder WithPlaywrightNewContextOptions(Action<BrowserNewContextOptions> configuration)
            {
                this.browserNewContextOptionsBuilder = configuration;

                return this;
            }

            public IPlaywrightTestBuilder WithTraces(Action<IPlaywrightTracesBuilder>? configuration = null)
            {
                this.traceFilePatternHandler = (f) => string.IsNullOrEmpty(f) ? $"Traces_{Guid.NewGuid()}.zip" : $"Traces_{f}_{Guid.NewGuid()}.zip";

                if (configuration != null)
                {
                    configuration(this);
                }

                return this;
            }

            public IPlaywrightTestBuilder WithLocalHost(Action<ILocalHostBuilder> configuration)
            {
                configuration(this);

                return this;
            }

            public IPlaywrightTestBuilder WithOnLineHost(string onLineHost)
            {
                this.useLocalHost = false;

                this.onLineHost = onLineHost;

                return this;
            }

            public ILocalHostBuilder UseWebHostWithWwwRoot(string wwwRootPath, string? index = null)
            {
                this.createTestingWebHostFactoryHandle = (url, configuration) =>
                {
                    return CreateTestingWebHostFactory<WebHost.Program>(url, builder =>
                    {
                        builder.UseSetting("RootPath", wwwRootPath);

                        if (!string.IsNullOrEmpty(index))
                        {
                            builder.UseSetting("Index", index);
                        }

                        configuration(builder);
                    });
                };

                return this;
            }

            public IPlaywrightTracesBuilder UseTraceOptions(Action<TracingStartOptions> configuration)
            {
                this.traceFileOptionsBuilder = configuration;

                return this;
            }

            public IPlaywrightTracesBuilder UseFilePattern(Func<string?, string?> configuration)
            {
                this.traceFilePatternHandler = configuration;

                return this;
            }

            public IPlaywrightTracesBuilder UseOutputFile(string tracesOutputFile)
            {
                return UseFilePattern(f => tracesOutputFile);
            }

            /// <summary>
            /// Create a typed TestingWebHostFactory.
            /// </summary>
            private static TestingWebHostFactory<TEntryPoint> CreateTestingWebHostFactory<TEntryPoint>(string url, Action<IWebHostBuilder> configuration)
                where TEntryPoint : class
            {
                var hostFactory = new TestingWebHostFactory<TEntryPoint>();

                hostFactory
                    // Override host configuration to configure the url to use.
                    .WithWebHostBuilder(builder =>
                    {
                        builder.UseUrls(url);

                        configuration(builder);
                    })
                    // Create the host using the CreateDefaultClient method.
                    .CreateDefaultClient();

                return hostFactory;
            }

            private sealed class PlaywrightTest : IPlaywrightTest
            {
                private readonly Browser browser;
                private readonly PlaywrightDriver playwrightDriver;
                private readonly string url;
                private readonly IAsyncDisposable? hostFactory;
                private readonly Action disposeCallback;
                private readonly Func<string?, string?> traceFilePatternHandler;

                private bool isDisposed;

                public string Url => this.url;

                internal PlaywrightTest(
                    Browser browser,
                    string url,
                    IAsyncDisposable? hostFactory,
                    PlaywrightDriver playwrightDriver,
                    Action disposeCallback,
                    Func<string?, string?> traceFilePatternHandler)
                {
                    this.browser = browser;
                    this.url = url;
                    this.hostFactory = hostFactory;
                    this.playwrightDriver = playwrightDriver;
                    this.disposeCallback = disposeCallback;
                    this.traceFilePatternHandler = traceFilePatternHandler;
                }

                public Task GotoPageAsync(string relativePath, Func<IPage, Task> testHandler, string? traceName = null, Func<IPage, Task>? pageSetupHandler = null)
                {
                    ObjectDisposedException.ThrowIf(this.isDisposed, this);

                    var traceFileName = traceName ?? GetCallingName();

                    var traceFile = this.traceFilePatternHandler(traceFileName);

                    return this.playwrightDriver.GotoPageAsync(
                        this.url.TrimEnd('/') + "/" + relativePath.TrimStart('/'),
                        testHandler,
                        traceFile: traceFile,
                        pageSetupHandler: pageSetupHandler);
                }

                private static string GetCallingName()
                {
                    var stackTrace = new StackTrace();

                    var idx = 2;

                    var frame = stackTrace.GetFrame(idx);

                    var method = GetOriginalAsyncMethod(frame!.GetMethod()!);

                    var name = $"{method.DeclaringType!.Name}_{method.Name}";

                    return name;
                }

                public static MethodBase GetOriginalAsyncMethod(MethodBase method)
                {
                    // Check if the method is part of a state machine
                    var asyncStateMachineAttribute = method.DeclaringType.GetCustomAttribute<AsyncStateMachineAttribute>();

                    var compilerGeneratedAttribute = method.DeclaringType.GetCustomAttribute<CompilerGeneratedAttribute>();

                    if (asyncStateMachineAttribute != null || compilerGeneratedAttribute != null)
                    {
                        if (method.Name == "MoveNext")
                        {
                            // Get the original type
                            var declaringType = method.DeclaringType.DeclaringType;

                            // The class name will be something like "<OriginalMethod>d__X"
                            var declaringTypeName = method.DeclaringType.Name;

                            // Regex pattern to extract the original method name from "<MethodName>d__X"
                            var match = Regex.Match(declaringTypeName, @"\<(?<methodName>.+)\>d__\d+");

                            if (match.Success && declaringType != null)
                            {
                                // Extracted the original method name from the regex match
                                var originalMethodName = match.Groups["methodName"].Value;

                                // If so, get the original method from the state machine type
                                var originalMethod = declaringType.GetMethod(originalMethodName);

                                return originalMethod ?? method;
                            }
                        }
                    }

                    return method;
                }

                public async ValueTask DisposeAsync()
                {
                    ObjectDisposedException.ThrowIf(this.isDisposed, this);

                    this.isDisposed = true;

                    if (this.hostFactory != null)
                    {
                        await this.hostFactory.DisposeAsync().ConfigureAwait(false);
                    }

                    await this.playwrightDriver.DisposeAsync().ConfigureAwait(false);

                    this.disposeCallback();

                    GC.SuppressFinalize(this);
                }
            }

            private sealed class PortStore
            {
                private readonly HashSet<int> usedPorts = [];

                public int GetPort(PortRange portRange)
                {
#pragma warning disable CA5394 // Do not use insecure randomness
                    var port = Random.Shared.Next(portRange.StartPort, portRange.EndPort);
#pragma warning restore CA5394 // Do not use insecure randomness

                    lock (this.usedPorts)
                    {
                        var systemPort = ProbUsedPorts();

                        var allUsedPorts = new HashSet<int>(systemPort.Union(this.usedPorts));

                        while (allUsedPorts.Contains(port))
                        {
                            port++;
                            if (port >= portRange.EndPort)
                            {
                                port = portRange.StartPort;
                            }
                        }

                        this.usedPorts.Add(port);
                    }
                    return port;
                }

                public void Release(int port)
                {
                    lock (this.usedPorts)
                    {
                        this.usedPorts.Remove(port);
                    }
                }

                public static HashSet<int> ProbUsedPorts()
                {
                    HashSet<int> usedSystemPorts = [];

                    // Get used port with Netstat like command.
                    var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                    var tcpListeners = ipGlobalProperties.GetActiveTcpListeners();

                    foreach (var tcpEndPoint in tcpListeners)
                    {
                        usedSystemPorts.Add(tcpEndPoint.Port);
                    }

                    var tcpConnections = ipGlobalProperties.GetActiveTcpConnections();

                    foreach (var tcpConnection in tcpConnections)
                    {
                        usedSystemPorts.Add(tcpConnection.LocalEndPoint.Port);
                    }

                    return usedSystemPorts;
                }
            }
        }
    }
}
