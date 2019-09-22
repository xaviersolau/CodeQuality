// ----------------------------------------------------------------------
// <copyright file="TestLoggerFactory.cs" company="SoloX Software">
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
    /// <summary>
    /// TestLoggerFactory for use in unit tests (Xunit).
    /// </summary>
    public class TestLoggerFactory : ILoggerFactory
    {
        private ITestOutputHelper testOutputHelper;

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
