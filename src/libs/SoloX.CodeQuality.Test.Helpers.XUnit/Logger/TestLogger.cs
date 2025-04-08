// ----------------------------------------------------------------------
// <copyright file="TestLogger.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace SoloX.CodeQuality.Test.Helpers.XUnit.Logger
{
    public class TestLogger<T> : ILogger<T>
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TestLogger(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return new TestLoggerScope<T, TState>(this, state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            this.testOutputHelper.WriteLine($"{logLevel}: {formatter(state, exception)}");
        }
    }
}
