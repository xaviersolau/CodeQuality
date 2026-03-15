// ----------------------------------------------------------------------
// <copyright file="ISnapshotTestBuilder.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System.IO;
using System.Runtime.CompilerServices;

namespace SoloX.CodeQuality.Test.Helpers.Snapshot
{
    /// <summary>
    /// Defines a builder for configuring and executing snapshot tests with customizable strategies and locations.
    /// </summary>
    /// <remarks>Implementations of this interface allow fluent configuration of snapshot test parameters,
    /// such as file locations and comparison strategies. Use the provided methods to specify the context and behavior
    /// of the snapshot test before execution. This interface is typically used in test code to set up and verify
    /// snapshot-based assertions.</remarks>
    public interface ISnapshotTestBuilder
    {
        /// <summary>
        /// Specifies the location to use for the snapshot test output.
        /// </summary>
        /// <param name="location">The file system path that determines where the snapshot will be stored. Cannot be null or
        /// empty.</param>
        /// <returns>The current instance of the snapshot test builder with the specified file path location applied.</returns>
        ISnapshotTestBuilder WithLocation(string location);

        /// <summary>
        /// Specifies the location to use for the snapshot test output as the calling .Net file path.
        /// </summary>
        /// <remarks>The location is automaticaly defined from the location of the .Net source file calling this method.</remarks>
        /// <param name="location">The full path of the source file to associate with the snapshot. If not specified, the compiler supplies the
        /// path of the caller's file.</param>
        /// <returns>The current instance of the snapshot test builder with the specified file path location applied.</returns>
        ISnapshotTestBuilder WithThisFilePathLocation([CallerFilePath] string location = default!);

        /// <summary>
        /// Specifies a sub-location to be used for the snapshot.
        /// </summary>
        /// <remarks>Use this method to differentiate snapshots within the same test or logical grouping,
        /// such as when multiple snapshots are needed for different parts of a test case.</remarks>
        /// <param name="subLocation">The sub-location name to associate with the snapshot. Cannot be null or empty.</param>
        /// <returns>The current instance of the snapshot test builder configured with the specified sub-location.</returns>
        ISnapshotTestBuilder WithSubLocation(string subLocation);

        /// <summary>
        /// Configures the snapshot test to use PNG image comparison with an optional differences threshold.
        /// </summary>
        /// <remarks>Use this method to perform visual regression testing by comparing PNG images. Adjust
        /// the differences threshold to allow for minor, acceptable variations between images, such as those caused by
        /// rendering differences.</remarks>
        /// <param name="differencesThreshold">The maximum allowed percentage of pixel differences between images for the test to pass. Must be greater
        /// than or equal to 0.0. A value of 0.0 requires exact matches.</param>
        /// <returns>An updated snapshot test builder configured to use PNG image comparison.</returns>
        ISnapshotTestBuilder<Stream> WithPngStrategy(double differencesThreshold = 0.0);

        /// <summary>
        /// Configures the snapshot test builder to use a text-based comparison strategy.
        /// </summary>
        /// <remarks>Use this method when the expected and actual values should be compared as plain text,
        /// such as for verifying file contents or string outputs.</remarks>
        /// <param name="ignoreWhitespace">Indicates whether to ignore differences in whitespace when comparing text. If true, all whitespace characters
        /// are treated as equivalent, and differences in whitespace will not cause the test to fail. Default is true.</param>
        /// <param name="ignoreCase">Indicates whether to ignore case differences when comparing text. If true, uppercase and lowercase characters
        /// are treated as equivalent, and differences in case will not cause the test to fail. Default is false.</param>
        /// <returns>An instance of the snapshot test builder configured to compare string content using a text strategy.</returns>
        ISnapshotTestBuilder<string> WithTextStrategy(bool ignoreWhitespace = true, bool ignoreCase = false);
    }

    /// <summary>
    /// Defines a builder for creating snapshot test instances with a specific data type.
    /// </summary>
    /// <remarks>Implementations of this interface allow for the construction of snapshot tests that are
    /// strongly typed to the provided data type. This enables type-safe test scenarios and assertions based on the
    /// expected data structure.</remarks>
    /// <typeparam name="TData">The type of data that the snapshot test will operate on.</typeparam>
    public interface ISnapshotTestBuilder<TData> : ISnapshotTestBuilder
    {
        /// <summary>
        /// Builds and returns a configured snapshot test instance for the specified data type.
        /// </summary>
        /// <returns>An instance of ISnapshotTest that can be used to perform snapshot testing with the configured
        /// settings.</returns>
        ISnapshotTest<TData> Build();
    }
}