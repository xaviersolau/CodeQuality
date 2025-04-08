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
    /// <summary>
    /// Exception while building solution part.
    /// </summary>
    public class SolutionBuilderException : Exception
    {
        private readonly ProcessResult? processResult;

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
            this.processResult = processResult;
        }

        public SolutionBuilderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
