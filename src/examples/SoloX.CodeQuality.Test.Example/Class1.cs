// ----------------------------------------------------------------------
// <copyright file="Class1.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using Microsoft.Extensions.Logging;

namespace SoloX.CodeQuality.Test.Example
{
    public static class Class1
    {
        private const string TestCase = "trtrt";

        public static int MethodPublic()
        {
            return TestCase.Length;
        }

        public static int MethodPublicWithLogs(ILogger<object> logger, int varInt)
        {
            ArgumentNullException.ThrowIfNull(logger);

            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation($"This is a log {varInt}");
            }

            return TestCase.Length;
        }
    }
}
