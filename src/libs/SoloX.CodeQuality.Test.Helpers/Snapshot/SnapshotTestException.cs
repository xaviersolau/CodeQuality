// ----------------------------------------------------------------------
// <copyright file="SnapshotTestException.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
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
    public class SnapshotTestException : Exception
    {
        public SnapshotTestException()
        {
        }

        public SnapshotTestException(string message) : base(message)
        {
        }

        public SnapshotTestException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
