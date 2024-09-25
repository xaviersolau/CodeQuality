// ----------------------------------------------------------------------
// <copyright file="ProcessHelperTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
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

            exitCode.Should().Be(0);

            stderr.Should().BeEmpty();
            stdout.Should().Contain("Microsoft.");
            stdout.Should().Contain(version);
        }
    }
}
