// ----------------------------------------------------------------------
// <copyright file="SnapshotException.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.CodeQuality.Test.Helpers.Snapshot
{
    /// <summary>
    /// Represents errors that occur during snapshot operations.
    /// </summary>
    /// <remarks>This exception is typically thrown when a snapshot operation fails due to differences between
    /// snapshot reference and generated snapshot to test.</remarks>
    public class SnapshotException : Exception
    {
        public SnapshotException()
        {
        }

        public SnapshotException(string message) : base(message)
        {
        }

        public SnapshotException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
