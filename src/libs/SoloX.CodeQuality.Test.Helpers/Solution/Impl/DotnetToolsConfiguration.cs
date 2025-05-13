// ----------------------------------------------------------------------
// <copyright file="DotnetToolsConfiguration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Generic;
using SoloX.CodeQuality.Test.Helpers.Solution.Exceptions;

namespace SoloX.CodeQuality.Test.Helpers.Solution.Impl
{
    internal class DotnetToolsConfiguration : IDotnetToolsConfiguration
    {
        private readonly SolutionBuilder solutionBuilder;
        private readonly List<string> dotnetTools = new List<string>();

        public DotnetToolsConfiguration(SolutionBuilder solutionBuilder)
        {
            this.solutionBuilder = solutionBuilder;
        }

        public IDotnetToolsConfiguration UseTool(string dotnetToolName)
        {
            this.dotnetTools.Add(dotnetToolName);

            return this;
        }

        public void Build()
        {
            SolutionBuilder.DotnetCall<ToolError>((out ProcessResult processResult) =>
                DotnetHelper.NewToolManifest(this.solutionBuilder.SolutionPath, out processResult, this.solutionBuilder.SetupVariables)
            );

            foreach (var dotnetTool in this.dotnetTools)
            {
                SolutionBuilder.DotnetCall<ToolError>((out ProcessResult processResult) =>
                    DotnetHelper.ToolInstall(this.solutionBuilder.SolutionPath, dotnetTool, out processResult, this.solutionBuilder.SetupVariables)
                );
            }
        }
    }
}
