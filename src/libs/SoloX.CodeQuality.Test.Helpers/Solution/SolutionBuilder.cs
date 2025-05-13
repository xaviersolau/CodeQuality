// ----------------------------------------------------------------------
// <copyright file="SolutionBuilder.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using SoloX.CodeQuality.Test.Helpers.Solution.Exceptions;
using SoloX.CodeQuality.Test.Helpers.Solution.Impl;

namespace SoloX.CodeQuality.Test.Helpers.Solution
{
    /// <summary>
    /// Dotnet fluent solution builder.
    /// It helps to create a solution and its projects to make E2E tests dedicated for testing:
    /// - Your packaged Nugets;
    /// - Your Dotnet tools
    /// - Your Dotnet Analyzers;
    /// </summary>
    public class SolutionBuilder
    {
        private const string DefaultPackageFolder = "Packages";
        private static readonly Action<INugetConfigConfiguration> DefaultNugetConfigConfigurationHandler =
            configuration => configuration.UsePackageSources(s =>
            {
                s.Clear();
                s.AddNugetOrg();
            });

        /// <summary>
        /// Solution builder root folder where the solution will be created.
        /// </summary>
        public string Root { get; }

        /// <summary>
        /// Name of the solution to build.
        /// </summary>
        public string SolutionName { get; }

        /// <summary>
        /// Path of the solution to build.
        /// </summary>
        public string SolutionPath { get; }

        private bool withNugetConfig;
        private string globalPackagesFolder = DefaultPackageFolder;
        private Action<INugetConfigConfiguration> nugetConfigConfigurationHandler = DefaultNugetConfigConfigurationHandler;

        private Action<IDotnetToolsConfiguration>? dotnetToolsConfigurationHandler;

        private readonly Dictionary<string, ProjectConfiguration> projectConfigurations =
            new Dictionary<string, ProjectConfiguration>();

        /// <summary>
        /// Build SolutionBuilder instance.
        /// </summary>
        /// <param name="root">Root folder.</param>
        /// <param name="solutionName">Solution name.</param>
        public SolutionBuilder(string root, string solutionName)
        {
            this.Root = root;
            this.SolutionName = solutionName;
            this.SolutionPath = Path.Combine(this.Root, this.SolutionName);
        }

        /// <summary>
        /// Generate the solution with a nuget.config file.
        /// </summary>
        /// <param name="globalPackagesFolder">The globalPackagesFolder where Nugets cache is located.
        /// Relative to the Solution builder Root folder.</param>
        /// <param name="configuration">nuget.config file configuration.</param>
        /// <returns>Self.</returns>
        /// <exception cref="SolutionBuilderException">if method already called.</exception>
        public SolutionBuilder WithNugetConfig(
            string globalPackagesFolder = DefaultPackageFolder,
            Action<INugetConfigConfiguration>? configuration = null)
        {
            if (this.withNugetConfig)
            {
                throw new SolutionBuilderException<BuilderError>("WithNugetConfig builder method already called.");
            }

            this.withNugetConfig = true;
            this.globalPackagesFolder = globalPackagesFolder;
            this.nugetConfigConfigurationHandler = configuration ?? DefaultNugetConfigConfigurationHandler;

            return this;
        }

        /// <summary>
        /// Generate the solution with a project named with the given projectName.
        /// </summary>
        /// <param name="projectName">Name of the project to create.</param>
        /// <param name="template">Project Template to use (like 'classlib', 'xunit'...).</param>
        /// <param name="configuration">Project configuration.</param>
        /// <returns>Self.</returns>
        public SolutionBuilder WithProject(string projectName, string template, Action<IProjectConfiguration> configuration)
        {
            var projectConfiguration = new ProjectConfiguration(this, projectName, template, configuration);

            this.projectConfigurations.Add(projectName, projectConfiguration);

            return this;
        }

        /// <summary>
        /// Generate the solution with a tool manifest file.
        /// </summary>
        /// <param name="configuration">Dotnet tools configuration.</param>
        /// <returns>Self.</returns>
        /// <exception cref="SolutionBuilderException">if method already called.</exception>
        public SolutionBuilder WithDotnetTools(Action<IDotnetToolsConfiguration> configuration)
        {
            if (this.dotnetToolsConfigurationHandler != null)
            {
                throw new SolutionBuilderException<BuilderError>("WithDotnetTools builder method already called.");
            }

            this.dotnetToolsConfigurationHandler = configuration;

            return this;
        }

        /// <summary>
        /// Build the solution and all its projects.
        /// </summary>
        public ISolution Build()
        {
            Directory.CreateDirectory(this.Root);

            try
            {
                if (!Directory.Exists(Path.Combine(this.Root, this.globalPackagesFolder)))
                {
                    Directory.CreateDirectory(Path.Combine(this.Root, this.globalPackagesFolder));
                }

                DotnetCall<BuilderError>((out ProcessResult processResult) =>
                    DotnetHelper.NewSln(this.Root, this.SolutionName, out processResult, SetupVariables)
                );

                if (this.withNugetConfig)
                {
                    var nugetConfigWriter = new NugetConfigWriter(this, this.globalPackagesFolder);

                    this.nugetConfigConfigurationHandler(nugetConfigWriter);

                    nugetConfigWriter.Build();
                }

                if (this.dotnetToolsConfigurationHandler != null)
                {
                    var dotnetToolsConfiguration = new DotnetToolsConfiguration(this);

                    this.dotnetToolsConfigurationHandler(dotnetToolsConfiguration);

                    dotnetToolsConfiguration.Build();
                }

                var projectPathMap = new Dictionary<string, string>();

                foreach (var projectConfigurationItem in this.projectConfigurations)
                {
                    var projectConfiguration = projectConfigurationItem.Value;

                    projectPathMap.Add(
                        projectConfiguration.ProjectName,
                        projectConfiguration.ProjectPath);

                    var projectFilePath = projectConfiguration.ProjectFilePath;

                    projectConfiguration.Build();

                    DotnetCall<BuilderError>((out ProcessResult processResult) =>
                        DotnetHelper.SlnAdd(this.SolutionPath, projectFilePath, out processResult, SetupVariables)
                    );
                }

                return new Solution(this, projectPathMap);
            }
            catch (Exception)
            {
                Directory.Delete(this.Root, true);

                throw;
            }
        }

        internal delegate bool DotnetCallHandler(out ProcessResult processResult);

        internal static ProcessResult DotnetCall<T>(DotnetCallHandler handler)
            where T : ASolutionBuilderError
        {
            if (!handler(out var processResult))
            {
                throw new SolutionBuilderException<T>(processResult);
            }

            return processResult;
        }

        internal void SetupVariables(StringDictionary environmentVariables)
        {
            environmentVariables.Add("DOTNET_CLI_HOME", Path.GetFullPath(Root));
        }

        private class Solution : ISolution
        {
            private readonly SolutionBuilder solutionBuilder;
            private readonly IReadOnlyDictionary<string, string> projectPathMap;

            public Solution(SolutionBuilder solutionBuilder, IReadOnlyDictionary<string, string> projectPathMap)
            {
                this.solutionBuilder = solutionBuilder;
                this.projectPathMap = projectPathMap;
            }

            public ProcessResult Build(string? project = null)
            {
                return string.IsNullOrEmpty(project)
                    ? DotnetCall<SolutionError>((out ProcessResult processResult) =>
                        DotnetHelper.Build(this.solutionBuilder.SolutionPath, out processResult, this.solutionBuilder.SetupVariables)
                    )
                    : DotnetCall<ProjectError>((out ProcessResult processResult) =>
                        DotnetHelper.Build(GetProjectPath(project), out processResult, this.solutionBuilder.SetupVariables)
                    );
            }

            public ProcessResult Run(string? project = null)
            {
                return string.IsNullOrEmpty(project)
                    ? DotnetCall<SolutionError>((out ProcessResult processResult) =>
                        DotnetHelper.Run(this.solutionBuilder.SolutionPath, out processResult, this.solutionBuilder.SetupVariables)
                    )
                    : DotnetCall<ProjectError>((out ProcessResult processResult) =>
                        DotnetHelper.Run(GetProjectPath(project), out processResult, this.solutionBuilder.SetupVariables)
                    );
            }

            public ProcessResult RunTool(string toolName, string? toolArgs, string? project = null)
            {
                var path = string.IsNullOrEmpty(project)
                    ? this.solutionBuilder.SolutionPath
                    : GetProjectPath(project);

                return DotnetCall<ToolError>((out ProcessResult processResult) =>
                    DotnetHelper.Dotnet(path, $"{toolName} {toolArgs}", out processResult, this.solutionBuilder.SetupVariables)
                );
            }

            public ProcessResult Test(string? project = null)
            {
                var path = string.IsNullOrEmpty(project)
                    ? this.solutionBuilder.SolutionPath
                    : GetProjectPath(project);

                return DotnetCall<TestError>((out ProcessResult processResult) =>
                    DotnetHelper.Test(path, out processResult, this.solutionBuilder.SetupVariables)
                );
            }

            private string GetProjectPath(string project)
            {
                if (this.projectPathMap.TryGetValue(project, out var projectPath))
                {
                    return Path.Combine(this.solutionBuilder.SolutionPath, projectPath);
                }

                throw new SolutionBuilderException<SolutionError>($"Could not find project {project}");
            }
        }
    }
}
