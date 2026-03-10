// ----------------------------------------------------------------------
// <copyright file="SnapshotHelperOptions.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.CodeQuality.Test.Helpers.Snapshot
{
    /// <summary>
    /// Provides configuration options for managing snapshot storage locations used by the snapshot helper.
    /// </summary>
    /// <remarks>Use this class to specify custom paths for the root directory, intermediate folder, and
    /// snapshots folder. If not set, the SnapshotsFolder property defaults to a predefined folder. These options allow
    /// for flexible organization of snapshot files in testing or code quality scenarios.</remarks>
    public class SnapshotHelperOptions
    {
        /// <summary>
        /// Gets or sets the root path for the application, which defines the base directory for file operations.
        /// </summary>
        /// <remarks>This property can be set to a valid directory path. If set to null, the application
        /// will use a default root path. Ensure that the specified path exists and is accessible to avoid runtime
        /// errors.</remarks>
        public string? RootPath { get; set; }

        /// <summary>
        /// Gets or sets the path to the intermediate folder used for build outputs.
        /// </summary>
        /// <remarks>This property allows customization of the intermediate folder location, which can be
        /// useful for organizing build artifacts or for integration with specific build systems.</remarks>
        public string? IntermediateFolder { get; set; }

        /// <summary>
        /// Gets or sets the folder path where snapshots are stored.
        /// </summary>
        /// <remarks>The default value is determined by SnapshotHelper.DefaultFolder.</remarks>
        public string SnapshotsFolder { get; set; } = SnapshotHelper.DefaultFolder;
    }
}
