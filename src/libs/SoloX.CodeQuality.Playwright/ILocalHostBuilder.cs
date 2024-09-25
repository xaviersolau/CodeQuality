// ----------------------------------------------------------------------
// <copyright file="ILocalHostBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.AspNetCore.Hosting;

namespace SoloX.CodeQuality.Playwright
{
    /// <summary>
    /// Builder to configure local host.
    /// </summary>
    public interface ILocalHostBuilder
    {
        /// <summary>
        /// Specify the application to run in the local host.
        /// </summary>
        /// <typeparam name="TProgram">A type defined in the application entry point assembly. It can be any type declared in the application assembly like Program or Startup.</typeparam>
        /// <returns>Self.</returns>
        ILocalHostBuilder UseApplication<TProgram>() where TProgram : class;

        /// <summary>
        /// Specify the port range available to set up the HTTP network interface.
        /// </summary>
        /// <param name="portRange">Port range where to randomly select the host listening port.</param>
        /// <returns>Self.</returns>
        ILocalHostBuilder UsePortRange(PortRange portRange);

        /// <summary>
        /// Specify use of HTTPS protocol.
        /// </summary>
        /// <param name="useHttps">True to use it.</param>
        /// <returns>Self.</returns>
        ILocalHostBuilder UseHttps(bool useHttps = true);

        /// <summary>
        /// Builder to set up the Web Host.
        /// </summary>
        /// <remarks>
        /// Used to override and/or mock some services.
        /// </remarks>
        /// <param name="configuration">configuration handler.</param>
        /// <returns>Self.</returns>
        ILocalHostBuilder UseWebHostBuilder(Action<IWebHostBuilder> configuration);

        /// <summary>
        /// Web host with static assets only.
        /// If you don't have an application to start in the local host but only static assets, you can
        /// specify where to locate the files.
        /// </summary>
        /// <param name="wwwRootPath">Folder where to load the static asserts from.</param>
        /// <param name="index">Default index file.</param>
        /// <returns>Self.</returns>
        ILocalHostBuilder UseWebHostWithWwwRoot(string wwwRootPath, string? index = null);
    }
}
