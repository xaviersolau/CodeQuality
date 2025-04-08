// ----------------------------------------------------------------------
// <copyright file="IProjectFiles.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;

namespace SoloX.CodeQuality.Test.Helpers.Solution
{
    /// <summary>
    /// IProjectFiles provides a way to register files onto the project.
    /// </summary>
    public interface IProjectFiles
    {
        /// <summary>
        /// Add a source code file in the project folder.
        /// </summary>
        /// <param name="source">Source file path. Relative to the solution builder root folder.</param>
        /// <param name="target">Target file path. Relative to the project folder.</param>
        /// <param name="replaceItems">Text replace list.</param>
        /// <returns>Self.</returns>
        IProjectFiles Add(string source, string target, IEnumerable<(string key, string value)>? replaceItems = null);

        /// <summary>
        /// Add a content file in the project folder and update the project file.
        /// </summary>
        /// <param name="source">Source file path. Relative to the solution builder root folder.</param>
        /// <param name="target">Target file path. Relative to the project folder.</param>
        /// <param name="copyToOutputDirectory">Option specifying how to copy the resource file. (PreserveNewest,...)</param>
        /// <param name="replaceItems">Text replace list.</param>
        /// <returns>Self.</returns>
        IProjectFiles AddContent(
            string source,
            string target,
            string? copyToOutputDirectory = null,
            IEnumerable<(string key, string value)>? replaceItems = null);

        /// <summary>
        /// Remove the target file from the project folder.
        /// </summary>
        /// <param name="target">Target file path. Relative to the project folder.</param>
        /// <returns>Self.</returns>
        IProjectFiles Remove(string target);
    }

}
