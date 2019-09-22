﻿// ----------------------------------------------------------------------
// <copyright file="TestLogger.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace SoloX.CodeQuality.Test.Helpers.Logger
{
    public class TestLogger<T> : ILogger<T>
    {
        private ITestOutputHelper testOutputHelper;

        public TestLogger(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public IDisposable BeginScope<TState>(TState state)
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
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            this.testOutputHelper.WriteLine($"{logLevel}: {formatter(state, exception)}");
        }
    }
}