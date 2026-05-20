// ----------------------------------------------------------------------
// <copyright file="IProjectConfiguration.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
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
        /// <param name="files">An action that receives an <see cref="IProjectFiles"/> instance to
        /// configure project properties.</param>
        /// <returns>Self.</returns>
        IProjectConfiguration UseFiles(Action<IProjectFiles> files);

        /// <summary>
        /// Configures project properties using the specified action.
        /// </summary>
        /// <remarks>This method enables fluent configuration of project properties. The provided action
        /// is invoked immediately with a mutable properties object.</remarks>
        /// <param name="props">An action that receives an <see cref="IProjectProperties"/> instance to
        /// configure project properties.</param>
        /// <returns>Self.</returns>
        IProjectConfiguration UseProperties(Action<IProjectProperties> props);
    }

}
