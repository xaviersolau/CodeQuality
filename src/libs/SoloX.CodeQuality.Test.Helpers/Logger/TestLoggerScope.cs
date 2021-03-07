// ----------------------------------------------------------------------
// <copyright file="TestLoggerScope.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.Extensions.Logging;

namespace SoloX.CodeQuality.Test.Helpers.Logger
{
    internal class TestLoggerScope<T, TState> : IDisposable
    {
        private readonly ILogger<T> logger;
        private readonly TState state;

        public TestLoggerScope(ILogger<T> logger, TState state)
        {
            this.logger = logger;
            this.state = state;
            logger.LogInformation($"Enter scope: {state}.");
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.logger.LogInformation($"Exit scope: {this.state}.");
        }
    }
}
