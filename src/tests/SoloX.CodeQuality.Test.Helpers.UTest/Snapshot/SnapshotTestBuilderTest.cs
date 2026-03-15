// ----------------------------------------------------------------------
// <copyright file="SnapshotTestBuilderTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Shouldly;
using SoloX.CodeQuality.Test.Helpers.Snapshot;
using Xunit;

namespace SoloX.CodeQuality.Test.Helpers.UTest.Snapshot
{
    public class SnapshotTestBuilderTest
    {
        [Fact]
        public async Task ItShouldGenerateTextSnapshotAsync()
        {
            var sh = SnapshotTestBuilder
                .Create()
                .WithLocation(".")
                .WithTextStrategy()
                .Build();

            var expectedFile = @$"./Snapshots/{nameof(ItShouldGenerateTextSnapshotAsync)}.snapshot.ref.txt";

            try
            {
                var someGeneratedText = "some generated text";

                await sh.CompareSnapshotAsync(nameof(ItShouldGenerateTextSnapshotAsync), someGeneratedText);

                // Check that the snapshot reference file exists and has been generated with the same content as the generated text
                File.Exists(expectedFile)
                    .ShouldBeTrue();

                var generatedText = await File.ReadAllTextAsync(expectedFile);

                generatedText.ShouldBe(someGeneratedText);
            }
            finally
            {
                if (File.Exists(expectedFile))
                {
                    File.Delete(expectedFile);
                }
            }
        }

        [Fact]
        public async Task ItShouldReplaceTextSnapshotAsync()
        {
            var snapshotRefFile = @$"./Snapshots/{nameof(ItShouldReplaceTextSnapshotAsync)}.snapshot.ref.txt";

            try
            {
                var sh = SnapshotTestBuilder
                    .Create()
                    .WithLocation(".")
                    .WithTextStrategy()
                    .Build();

                Directory.CreateDirectory("./Snapshots");

                await File.WriteAllTextAsync(snapshotRefFile, "some snapshot text");

                var newGeneratedText = "some generated text";

                await sh.CompareSnapshotAsync(nameof(ItShouldReplaceTextSnapshotAsync), newGeneratedText, forceReplaceSnapshot: true);

                // Check that the snapshot reference file has been replaced with the new generated text
                var newSnapshotRefText = await File.ReadAllTextAsync(snapshotRefFile);

                newSnapshotRefText.ShouldBe(newGeneratedText);
            }
            finally
            {
                if (File.Exists(snapshotRefFile))
                {
                    File.Delete(snapshotRefFile);
                }
            }
        }

        [Fact]
        public async Task ItShouldMatchTextSnapshotAsync()
        {
            var sh = SnapshotTestBuilder
                .Create()
                .WithThisFilePathLocation()
                .WithTextStrategy()
                .Build();

            var someGeneratedText = "some generated text";

            await sh.CompareSnapshotAsync(nameof(ItShouldMatchTextSnapshotAsync), someGeneratedText);
        }

        [Fact]
        public async Task ItShouldNotMatchTextSnapshotAsync()
        {
            var expectedRunFile = @$"../../../Snapshot/Snapshots/{nameof(ItShouldNotMatchTextSnapshotAsync)}.snapshot.run.txt";
            var expectedDiffsFile = @$"../../../Snapshot/Snapshots/{nameof(ItShouldNotMatchTextSnapshotAsync)}.snapshot.diffs.txt";

            try
            {
                var sh = SnapshotTestBuilder
                    .Create()
                    .WithThisFilePathLocation()
                    .WithTextStrategy()
                    .Build();

                var someGeneratedText = "not matching generated text";

                var snapshotException = await Should.ThrowAsync<SnapshotTestException>(
                    sh.CompareSnapshotAsync(nameof(ItShouldNotMatchTextSnapshotAsync), someGeneratedText));

                snapshotException.Message.ShouldBe(@"--- Snapshot reference
+++ Snapshot run
@@ -1,1 +1,1 @@
-some generated text
+not matching generated text
");

                File.Exists(expectedRunFile)
                    .ShouldBeTrue();

                File.Exists(expectedDiffsFile)
                    .ShouldBeTrue();
            }
            finally
            {
                if (File.Exists(expectedRunFile))
                {
                    File.Delete(expectedRunFile);
                }
                if (File.Exists(expectedDiffsFile))
                {
                    File.Delete(expectedDiffsFile);
                }
            }
        }

        [Fact]
        public async Task ItShouldGeneratePngSnapshotAsync()
        {
            var expectedFile = @$"./Snapshots/{nameof(ItShouldGeneratePngSnapshotAsync)}.snapshot.ref.png";

            try
            {
                var sh = SnapshotTestBuilder
                    .Create()
                    .WithLocation(".")
                    .WithPngStrategy()
                    .Build();

                var pngSourcePath = @"Resources/small_mountain_land_scape.png";
                await using var someGeneratedPng = File.OpenRead(pngSourcePath);

                await sh.CompareSnapshotAsync(nameof(ItShouldGeneratePngSnapshotAsync), someGeneratedPng);

                // Check that the snapshot reference file exists and has been generated with the same content as the png source path
                File.Exists(expectedFile)
                    .ShouldBeTrue();

                var generatedPngBytes = await File.ReadAllBytesAsync(expectedFile);
                var expectedGeneratedPngBytes = await File.ReadAllBytesAsync(pngSourcePath);

                generatedPngBytes.ShouldBeEquivalentTo(expectedGeneratedPngBytes);
            }
            finally
            {
                if (File.Exists(expectedFile))
                {
                    File.Delete(expectedFile);
                }
            }
        }

        [Fact]
        public async Task ItShouldReplacePngSnapshotAsync()
        {
            var snapshotRefFile = @$"./Snapshots/{nameof(ItShouldReplacePngSnapshotAsync)}.snapshot.ref.png";

            try
            {
                var sh = SnapshotTestBuilder
                    .Create()
                    .WithLocation(".")
                    .WithPngStrategy()
                    .Build();

                var pngSourcePath = @"Resources/small_mountain_land_scape.png";

                Directory.CreateDirectory("./Snapshots");

                File.Copy(pngSourcePath, snapshotRefFile);

                var pngSourcePathDiffsPath = @"Resources/small_mountain_land_scape_diffs.png";
                await using var someGeneratedPng = File.OpenRead(pngSourcePathDiffsPath);

                await sh.CompareSnapshotAsync(nameof(ItShouldReplacePngSnapshotAsync), someGeneratedPng, forceReplaceSnapshot: true);

                // Check that the snapshot reference file has been replaced with the new png source path diffs file content
                var newSnapshotRefBytes = await File.ReadAllBytesAsync(snapshotRefFile);
                var expectedPngBytes = await File.ReadAllBytesAsync(pngSourcePathDiffsPath);

                newSnapshotRefBytes.ShouldBeEquivalentTo(expectedPngBytes);
            }
            finally
            {
                if (File.Exists(snapshotRefFile))
                {
                    File.Delete(snapshotRefFile);
                }
            }
        }

        [Theory]
        [InlineData("exact_match", @"Resources/small_mountain_land_scape.png", 0.0)]
        [InlineData("threshold_match", @"Resources/small_mountain_land_scape_diffs.png", 0.2)]
        public async Task ItShouldMatchPngSnapshotAsync(string testName, string pngSourcePath, double threshold)
        {
            var sh = SnapshotTestBuilder
                .Create()
                .WithThisFilePathLocation()
                .WithPngStrategy(threshold)
                .Build();

            await using var someGeneratedPng = File.OpenRead(pngSourcePath);

            await sh.CompareSnapshotAsync(
                $"{nameof(ItShouldMatchPngSnapshotAsync)}_{testName}",
                someGeneratedPng);
        }

        [Fact]
        public async Task ItShouldNotMatchPngSnapshotAsync()
        {
            var expectedRunFile = @$"../../../Snapshot/Snapshots/{nameof(ItShouldNotMatchPngSnapshotAsync)}.snapshot.run.png";
            var expectedDiffsFile = @$"../../../Snapshot/Snapshots/{nameof(ItShouldNotMatchPngSnapshotAsync)}.snapshot.diffs.png";

            try
            {
                var sh = SnapshotTestBuilder
                    .Create()
                    .WithThisFilePathLocation()
                    .WithPngStrategy()
                    .Build();

                await using var someGeneratedPng = File.OpenRead(@"Resources/small_mountain_land_scape_diffs.png");

                var snapshotException = await Should.ThrowAsync<SnapshotTestException>(
                    sh.CompareSnapshotAsync(nameof(ItShouldNotMatchPngSnapshotAsync), someGeneratedPng));

                snapshotException.Message.ShouldBe("See Png Diffs file (deltas 0.08724384654082065 <= threshold 0)");

                File.Exists(expectedRunFile)
                    .ShouldBeTrue();

                File.Exists(expectedDiffsFile)
                    .ShouldBeTrue();

                var generatedDiffsBytes = await File.ReadAllBytesAsync(expectedDiffsFile);
                var expectedDiffsBytes = await File.ReadAllBytesAsync(@"Resources/small_mountain_land_scape.snapshot.diffs.png");

                generatedDiffsBytes.ShouldBeEquivalentTo(expectedDiffsBytes);
            }
            finally
            {
                if (File.Exists(expectedRunFile))
                {
                    File.Delete(expectedRunFile);
                }
                if (File.Exists(expectedDiffsFile))
                {
                    File.Delete(expectedDiffsFile);
                }
            }
        }
    }
}
