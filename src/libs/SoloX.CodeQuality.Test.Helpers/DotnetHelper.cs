// ----------------------------------------------------------------------
// <copyright file="DotnetHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Collections.Specialized;
using System;

namespace SoloX.CodeQuality.Test.Helpers
{
#pragma warning disable CA1021 // Avoid out parameters
    /// <summary>
    /// DotnetHelper allows to run dotnet command.
    /// </summary>
    public static class DotnetHelper
    {
        private const string DOTNET = "dotnet";
        private const string RESTORE = "restore";
        private const string BUILD = "build";
        private const string PUBLISH = "publish";
        private const string RUN = "run";
        private const string TEST = "test";
        private const string NEW = "new";
        private const string SLN = "sln";
        private const string ADD = "add";
        private const string PACKAGE = "package";
        private const string REFERENCE = "reference ";
        private const string TOOL = "tool";
        private const string TOOL_MANIFEST = "tool-manifest";
        private const string INSTALL = "install";

        public static bool Restore(string projectPath, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null)
        {
            return Dotnet(projectPath, RESTORE, out processResult, environmentVariablesHandler);
        }

        public static bool Build(string projectPath, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null, string? configuration = null)
        {
            var configurationArg = string.IsNullOrEmpty(configuration) ? string.Empty : $" --configuration {configuration}";

            return Dotnet(projectPath, $"{BUILD}{configurationArg}", out processResult, environmentVariablesHandler);
        }

        public static bool Test(string projectPath, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null, string? configuration = null)
        {
            var configurationArg = string.IsNullOrEmpty(configuration) ? string.Empty : $" --configuration {configuration}";

            return Dotnet(projectPath, $"{TEST}{configurationArg}", out processResult, environmentVariablesHandler);
        }

        public static bool Publish(string projectPath, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null, string? configuration = null)
        {
            var configurationArg = string.IsNullOrEmpty(configuration) ? string.Empty : $" --configuration {configuration}";

            return Dotnet(projectPath, $"{PUBLISH}{configurationArg}", out processResult, environmentVariablesHandler);
        }

        public static bool New(string path, string template, string? framework, string output, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null)
        {
            return string.IsNullOrEmpty(framework)
                ? Dotnet(path, $"{NEW} {template} --output {output}", out processResult, environmentVariablesHandler)
                : Dotnet(path, $"{NEW} {template} --output {output} --framework {framework}", out processResult, environmentVariablesHandler);
        }

        public static bool New(string path, string template, string output, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null)
        {
            return New(path, template, null, output, out processResult, environmentVariablesHandler);
        }

        public static bool NewSln(string path, string solutionName, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null)
        {
            return New(path, SLN, solutionName, out processResult, environmentVariablesHandler);
        }

        public static bool SlnAdd(string path, string project, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null)
        {
            return Dotnet(path, $"{SLN} {ADD} {project}", out processResult, environmentVariablesHandler);
        }

        public static bool AddPackage(string path, string projectFilePath, string packageName, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null)
        {
            return Dotnet(path, $"{ADD} {projectFilePath} {PACKAGE} {packageName} --prerelease", out processResult, environmentVariablesHandler);
        }

        public static bool AddReference(string path, string projectFilePath, string projectReferenceFilePath, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null)
        {
            return Dotnet(path, $"{ADD} {projectFilePath} {REFERENCE} {projectReferenceFilePath}", out processResult, environmentVariablesHandler);
        }

        public static bool Run(string projectPath, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null, string? configuration = null)
        {
            return Run(projectPath, string.Empty, out processResult, environmentVariablesHandler, configuration);
        }

        public static bool Run(string projectPath, string args, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null, string? configuration = null)
        {
            var configurationArg = string.IsNullOrEmpty(configuration) ? string.Empty : $" --configuration {configuration}";

            return Dotnet(projectPath, $"{RUN}{configurationArg} {args}", out processResult, environmentVariablesHandler);
        }

        internal static bool NewToolManifest(string path, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null)
        {
            return Dotnet(path, $"{NEW} {TOOL_MANIFEST}", out processResult, environmentVariablesHandler);
        }

        internal static bool ToolInstall(string path, string toolName, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null)
        {
            return Dotnet(path, $"{TOOL} {INSTALL} {toolName} --prerelease", out processResult, environmentVariablesHandler);
        }

        public static bool Dotnet(string path, string args, out ProcessResult processResult,
            Action<StringDictionary>? environmentVariablesHandler = null)
        {
            processResult = ProcessHelper.Run(path, DOTNET, args, environmentVariablesHandler);
            return processResult.ExitCode == 0;
        }
    }
#pragma warning restore CA1021 // Avoid out parameters
}
