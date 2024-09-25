// ----------------------------------------------------------------------
// <copyright file="ProcessHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Diagnostics;
using System.Text;
using System.Threading;

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
            var output = new StringBuilder();
            var error = new StringBuilder();

            using (var process = new Process())
            using (var outputWaitHandle = new AutoResetEvent(false))
            using (var errorWaitHandle = new AutoResetEvent(false))
            {
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        outputWaitHandle.Set();
                    }
                    else
                    {
                        output.AppendLine(e.Data);
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data == null)
                    {
                        errorWaitHandle.Set();
                    }
                    else
                    {
                        error.AppendLine(e.Data);
                    }
                };

                var dotnetStartInfo = new ProcessStartInfo(command, arguments)
                {
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                };

                process.StartInfo = dotnetStartInfo;

                process.Start();

                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                process.WaitForExit();

                outputWaitHandle.WaitOne();
                errorWaitHandle.WaitOne();

                stdout = output.ToString();
                stderr = error.ToString();

                return process.ExitCode;
            }
        }
    }
}
