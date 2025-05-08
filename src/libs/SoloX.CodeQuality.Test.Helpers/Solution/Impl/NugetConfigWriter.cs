// ----------------------------------------------------------------------
// <copyright file="NugetConfigWriter.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SoloX.CodeQuality.Test.Helpers.Solution.Exceptions;

namespace SoloX.CodeQuality.Test.Helpers.Solution.Impl
{
    internal class NugetConfigWriter : INugetConfigConfiguration, IPackageSources
    {
        private readonly SolutionBuilder solutionBuilder;
        private readonly string globalPackagesFolder;

        private readonly List<string> config = [];

        public NugetConfigWriter(SolutionBuilder solutionBuilder, string globalPackagesFolder)
        {
            this.solutionBuilder = solutionBuilder;
            this.globalPackagesFolder = globalPackagesFolder;
        }

        public IPackageSources Add(string path)
        {
            var keyName = $"source_{this.config.Count}";

            var sourcePath = Path.IsPathRooted(path)
                ? path
                : Path.Combine("..", path);

            this.config.Add($"    <add key=\"{keyName}\" value=\"{sourcePath}\" />");
            return this;
        }

        public IPackageSources AddNugetOrg()
        {
            var keyName = $"source_{this.config.Count}";
            this.config.Add($"    <add key=\"{keyName}\" value=\"https://api.nuget.org/v3/index.json\" />");
            return this;
        }

        public IPackageSources Clear()
        {
            this.config.Clear();

            this.config.Add("    <clear />");
            return this;
        }

        public void UsePackageSources(Action<IPackageSources> packageSources)
        {
            packageSources(this);
        }

        public void Build()
        {
            var configFilePath = Path.Combine(this.solutionBuilder.SolutionPath, "nuget.config");

            IEnumerable<string> begin =
            [
                "<configuration>",
                    "  <config>",
                    $"    <add key=\"globalPackagesFolder\" value=\"..\\{this.globalPackagesFolder}\" />",
                    "  </config>",
                    "  <packageSources>",
                ];

            IEnumerable<string> end =
            [
                "  </packageSources>",
                "</configuration>",
            ];

            try
            {
                File.WriteAllLines(configFilePath,
                    begin
                    .Concat(this.config)
                    .Concat(end));
            }
            catch (Exception e)
            {
                throw new SolutionBuilderException<NugetConfigError>("Could not write Nuget config file.", e);
            }
        }
    }
}
