// ----------------------------------------------------------------------
// <copyright file="ISnapshotTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Threading.Tasks;

namespace SoloX.CodeQuality.Test.Helpers.Snapshot
{
    /// <summary>
    /// Defines a helper for comparing and managing snapshot data of a specified type.
    /// </summary>
    /// <typeparam name="TData">The type of data to be used for snapshot comparison.</typeparam>
    public interface ISnapshotTest<TData>
    {
        /// <summary>
        /// Compares the provided snapshot data with the existing reference snapshot and updates or validates the
        /// reference as needed.
        /// </summary>
        /// <remarks>If the reference snapshot does not exist or <paramref name="forceReplaceSnapshot"/>
        /// is true, the reference is replaced with the provided data. If differences are detected
        /// during comparison, the method saves the run and diff files before throwing an exception. Temporary files
        /// created during comparison are cleaned up after the operation.</remarks>
        /// <param name="snapshotName">The name of the snapshot to compare or update. Used to locate the corresponding snapshot files.</param>
        /// <param name="snapshotData">The data to compare against the reference snapshot. This data is saved or validated depending on the
        /// operation.</param>
        /// <param name="forceReplaceSnapshot">If set to true, replaces the reference snapshot with the provided data regardless of
        /// differences. If false, only replaces if the reference does not exist.</param>
        /// <returns>A task that represents the asynchronous compare operation.</returns>
        /// <exception cref="SnapshotTestException">Thrown if the provided snapshot data differs from the reference snapshot.</exception>
        Task CompareSnapshotAsync(string snapshotName, TData snapshotData, bool forceReplaceSnapshot = false);
    }
}