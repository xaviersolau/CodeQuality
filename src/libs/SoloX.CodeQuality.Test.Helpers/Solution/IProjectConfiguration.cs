// ----------------------------------------------------------------------
// <copyright file="IProjectConfiguration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.CodeQuality.Test.Helpers.Solution
{
    /// <summary>
    /// IProjectConfiguration is the interface to create a dotnet project.
    /// </summary>
    public interface IProjectConfiguration
    {
        /// <summary>
        /// Use a package reference in the project.
        /// </summary>
        /// <param name="packageName">The package name.</param>
        /// <returns>Self.</returns>
        IProjectConfiguration UsePackageReference(string packageName);

        /// <summary>
        /// Use a global using in the project.
        /// </summary>
        /// <param name="usingNamespace">The namespace to use.</param>
        /// <returns>Self.</returns>
        IProjectConfiguration UseGlobalUsing(string usingNamespace);

        /// <summary>
        /// Use files in the project.
        /// </summary>
        /// <param name="value">Project files configuration.</param>
        /// <returns>Self.</returns>
        IProjectConfiguration UseFiles(Action<IProjectFiles> value);
    }

}
