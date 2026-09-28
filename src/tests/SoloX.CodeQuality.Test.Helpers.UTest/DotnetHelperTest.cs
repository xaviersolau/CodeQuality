// ----------------------------------------------------------------------
// <copyright file="DotnetHelperTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Shouldly;
using Xunit;

namespace SoloX.CodeQuality.Test.Helpers.UTest
{

    public class DotnetHelperTest
    {
        private const string DotnetNewSearchOutput = @"
Searching for the templates...
Matches from template source: NuGet.org
These templates matched your input: 'xunit3'

Template Name                   Short Name        Language    Package Name / Owners       Trusted  Downloads
------------------------------  ----------------  ----------  --------------------------  -------  ---------
xUnit.net v3 Extension Project  xunit3-extension  [C#],F#,VB  xunit.v3.templates / xunit     ✓        36k   
xUnit.net v3 Test Project       xunit3            [C#],F#,VB  xunit.v3.templates / xunit     ✓        36k   
Untrusted Test Project          untrusted         [C#],F#,VB  xunit.v3.templates / xunit              36k   


To use the template, run the following command to install the package:
   dotnet new install [<package>...]
Example:
   dotnet new install xunit.v3.templates
";

        [Fact]
        public void IsShouldReadDotnetNewSearchOutput()
        {
            var result = DotnetHelper.ExtractDotnetNewSearchOutput(DotnetNewSearchOutput.Split(Environment.NewLine));

            result.ShouldNotBeNull();

            result.ShouldContainKeyAndValue("xunit3-extension", "xunit.v3.templates");
            result.ShouldContainKeyAndValue("xunit3", "xunit.v3.templates");
            result.ShouldNotContainKey("untrusted");
        }
    }
}