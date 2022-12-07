// ----------------------------------------------------------------------
// <copyright file="LoggerTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SoloX.CodeQuality.Test.Helpers.XUnit.Logger;
using Xunit;
using Xunit.Abstractions;

namespace SoloX.CodeQuality.Test.Helpers.XUnit.UTest
{
    public class LoggerTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public LoggerTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void IsShouldRegisterXUnitLoggerFactory()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTestLogging(this.testOutputHelper);

            using var serviceProvider = serviceCollection.BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<LoggerTest>>();

            logger.LogError("This is an error log message!");

            Assert.True(true);
        }

        [Fact]
        public void IsShouldLogThoughTestLogger()
        {
            var logger = new TestLogger<LoggerTest>(this.testOutputHelper);

            logger.LogError("This is an error log message!");

            Assert.True(true);
        }
    }
}