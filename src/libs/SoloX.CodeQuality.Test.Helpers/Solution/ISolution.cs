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
        /// Build solution.
        /// </summary>
        /// <param name="project">Project to build or null to build all solution.</param>
        void Build(string? project = null);

        /// <summary>
        /// Test solution.
        /// </summary>
        /// <param name="project">Project to run.</param>
        void Run(string? project = null);

        /// <summary>
        /// Test solution.
        /// </summary>
        /// <param name="project">Project to test or null to run all solution tests.</param>
        void Test(string? project = null);
    }
}
