// ----------------------------------------------------------------------
// <copyright file="SnapshotTestBuilder.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using SoloX.CodeQuality.Test.Helpers.Snapshot.Impl;

namespace SoloX.CodeQuality.Test.Helpers.Snapshot
{
    /// <summary>
    /// Provides factory methods for creating and configuring snapshot test builders used to define snapshot-based
    /// tests.
    /// </summary>
    /// <remarks>This class serves as the entry point for constructing snapshot test builders with various
    /// configuration options and snapshot strategies. It is intended for use in test code to facilitate the setup of
    /// snapshot testing scenarios. The class is static and cannot be instantiated.</remarks>
    public static class SnapshotTestBuilder
    {
        public const string DefaultSubLocation = "Snapshots";

        /// <summary>
        /// Creates a new instance of a snapshot test builder.
        /// </summary>
        /// <returns>An object that implements the ISnapshotTestBuilder interface for configuring and executing snapshot tests.</returns>
        public static ISnapshotTestBuilder Create()
        {
            return new SnapshotTestBuilderInternal();
        }

        private class SnapshotTestBuilderInternal : ISnapshotTestBuilder
        {
            protected string? location;
            protected string subLocation = DefaultSubLocation;

            public SnapshotTestBuilderInternal()
            {
            }

            public SnapshotTestBuilderInternal(SnapshotTestBuilderInternal snapshotTestBuilder)
            {
                this.location = snapshotTestBuilder.location;
                this.subLocation = snapshotTestBuilder.subLocation;
            }

            public ISnapshotTestBuilder WithLocation(string location)
            {
                this.location = location;

                return this;
            }

            public ISnapshotTestBuilder WithThisFilePathLocation([CallerFilePath] string location = default!)
            {
                this.location = Path.GetDirectoryName(location);

                return this;
            }

            public ISnapshotTestBuilder WithSubLocation(string subLocation)
            {
                this.subLocation = subLocation;

                return this;
            }

            public ISnapshotTestBuilder<Stream> WithPngStrategy(double differencesThreshold = 0.0)
            {
                var builder = new SnapshotTestBuilderInternal<Stream>(this, new PngSnapshotStrategy(differencesThreshold));

                return builder;
            }

            public ISnapshotTestBuilder<string> WithUtf8TextStrategy(bool ignoreWhitespace = true, bool ignoreCase = false)
            {
                return WithTextStrategy(ignoreWhitespace, ignoreCase, Encoding.UTF8);
            }

            public ISnapshotTestBuilder<string> WithTextStrategy(bool ignoreWhitespace = true, bool ignoreCase = false, Encoding? encoding = null)
            {
                var builder = new SnapshotTestBuilderInternal<string>(this, new TextSnapshotStrategy(ignoreWhitespace, ignoreCase, encoding));

                return builder;
            }
        }

        private sealed class SnapshotTestBuilderInternal<TData> : SnapshotTestBuilderInternal, ISnapshotTestBuilder<TData>
        {
            private readonly ISnapshotStrategy<TData> strategy;

            public SnapshotTestBuilderInternal(SnapshotTestBuilderInternal snapshotTestBuilder, ISnapshotStrategy<TData> strategy)
                : base(snapshotTestBuilder)
            {
                this.strategy = strategy;
            }

            public ISnapshotTest<TData> Build()
            {
                if (string.IsNullOrEmpty(this.location))
                {
                    throw new InvalidOperationException("Location must be set");
                }

                var snapshotsFolder = string.IsNullOrWhiteSpace(this.subLocation) ? this.location : Path.Combine(this.location, this.subLocation);

                return new SnapshotTest<TData>(this.strategy, snapshotsFolder);
            }
        }
    }
}