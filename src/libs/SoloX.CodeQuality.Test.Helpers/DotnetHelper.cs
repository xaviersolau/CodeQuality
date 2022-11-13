// ----------------------------------------------------------------------
// <copyright file="DotnetHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.CodeQuality.Test.Helpers
{
#pragma warning disable CA1021 // Avoid out parameters
    public static class DotnetHelper
    {
        private const string DOTNET = "dotnet";
        private const string RESTORE = "restore";
        private const string BUILD = "build";
        private const string PUBLISH = "publish";
        private const string RUN = "run";

        public static bool Restore(string projectPath, out string stdout, out string stderr)
        {
            return ProcessHelper.Run(projectPath, DOTNET, RESTORE, out stdout, out stderr) == 0;
        }

        public static bool Build(string projectPath, out string stdout, out string stderr)
        {
            return ProcessHelper.Run(projectPath, DOTNET, BUILD, out stdout, out stderr) == 0;
        }

        public static bool Publish(string projectPath, out string stdout, out string stderr)
        {
            return ProcessHelper.Run(projectPath, DOTNET, PUBLISH, out stdout, out stderr) == 0;
        }

        public static bool Run(string projectPath, out string stdout, out string stderr)
        {
            return Run(projectPath, string.Empty, out stdout, out stderr);
        }

        public static bool Run(string projectPath, string args, out string stdout, out string stderr)
        {
            return ProcessHelper.Run(projectPath, DOTNET, $"{RUN} {args}", out stdout, out stderr) == 0;
        }
    }
#pragma warning restore CA1021 // Avoid out parameters
}
