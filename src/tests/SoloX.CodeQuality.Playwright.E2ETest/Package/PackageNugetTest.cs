// ----------------------------------------------------------------------
// <copyright file="PackageNugetTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using FluentAssertions;
using SoloX.CodeQuality.Test.Helpers;

namespace SoloX.CodeQuality.Playwright.E2ETest.Package
{
    public class PackageNugetTest
    {
        [Fact]
        public void IsShouldDeployNugetPackageAndRunTestWithEmbeddedWebHost()
        {
            var configuration = ProbConfiguration();

            var root = new RandomGenerator().RandomString(4);

            var nugetCache = Path.Combine(root, "PkgCache");
            var solutionPath = Path.Combine(root, "PackageNugetTestProject");
            var projectPath = Path.Combine(solutionPath, "PackageNugetTestProject");

            Directory.CreateDirectory(root);

            try
            {
                Directory.CreateDirectory(nugetCache);
                Directory.CreateDirectory(solutionPath);
                Directory.CreateDirectory(projectPath);

                CopyResourceFile(root, Path.Combine("PackageNugetTestProject", "nuget.config"), [("Configuration", configuration)]);
                CopyResourceFile(root, Path.Combine("PackageNugetTestProject", "PackageNugetTestProject.sln"));
                CopyResourceFile(root, Path.Combine("PackageNugetTestProject", "PackageNugetTestProject", "home.html"));
                CopyResourceFile(root, Path.Combine("PackageNugetTestProject", "PackageNugetTestProject", "PackageNugetTestProject.csproj"));
                CopyResourceFile(root, Path.Combine("PackageNugetTestProject", "PackageNugetTestProject", "PlaywrightTestBuilderLocalTest.cs"));

                var build = DotnetHelper.Build(Path.Combine(root, "PackageNugetTestProject"), out var _, out var _);

                build.Should().BeTrue();

                var test = DotnetHelper.Test(Path.Combine(root, "PackageNugetTestProject"), out var _, out var _);

                test.Should().BeTrue();
            }
            finally
            {
                Directory.Delete(root, true);
            }
        }

        private static string ProbConfiguration()
        {
            var location = Path.GetDirectoryName(typeof(PackageNugetTest).Assembly.Location);

            return Path.GetFileName(Path.GetDirectoryName(location)!);
        }

        private static void CopyResourceFile(string target, string filePath, IEnumerable<(string key, string value)> replaceItems = null)
        {
            var txt = File.ReadAllText(Path.Combine("Resources", filePath));

            if (replaceItems != null)
            {
                foreach (var item in replaceItems)
                {
                    txt = txt.Replace(item.key, item.value, StringComparison.InvariantCulture);
                }
            }

            File.WriteAllText(Path.Combine(target, filePath), txt);
        }
    }
}
