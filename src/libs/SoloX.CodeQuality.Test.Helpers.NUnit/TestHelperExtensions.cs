// ----------------------------------------------------------------------
// <copyright file="TestHelperExtensions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoloX.CodeQuality.Test.Helpers.NUnit.Logger;

namespace SoloX.CodeQuality.Test.Helpers.NUnit
{
    public static class TestHelperExtensions
    {
        /// <summary>
        /// Setup the test logging service on the given ServiceCollection.
        /// </summary>
        /// <param name="serviceCollection">The service collection to setup.</param>
        /// <returns>The given service collection.</returns>
        public static IServiceCollection AddTestLogging(
            this IServiceCollection serviceCollection)
        {
            serviceCollection.Add(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(TestLogger<>)));
            return serviceCollection
                .AddSingleton<ILoggerFactory, TestLoggerFactory>()
                .AddSingleton<ILogger, TestLogger<object>>();
        }
    }
}
