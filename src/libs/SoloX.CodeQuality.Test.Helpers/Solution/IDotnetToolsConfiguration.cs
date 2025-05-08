// ----------------------------------------------------------------------
// <copyright file="IDotnetToolsConfiguration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.CodeQuality.Test.Helpers.Solution
{
    public interface IDotnetToolsConfiguration
    {
        /// <summary>
        /// Use and install the given Dotnet Tool locally to the solution.
        /// </summary>
        /// <param name="dotnetToolName">Dotnet Tool name to install.</param>
        /// <returns>Self.</returns>
        IDotnetToolsConfiguration UseTool(string dotnetToolName);
    }
}
