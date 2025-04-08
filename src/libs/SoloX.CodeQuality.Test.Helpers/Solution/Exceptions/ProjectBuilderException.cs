// ----------------------------------------------------------------------
// <copyright file="ProjectBuilderException.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.CodeQuality.Test.Helpers.Solution.Exceptions
{
    /// <summary>
    /// Exception while building a project part.
    /// </summary>
    public class ProjectBuilderException : Exception
    {
        private readonly ProcessResult? processResult;

        public ProjectBuilderException()
        {
        }

        public ProjectBuilderException(string message)
            : base(message)
        {
        }

        public ProjectBuilderException(ProcessResult processResult)
            : this(processResult.GetLogs())
        {
            this.processResult = processResult;
        }

        public ProjectBuilderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
