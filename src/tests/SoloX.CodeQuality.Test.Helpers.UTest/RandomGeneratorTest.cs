// ----------------------------------------------------------------------
// <copyright file="RandomGeneratorTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using Xunit;

namespace SoloX.CodeQuality.Test.Helpers.UTest
{
    public class RandomGeneratorTest
    {
        [Fact]
        public void ItShouldGenerateRandomString()
        {
            var generator = new RandomGenerator();

            var rs = generator.RandomString(10);

            rs.Should().NotBeNull();
            rs.Length.Should().Be(10);
        }
    }
}
