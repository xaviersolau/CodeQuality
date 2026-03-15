// ----------------------------------------------------------------------
// <copyright file="TextSnapshotStrategy.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;
using System.Threading.Tasks;
using DiffPlex.Renderer;

namespace SoloX.CodeQuality.Test.Helpers.Snapshot.Impl
{
    /// <summary>
    /// Provides a snapshot strategy for handling plain text files, supporting saving and comparing text-based
    /// snapshots.
    /// </summary>
    /// <remarks>This class implements the ISnapshotStrategy interface for string-based snapshots, using the
    /// ".txt" file extension. It is suitable for scenarios where snapshot data is represented as plain text, such as
    /// generated code files or textual outputs.</remarks>
    public class TextSnapshotStrategy : ISnapshotStrategy<string>
    {
        private readonly bool ignoreWhitespace;
        private readonly bool ignoreCase;

        /// <inheritdoc/>
        public string FileExtension => "txt";

        public TextSnapshotStrategy(bool ignoreWhitespace = true, bool ignoreCase = false)
        {
            this.ignoreWhitespace = ignoreWhitespace;
            this.ignoreCase = ignoreCase;
        }

        /// <inheritdoc/>
        public Task SaveAsync(string snapshotFile, string snapshotData)
        {
            if (File.Exists(snapshotFile))
            {
                File.Delete(snapshotFile);
            }

            return File.WriteAllTextAsync(snapshotFile, snapshotData);
        }

        /// <inheritdoc/>
        public async Task<CompareSnapshotResult<string>> CompareAsync(string snapshotReferenceFile, string snapshotData)
        {
            var referenceText = await File.ReadAllTextAsync(snapshotReferenceFile).ConfigureAwait(false);
            var snapshotDiffs = UnidiffRenderer.GenerateUnidiff(
                referenceText,
                snapshotData,
                oldFileName: "Snapshot reference",
                newFileName: "Snapshot run",
                ignoreWhitespace: this.ignoreWhitespace,
                ignoreCase: this.ignoreCase);

            return new CompareSnapshotResult<string>(IsDifferent: !string.IsNullOrEmpty(snapshotDiffs), DiffsData: snapshotDiffs, DiffsString: snapshotDiffs);
        }
    }
}