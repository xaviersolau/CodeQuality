// ----------------------------------------------------------------------
// <copyright file="TestHelperExtensions.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoloX.CodeQuality.Test.Helpers.Logger;
using Xunit.Abstractions;

namespace SoloX.CodeQuality.Test.Helpers
{
    public static class TestHelperExtensions
    {
        /// <summary>
        /// Setup the test logging service on the given ServiceCollection.
        /// </summary>
        /// <param name="serviceCollection">The service collection to setup.</param>
        /// <param name="testOutputHelper">The XUnit test output helper.</param>
        /// <returns>The given service collection.</returns>
        public static IServiceCollection AddTestLogging(
            this IServiceCollection serviceCollection,
            ITestOutputHelper testOutputHelper)
        {
            serviceCollection.Add(ServiceDescriptor.Singleton(typeof(ILogger<>), typeof(TestLogger<>)));
            return serviceCollection
                .AddSingleton(testOutputHelper)
                .AddSingleton<ILoggerFactory, TestLoggerFactory>()
                .AddSingleton<ILogger, TestLogger<object>>();
        }
    }
}
