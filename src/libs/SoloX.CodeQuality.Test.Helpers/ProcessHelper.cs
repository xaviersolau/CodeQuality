// ----------------------------------------------------------------------
// <copyright file="ProcessHelper.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Diagnostics;

namespace SoloX.CodeQuality.Test.Helpers
{
    /// <summary>
    /// ProcessHelper allows to run a process and to get the output.
    /// </summary>
    public static class ProcessHelper
    {
        /// <summary>
        /// Run a process with the given arguments and wait for exit.
        /// </summary>
        /// <param name="workingDirectory">Working directory.</param>
        /// <param name="command">The command to run.</param>
        /// <param name="arguments">The command arguments.</param>
        /// <param name="stdout">Standard output.</param>
        /// <param name="stderr">Error output.</param>
        /// <returns>The process exit code.</returns>
        public static int Run(string workingDirectory, string command, string arguments, out string stdout, out string stderr)
        {
            var dotnet = new ProcessStartInfo(command, arguments)
            {
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
            };
            using (var process = Process.Start(dotnet))
            {
                process.WaitForExit();

                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();

                return process.ExitCode;
            }
        }
    }
}
