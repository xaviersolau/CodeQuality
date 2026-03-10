// ----------------------------------------------------------------------
// <copyright file="SnapshotHelperTest.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Shouldly;
using SoloX.CodeQuality.Test.Helpers.Snapshot;
using Xunit;

namespace SoloX.CodeQuality.Test.Helpers.UTest
{
    public class SnapshotHelperTest
    {
        [Fact]
        public async Task ItShouldGenerateTextSnapshotAsync()
        {
            var sh = new SnapshotHelper(cfg => cfg.RootPath = ".");

            var expectedFile = @$"./Snapshots/{nameof(ItShouldGenerateTextSnapshotAsync)}.snapshot.ref.txt";

            try
            {
                var someGeneratedText = "some generated text";

                await sh.CompareTextSnapshotAsync(nameof(ItShouldGenerateTextSnapshotAsync), someGeneratedText);

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
                var sh = new SnapshotHelper(cfg => cfg.RootPath = ".");

                Directory.CreateDirectory("./Snapshots");

                await File.WriteAllTextAsync(snapshotRefFile, "some snapshot text");

                var newGeneratedText = "some generated text";

                await sh.CompareTextSnapshotAsync(nameof(ItShouldReplaceTextSnapshotAsync), newGeneratedText, forceReplaceSnapshot: true);

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
            var sh = new SnapshotHelper();

            var someGeneratedText = "some generated text";

            await sh.CompareTextSnapshotAsync(nameof(ItShouldMatchTextSnapshotAsync), someGeneratedText);
        }

        [Fact]
        public async Task ItShouldNotMatchTextSnapshotAsync()
        {
            var expectedRunFile = @$"../../../Snapshots/{nameof(ItShouldNotMatchTextSnapshotAsync)}.snapshot.run.txt";
            var expectedDiffsFile = @$"../../../Snapshots/{nameof(ItShouldNotMatchTextSnapshotAsync)}.snapshot.diffs.txt";

            try
            {
                var sh = new SnapshotHelper();

                var someGeneratedText = "not matching generated text";

                var snapshotException = await Should.ThrowAsync<SnapshotException>(
                    sh.CompareTextSnapshotAsync(nameof(ItShouldNotMatchTextSnapshotAsync), someGeneratedText));

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
                var sh = new SnapshotHelper(cfg => cfg.RootPath = ".");

                var pngSourcePath = @"Resources/small_mountain_land_scape.png";
                await using var someGeneratedPng = File.OpenRead(pngSourcePath);

                await sh.ComparePngSnapshotAsync(nameof(ItShouldGeneratePngSnapshotAsync), someGeneratedPng);

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
                var sh = new SnapshotHelper(cfg => cfg.RootPath = ".");

                var pngSourcePath = @"Resources/small_mountain_land_scape.png";

                Directory.CreateDirectory("./Snapshots");

                File.Copy(pngSourcePath, snapshotRefFile);

                var pngSourcePathDiffsPath = @"Resources/small_mountain_land_scape_diffs.png";
                await using var someGeneratedPng = File.OpenRead(pngSourcePathDiffsPath);

                await sh.ComparePngSnapshotAsync(nameof(ItShouldReplacePngSnapshotAsync), someGeneratedPng, forceReplaceSnapshot: true);

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
            var sh = new SnapshotHelper();

            await using var someGeneratedPng = File.OpenRead(pngSourcePath);

            await sh.ComparePngSnapshotAsync(
                $"{nameof(ItShouldMatchPngSnapshotAsync)}_{testName}",
                someGeneratedPng,
                differencesThreshold: threshold);
        }

        [Fact]
        public async Task ItShouldNotMatchPngSnapshotAsync()
        {
            var expectedRunFile = @$"../../../Snapshots/{nameof(ItShouldNotMatchPngSnapshotAsync)}.snapshot.run.png";
            var expectedDiffsFile = @$"../../../Snapshots/{nameof(ItShouldNotMatchPngSnapshotAsync)}.snapshot.diffs.png";

            try
            {
                var sh = new SnapshotHelper();

                await using var someGeneratedPng = File.OpenRead(@"Resources/small_mountain_land_scape_diffs.png");

                var snapshotException = await Should.ThrowAsync<SnapshotException>(
                    sh.ComparePngSnapshotAsync(nameof(ItShouldNotMatchPngSnapshotAsync), someGeneratedPng));

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
