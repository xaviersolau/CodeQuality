// ----------------------------------------------------------------------
// <copyright file="TestingWebHostFactory.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace SoloX.CodeQuality.Playwright
{
    /// <summary>
    /// Testing Web Host Factory to create a InProcess Web Host using the real HTTP network interface.
    /// </summary>
    /// <typeparam name="TEntryPoint">A type in the entry point assembly of the application.
    /// Typically the Startup or Program classes can be used.</typeparam>
    public class TestingWebHostFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint>
        where TEntryPoint : class
    {
        /// <inheritdoc/>
        /// <remarks>Override the CreateHost to build our HTTP host server.</remarks>
        protected override IHost CreateHost(IHostBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            // Create the host that is actually used by the TestServer (In Memory).
            var testHost = base.CreateHost(builder);

            // configure and start the actual host using Kestrel.
            builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());
            var host = builder.Build();
            host.Start();

            // In order to cleanup and properly dispose HTTP server resources we return a composite host object
            // that is actually just a way to way to intercept the StopAsync and Dispose call and relay to our
            // HTTP host.
            return new CompositeHost(testHost, host);
        }

        /// <summary>
        /// Relay the call to both test host and kestrel host.
        /// </summary>
        private sealed class CompositeHost : IHost
        {
            private readonly IHost testHost;
            private readonly IHost kestrelHost;

            public CompositeHost(IHost testHost, IHost kestrelHost)
            {
                this.testHost = testHost;
                this.kestrelHost = kestrelHost;
            }

            public IServiceProvider Services => this.testHost.Services;

            public void Dispose()
            {
                this.testHost.Dispose();

                // Relay the call to kestrel host.
                this.kestrelHost.Dispose();
            }

            public async Task StartAsync(CancellationToken cancellationToken = default)
            {
                await this.testHost.StartAsync(cancellationToken).ConfigureAwait(false);

                // Relay the call to kestrel host.
                await this.kestrelHost.StartAsync(cancellationToken).ConfigureAwait(false);
            }

            public async Task StopAsync(CancellationToken cancellationToken = default)
            {
                await this.testHost.StopAsync(cancellationToken).ConfigureAwait(false);

                // Relay the call to kestrel host.
                await this.kestrelHost.StopAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }
}
