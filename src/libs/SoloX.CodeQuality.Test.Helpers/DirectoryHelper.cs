// ----------------------------------------------------------------------
// <copyright file="DirectoryHelper.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;
using System.Runtime.CompilerServices;

namespace SoloX.CodeQuality.Test.Helpers
{
    /// <summary>
    /// Provides helper methods for deriving directory names from test assembly locations or test file locations.
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        /// Gets the name of the current build configuration (parent directory of the assembly that contains TTestClass).
        /// </summary>
        /// <typeparam name="TTestClass">The type whose containing assembly is used to determine the current configuration
        /// (parent directory name from the assembly path ProjectName/bin/Debug/net10.0/ProjectAssembly.dll).</typeparam>
        /// <returns>The file-system name of the parent directory of the assembly directory for TTestClass.</returns>
        public static string ProbConfiguration<TTestClass>()
        {
            var location = Path.GetDirectoryName(typeof(TTestClass).Assembly.Location);

            return Path.GetFileName(Path.GetDirectoryName(location)!);
        }

        /// <summary>
        /// Gets the directory portion of the caller's source file path.
        /// </summary>
        /// <remarks>Extracted from the compile-time caller file path provided by CallerFilePath using
        /// Path.GetDirectoryName.</remarks>
        /// <param name="location">The source file path of the caller, supplied by the CallerFilePath attribute.</param>
        /// <returns>The directory path that contains the caller's source file.</returns>
        public static string ThisFilePathLocation([CallerFilePath] string location = default!)
        {
            return Path.GetDirectoryName(location)!;
        }
    }
}
