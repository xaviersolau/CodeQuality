// ----------------------------------------------------------------------
// <copyright file="ProcessHelperTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Shouldly;
using Xunit;

namespace SoloX.CodeQuality.Test.Helpers.UTest
{
    public class ProcessHelperTest
    {
        [Fact]
        public void IsShouldRunACommandAndGetTheOutput()
        {
            var version = System.Environment.Version.ToString();

            var exitCode = ProcessHelper.Run(".", "dotnet", "--list-runtimes", out var stdout, out var stderr);

            exitCode.ShouldBe(0);

            stderr.ShouldBeEmpty();
            stdout.ShouldContain("Microsoft.");
            stdout.ShouldContain(version);
        }

        [Fact]
        public void IsShouldRunACommandAndGetTheLogs()
        {
            var version = System.Environment.Version.ToString();

            var processResult = ProcessHelper.Run(".", "dotnet", "--list-runtimes");

            processResult.ExitCode.ShouldBe(0);

            processResult.LogMessages.Where(x => x.IsError).ShouldBeEmpty();

            var logs = processResult.GetLogs();
            logs.ShouldContain("Microsoft.");
            logs.ShouldContain(version);
        }
    }
}
