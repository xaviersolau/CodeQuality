// ----------------------------------------------------------------------
// <copyright file="ProcessHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public static int Run(string workingDirectory, string command, string arguments,
            out string stdout, out string stderr)
        {
            var outBuilder = new StringBuilder();
            var errBuilder = new StringBuilder();

            var exitCode = RunInternal(
                workingDirectory,
                command,
                arguments,
                s => outBuilder.Append(s),
                s => errBuilder.Append(s));

            stderr = errBuilder.ToString();
            stdout = outBuilder.ToString();

            return exitCode;
        }

        /// <summary>
        /// Run a process with the given arguments and wait for exit.
        /// </summary>
        /// <param name="workingDirectory">Working directory.</param>
        /// <param name="command">The command to run.</param>
        /// <param name="arguments">The command arguments.</param>
        /// <returns>The process result.</returns>
        public static ProcessResult Run(string workingDirectory, string command, string arguments)
        {
            var processResult = new ProcessResult();

            var exitCode = RunInternal(
                workingDirectory,
                command,
                arguments,
                processResult.AppendInfo,
                processResult.AppendError);

            processResult.SetReturnCode(exitCode);

            return processResult;
        }

        private static int RunInternal(string workingDirectory, string command, string arguments,
            Action<string> outCallback, Action<string> errorCallback)
        {
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
                        outCallback(e.Data);
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
                        errorCallback(e.Data);
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

                return process.ExitCode;
            }
        }
    }

    /// <summary>
    /// A log message.
    /// </summary>
    /// <param name="Message">Message log.</param>
    /// <param name="IsError">Tells if this is an error log.</param>
    /// <param name="TimeStamp">Log message time stamp.</param>
    public record struct Log(string Message, bool IsError, DateTime TimeStamp);

    /// <summary>
    /// Process logs.
    /// </summary>
    public class ProcessResult
    {
        private readonly List<Log> logs = new List<Log>();

        /// <summary>
        /// Log messages.
        /// </summary>
        public IReadOnlyList<Log> LogMessages => this.logs;

        /// <summary>
        /// Process exit code.
        /// </summary>
        public int ExitCode { get; private set; }

        internal void AppendInfo(string logMessage)
        {
            this.logs.Add(new Log(logMessage, false, DateTime.Now));
        }

        internal void AppendError(string logMessage)
        {
            this.logs.Add(new Log(logMessage, true, DateTime.Now));
        }

        internal void SetReturnCode(int exitCode)
        {
            this.ExitCode = exitCode;
        }

        public string GetErrors()
        {
            return GetLogs(l => l.IsError);
        }

        public string GetInfo()
        {
            return GetLogs(l => !l.IsError);
        }

        public string GetLogs(Func<Log, bool>? filter = null)
        {
            var stringBuilder = new StringBuilder();

            var logItems = filter != null
                ? this.logs.Where(filter)
                : this.logs;

            foreach (var log in logItems)
            {
                stringBuilder.AppendLine(log.Message);
            }

            return stringBuilder.ToString();
        }
    }
}
