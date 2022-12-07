// ----------------------------------------------------------------------
// <copyright file="LoggerTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using SoloX.CodeQuality.Test.Helpers.NUnit.Logger;

namespace SoloX.CodeQuality.Test.Helpers.NUnit.UTest
{
    public class LoggerTest
    {
        [Test]
        public void IsShouldRegisterNUnitLoggerFactory()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTestLogging();

            using var serviceProvider = serviceCollection.BuildServiceProvider();

            var logger = serviceProvider.GetRequiredService<ILogger<LoggerTest>>();

            logger.LogError("This is an error log message!");

            Assert.Pass();
        }

        [Test]
        public void IsShouldLogThoughTestLogger()
        {
            var logger = new TestLogger<LoggerTest>();

            logger.LogError("This is an error log message!");

            Assert.Pass();
        }
    }
}