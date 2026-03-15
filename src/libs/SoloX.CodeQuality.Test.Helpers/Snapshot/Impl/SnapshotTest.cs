// ----------------------------------------------------------------------
// <copyright file="SnapshotTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;

namespace SoloX.CodeQuality.Test.Helpers.Snapshot.Impl
{

    /// <summary>
    /// Provides helper methods for managing and comparing data snapshots using a specified snapshot strategy.
    /// </summary>
    /// <remarks>This class is typically used in testing scenarios to persist, compare, and update snapshots
    /// of data for regression testing or verification purposes. The snapshot strategy defines how snapshots are
    /// serialized, saved, and compared. Thread safety depends on the provided snapshot strategy and usage
    /// context.</remarks>
    /// <typeparam name="TData">The type of data to be managed by the snapshot helper.</typeparam>
    public class SnapshotTest<TData> : ISnapshotTest<TData>
    {
        private readonly ISnapshotStrategy<TData> snapshotStrategy;
        private readonly string snapshotsFolder;

        /// <summary>
        /// Initializes a new instance of the SnapshotTest class with the specified snapshot strategy and snapshots
        /// folder.
        /// </summary>
        /// <param name="snapshotStrategy">The strategy used to create and compare snapshots of type TData. Cannot be null.</param>
        /// <param name="snapshotsFolder">The path to the folder where snapshot files are stored. Cannot be null or empty.</param>
        public SnapshotTest(ISnapshotStrategy<TData> snapshotStrategy, string snapshotsFolder)
        {
            this.snapshotsFolder = snapshotsFolder;
            this.snapshotStrategy = snapshotStrategy;
        }

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
        public async Task CompareSnapshotAsync(string snapshotName, TData snapshotData, bool forceReplaceSnapshot = false)
        {
            var fileExt = this.snapshotStrategy.FileExtension;

            var snapshotReferenceFile = Path.Combine(this.snapshotsFolder, $"{snapshotName}.snapshot.ref.{fileExt}");
            var snapshotRunFile = Path.Combine(this.snapshotsFolder, $"{snapshotName}.snapshot.run.{fileExt}");
            var snapshotDiffsFile = Path.Combine(this.snapshotsFolder, $"{snapshotName}.snapshot.diffs.{fileExt}");

            forceReplaceSnapshot = forceReplaceSnapshot || !File.Exists(snapshotReferenceFile);

            if (!Directory.Exists(this.snapshotsFolder) && forceReplaceSnapshot)
            {
                Directory.CreateDirectory(this.snapshotsFolder);
            }

            if (forceReplaceSnapshot)
            {
                await this.snapshotStrategy.SaveAsync(snapshotReferenceFile, snapshotData).ConfigureAwait(false);
            }
            else
            {
                var compareResult = await this.snapshotStrategy.CompareAsync(snapshotReferenceFile, snapshotData).ConfigureAwait(false);

                if (forceReplaceSnapshot)
                {
                    await this.snapshotStrategy.SaveAsync(snapshotReferenceFile, snapshotData).ConfigureAwait(false);
                }

                if (compareResult.IsDifferent)
                {
                    await this.snapshotStrategy.SaveAsync(snapshotRunFile, snapshotData).ConfigureAwait(false);

                    await this.snapshotStrategy.SaveAsync(snapshotDiffsFile, compareResult.DiffsData!).ConfigureAwait(false);

                    throw new SnapshotTestException(compareResult.DiffsString!);
                }
            }

            if (File.Exists(snapshotRunFile))
            {
                File.Delete(snapshotRunFile);
            }

            if (File.Exists(snapshotDiffsFile))
            {
                File.Delete(snapshotDiffsFile);
            }
        }
    }
}