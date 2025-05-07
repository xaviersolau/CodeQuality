// ----------------------------------------------------------------------
// <copyright file="ProjectConfiguration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Construction;
using SoloX.CodeQuality.Test.Helpers.Solution.Exceptions;

namespace SoloX.CodeQuality.Test.Helpers.Solution.Impl
{
    internal class ProjectConfiguration : IProjectConfiguration, IProjectFiles
    {
        private readonly SolutionBuilder solutionBuilder;
        private readonly Action<IProjectConfiguration> configuration;

        public ProjectConfiguration(
            SolutionBuilder solutionBuilder,
            string projectName,
            string template,
            Action<IProjectConfiguration> configuration)
        {
            this.solutionBuilder = solutionBuilder;
            ProjectName = projectName;
            Template = template;

            ProjectFilePath = Path.Combine(projectName, $"{projectName}.csproj");
            ProjectPath = projectName;

            this.configuration = configuration;
        }

        /// <summary>
        /// Project name.
        /// </summary>
        public string ProjectName { get; }

        /// <summary>
        /// Path to the project folder. Relative to the solution.
        /// </summary>
        public string ProjectPath { get; }

        /// <summary>
        /// Project file path. Relative to the solution.
        /// </summary>
        public string ProjectFilePath { get; }

        /// <summary>
        /// Project Template.
        /// </summary>
        public string Template { get; }

        public void Build()
        {
            SolutionBuilder.DotnetCall<ProjectError>((out ProcessResult processResult) =>
                DotnetHelper.New(this.solutionBuilder.SolutionPath, Template, ProjectName, out processResult)
            );

            this.configuration(this);
        }

        public IProjectConfiguration UseFiles(Action<IProjectFiles> files)
        {
            files(this);

            return this;
        }

        public IProjectFiles Add(string source, string target, IEnumerable<(string key, string value)>? replaceItems = null)
        {
            CopyResourceFile(source, target, replaceItems);

            return this;
        }

        public IProjectFiles AddContent(
            string source,
            string target,
            string? copyToOutputDirectory = null,
            IEnumerable<(string key, string value)>? replaceItems = null)
        {
            Add(source, target, replaceItems);

            var projectRoot = ProjectRootElement.Open(Path.Combine(this.solutionBuilder.SolutionPath, ProjectFilePath));

            var itemGroup = projectRoot.AddItemGroup();

            var contentItem = itemGroup.AddItem("Content", target);

            if (!string.IsNullOrEmpty(copyToOutputDirectory))
            {
                contentItem.AddMetadata("CopyToOutputDirectory", copyToOutputDirectory, expressAsAttribute: true);
            }

            projectRoot.Save();

            return this;
        }

        public IProjectFiles Remove(string target)
        {
            var filePath = Path.Combine(this.solutionBuilder.SolutionPath, this.ProjectPath, target);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return this;
        }

        public IProjectConfiguration UsePackageReference(string packageName)
        {
            SolutionBuilder.DotnetCall<ProjectError>((out ProcessResult processResult) =>
                DotnetHelper.AddPackage(this.solutionBuilder.SolutionPath, ProjectFilePath, packageName, out processResult)
            );

            return this;
        }

        public IProjectConfiguration UseGlobalUsing(string usingNamespace)
        {
            var projectRoot = ProjectRootElement.Open(Path.Combine(this.solutionBuilder.SolutionPath, ProjectFilePath));

            var itemGroup = projectRoot.AddItemGroup();

            var contentItem = itemGroup.AddItem("Using", usingNamespace);

            projectRoot.Save();

            return this;
        }

        private void CopyResourceFile(string filePath, string targetPath, IEnumerable<(string key, string value)>? replaceItems = null)
        {
            var txt = File.ReadAllText(Path.Combine(this.solutionBuilder.Root, filePath));

            if (replaceItems != null)
            {
                foreach (var item in replaceItems)
                {
                    txt = txt.Replace(item.key, item.value, StringComparison.InvariantCulture);
                }
            }

            File.WriteAllText(Path.Combine(this.solutionBuilder.SolutionPath, this.ProjectPath, targetPath), txt);
        }
    }
}
