// ----------------------------------------------------------------------
// <copyright file="TestLoggerScope.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace SoloX.CodeQuality.Test.Helpers.Logger
{
    internal class TestLoggerScope<T, TState> : IDisposable
    {
        private ILogger<T> logger;
        private TState state;

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
