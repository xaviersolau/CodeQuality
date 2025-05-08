// ----------------------------------------------------------------------
// <copyright file="SolutionBuilderException.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.CodeQuality.Test.Helpers.Solution.Exceptions
{
    public abstract class ASolutionBuilderError { }

    public sealed class BuilderError : ASolutionBuilderError { }
    public sealed class SolutionError : ASolutionBuilderError { }
    public sealed class ProjectError : ASolutionBuilderError { }
    public sealed class TestError : ASolutionBuilderError { }
    public sealed class ToolError : ASolutionBuilderError { }
    public sealed class NugetConfigError : ASolutionBuilderError { }

    /// <summary>
    /// Exception while building solution part.
    /// </summary>
    public class SolutionBuilderException<T> : Exception
        where T : ASolutionBuilderError
    {
        public ProcessResult? ProcessResult { get; }

        public SolutionBuilderException()
        {
        }

        public SolutionBuilderException(string message)
            : base(message)
        {
        }

        public SolutionBuilderException(ProcessResult processResult)
            : this(processResult.GetLogs())
        {
            ProcessResult = processResult;
        }

        public SolutionBuilderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
