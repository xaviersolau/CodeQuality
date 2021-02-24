// ----------------------------------------------------------------------
// <copyright file="SnapshotHelper.cs" company="SoloX Software">
// Copyright (c) SoloX Software. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.Diagnostics;
using System.IO;
using Xunit;

namespace SoloX.CodeQuality.Test.Helpers
{
    /// <summary>
    /// Snapshot helper to test generated file against a reference snapshot.
    /// </summary>
    public static class SnapshotHelper
    {
        /// <summary>
        /// Gets or sets a value indicating whether the snapshot reference files must be overwritten.
        /// </summary>
        public static bool IsOverwriteEnable { get; set; } = true;

        /// <summary>
        /// Gets or sets the snapshot folder name.
        /// </summary>
        public static string SnapshotsFolderName { get; set; } = "Snapshots";

        /// <summary>
        /// Assert that the generated text is the same as the snapshot.
        /// </summary>
        /// <param name="generated">The generated text to match against the snapshot.</param>
        /// <param name="snapshotName">The snapshot name.</param>
        /// <param name="location">Location where to find the snapshots folder.</param>
        public static void AssertSnapshot(string generated, string snapshotName, string location)
        {
            var snapshotFolder = Path.Combine(location, SnapshotsFolderName);
            var snapshotFile = Path.Combine(snapshotFolder, $"{snapshotName}.snapshot");

            if (!Directory.Exists(snapshotFolder) && IsOverwriteEnable)
            {
                Directory.CreateDirectory(snapshotFolder);
            }

            if (!File.Exists(snapshotFile))
            {
                if (IsOverwriteEnable)
                {
                    File.WriteAllText(snapshotFile, generated);
                }

                Assert.Equal($"The snapshot file {snapshotName} does not exist", generated);
            }
            else
            {
                var generatedRef = File.ReadAllText(snapshotFile);

                if (IsOverwriteEnable)
                {
                    File.WriteAllText(snapshotFile, generated);
                }

                Assert.Equal(
                    generatedRef.Replace("\r\n", "\n"),
                    generated.Replace("\r\n", "\n"));
            }
        }

        /// <summary>
        /// Compute the folder location form the calling code assembly project root.
        /// </summary>
        /// <param name="folder">The relative folder.</param>
        /// <returns>Folder location in the project of the calling code assembly.</returns>
        public static string GetLocationFromCallingCodeProjectRoot(string folder)
        {
            var callingAssembly = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Assembly;
            var assemblyFolder = Path.GetDirectoryName(callingAssembly.Location);
            var projectRoot = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assemblyFolder)));
            return folder != null ? Path.Combine(projectRoot, folder) : projectRoot;
        }
    }
}
