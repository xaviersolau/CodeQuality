// ----------------------------------------------------------------------
// <copyright file="CA2012RuleTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using NSubstitute;
using Shouldly;

namespace SoloX.CodeQuality.E2ETest
{
    public class CA2012RuleTest
    {
        [Fact]
        public async Task ItShouldCompileWithWarningOnCA2012Async()
        {
            var mock = Substitute.For<ITest>();

            mock.DoSomethingAsync(Arg.Any<int>())
                .Returns(ci =>
                {
                    var arg = ci.Arg<int>();
                    return ValueTask.FromResult(arg);
                });

            var value = 123;
            var result = await mock.DoSomethingAsync(value);

            result.ShouldBe(value);
        }

#pragma warning disable CA1034 // Nested types should not be visible
        public interface ITest
        {
            ValueTask<int> DoSomethingAsync(int arg);
        }
#pragma warning restore CA1034 // Nested types should not be visible
    }
}
