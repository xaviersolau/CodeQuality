// ----------------------------------------------------------------------
// <copyright file="ISnapshotStrategy.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Threading.Tasks;

namespace SoloX.CodeQuality.Test.Helpers.Snapshot
{
    /// <summary>
    /// Defines a strategy for saving and comparing snapshot data of a specified type.
    /// </summary>
    /// <remarks>Implementations of this interface provide mechanisms for persisting snapshot data to files
    /// and comparing new snapshots against existing references. This enables flexible support for different snapshot
    /// formats or comparison algorithms. Thread safety and file access behavior depend on the specific
    /// implementation.</remarks>
    /// <typeparam name="TData">The type of the snapshot data to be handled by the strategy.</typeparam>
    public interface ISnapshotStrategy<TData>
    {
        /// <summary>
        /// Gets the file extension associated with the current snapshot strategy.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Asynchronously saves the specified snapshot data to the given file.
        /// </summary>
        /// <param name="snapshotFile">The path to the file where the snapshot data will be saved. Cannot be null or empty.</param>
        /// <param name="snapshotData">The snapshot data to save. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous save operation.</returns>
        Task SaveAsync(string snapshotFile, TData snapshotData);

        /// <summary>
        /// Compares the provided snapshot data to the reference snapshot stored in the specified file and returns the
        /// result of the comparison.
        /// </summary>
        /// <param name="snapshotReferenceFile">The path to the file containing the reference snapshot to compare against. Cannot be null or empty.</param>
        /// <param name="snapshotData">The snapshot data to compare with the reference snapshot. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a CompareSnapshotResult
        /// indicating whether the snapshots match and providing details of any differences.</returns>
        Task<CompareSnapshotResult<TData>> CompareAsync(string snapshotReferenceFile, TData snapshotData);
    }
}