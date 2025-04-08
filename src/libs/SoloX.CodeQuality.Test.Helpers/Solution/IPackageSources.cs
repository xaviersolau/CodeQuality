// ----------------------------------------------------------------------
// <copyright file="IPackageSources.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.CodeQuality.Test.Helpers.Solution
{
    /// <summary>
    /// IPackageSources is the interface to configure Nuget package sources.
    /// </summary>
    public interface IPackageSources
    {
        /// <summary>
        /// Clear the sources.
        /// </summary>
        /// <returns>Self.</returns>
        IPackageSources Clear();

        /// <summary>
        /// Add source folder.
        /// </summary>
        /// <param name="path">Folder path where to load the packages from.</param>
        /// <remarks>Relative path are based on the SolutionBuilder root path. (rooted path can also be used)</remarks>
        /// <returns>Self.</returns>
        IPackageSources Add(string path);

        /// <summary>
        /// Add the Nuget.org package source.
        /// </summary>
        /// <returns>Self.</returns>
        IPackageSources AddNugetOrg();
    }
}
