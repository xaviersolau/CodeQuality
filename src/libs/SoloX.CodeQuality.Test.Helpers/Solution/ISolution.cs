// ----------------------------------------------------------------------
// <copyright file="ISolution.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.CodeQuality.Test.Helpers.Solution
{
    /// <summary>
    /// Dotnet Solution interface that helps to run dotnet commands.
    /// </summary>
    public interface ISolution
    {
        /// <summary>
        /// Name of the solution to build.
        /// </summary>
        string SolutionName { get; }

        /// <summary>
        /// Path of the solution to build.
        /// </summary>
        string SolutionPath { get; }

        /// <summary>
        /// Default configuration.
        /// </summary>
        string DefaultConfiguration { get; }

        /// <summary>
        /// Build solution.
        /// </summary>
        /// <param name="project">Project to build or null to build all solution.</param>
        /// <param name="configuration">Configuration to use.</param>
        /// <returns>Process result.</returns>
        ProcessResult Build(string? project = null, string? configuration = null);

        /// <summary>
        /// Test solution.
        /// </summary>
        /// <param name="project">Project to run.</param>
        /// <param name="configuration">Configuration to use.</param>
        /// <returns>Process result.</returns>
        ProcessResult Run(string? project = null, string? configuration = null);

        /// <summary>
        /// Test solution.
        /// </summary>
        /// <param name="project">Project to test or null to run all solution tests.</param>
        /// <param name="configuration">Configuration to use.</param>
        /// <returns>Process result.</returns>
        ProcessResult Test(string? project = null, string? configuration = null);

        /// <summary>
        /// Run a Dotnet tool.
        /// </summary>
        /// <param name="toolName">Tool command to run.</param>
        /// <param name="toolArgs">Tool command arguments to run.</param>
        /// <param name="project">Project where to run the tool or null to run from solution folder.</param>
        /// <returns>Process result.</returns>
        ProcessResult RunTool(string toolName, string? toolArgs = null, string? project = null);

        /// <summary>
        /// Get the project path.
        /// </summary>
        /// <param name="project">Project name.</param>
        /// <returns>The project path.</returns>
        string GetProjectPath(string project);

        /// <summary>
        /// Get the project binary path.
        /// </summary>
        /// <param name="project">Project name.</param>
        /// <param name="configuration">Configuration to use.</param>
        /// <returns>The project binary path.</returns>
        string GetProjectBinaryPath(string project, string? configuration = null);
    }
}
