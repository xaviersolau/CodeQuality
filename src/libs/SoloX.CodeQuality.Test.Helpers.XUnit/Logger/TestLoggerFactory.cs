﻿// ----------------------------------------------------------------------
// <copyright file="TestLoggerFactory.cs" company="Xavier Solau">
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
    /// <summary>
    /// TestLoggerFactory for use in unit tests (Xunit).
    /// </summary>
    public class TestLoggerFactory : ILoggerFactory
    {
        private readonly ITestOutputHelper testOutputHelper;

        public TestLoggerFactory(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public void AddProvider(ILoggerProvider provider)
        {
            throw new NotImplementedException();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TestLogger<object>(this.testOutputHelper);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            // Nothing to do for now...
        }
    }
}
