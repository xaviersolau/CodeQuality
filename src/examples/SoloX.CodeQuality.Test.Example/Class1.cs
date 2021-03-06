﻿// ----------------------------------------------------------------------
// <copyright file="Class1.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.CodeQuality.Test.Example
{
    public static class Class1
    {
        private const string TestCase = "trtrt";

        public static int MethodPublic()
        {
            return TestCase.Length;
        }
    }
}
