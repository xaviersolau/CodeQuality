// ----------------------------------------------------------------------
// <copyright file="RandomGenerator.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Text;

namespace SoloX.CodeQuality.Test.Helpers
{
    /// <summary>
    /// RandomGenerator class to generate random values for test purpose.
    /// </summary>
    public class RandomGenerator
    {
        private readonly Random random = new Random();
        private const int LettersCount = 26;
        private const int NumbersCount = 10;

#pragma warning disable CA5394 // Do not use insecure randomness
        /// <summary>
        /// Get a random number.
        /// </summary>
        /// <param name="min">Min value (inclusive)</param>
        /// <param name="max">Max value (exclusive)</param>
        /// <returns></returns>
        public int RandomNumber(int min, int max)
        {
            return this.random.Next(min, max);
        }

        /// <summary>
        /// Get a random string with the given size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public string RandomString(int size)
        {
            var builder = new StringBuilder(size);

            for (var i = 0; i < size; i++)
            {
                // Get an int in [0, 1, 2] to select a letter or a number.
                var selector = this.random.Next(0, 3);

                if (selector == 2)
                {
                    var offset = '0';

                    var randomNumber = (char)this.random.Next(offset, offset + NumbersCount);
                    builder.Append(randomNumber);
                }
                else
                {
                    var offset = selector == 0 ? 'a' : 'A';

                    var randomChar = (char)this.random.Next(offset, offset + LettersCount);
                    builder.Append(randomChar);
                }
            }

            return builder.ToString();
        }
#pragma warning restore CA5394 // Do not use insecure randomness
    }
}
