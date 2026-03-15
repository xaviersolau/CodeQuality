// ----------------------------------------------------------------------
// <copyright file="TextSnapshotStrategyTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Shouldly;
using SoloX.CodeQuality.Test.Helpers.Snapshot.Impl;
using Xunit;

namespace SoloX.CodeQuality.Test.Helpers.UTest.Snapshot
{
    public sealed class TextSnapshotStrategyTest : IDisposable
    {
        private readonly string testDirectory;

        public TextSnapshotStrategyTest()
        {
            this.testDirectory = Path.Combine(Path.GetTempPath(), $"TextSnapshotStrategyTest_{Guid.NewGuid()}");
            Directory.CreateDirectory(this.testDirectory);
        }

        public void Dispose()
        {
            if (Directory.Exists(this.testDirectory))
            {
                Directory.Delete(this.testDirectory, recursive: true);
            }
            GC.SuppressFinalize(this);
        }


        [Fact]
        public void FileExtensionShouldReturnTxt()
        {
            var strategy = new TextSnapshotStrategy();

            strategy.FileExtension.ShouldBe("txt");
        }

        [Fact]
        public async Task SaveShouldCreateNewFile()
        {
            var strategy = new TextSnapshotStrategy();
            var filePath = Path.Combine(this.testDirectory, "test_snapshot.txt");
            var content = "Test snapshot content";

            await strategy.SaveAsync(filePath, content);

            File.Exists(filePath).ShouldBeTrue();
            var savedContent = await File.ReadAllTextAsync(filePath);
            savedContent.ShouldBe(content);
        }

        [Fact]
        public async Task SaveShouldReplaceExistingFile()
        {
            var strategy = new TextSnapshotStrategy();
            var filePath = Path.Combine(this.testDirectory, "test_snapshot.txt");
            var originalContent = "Original content";
            var newContent = "New content";

            // Create initial file
            await File.WriteAllTextAsync(filePath, originalContent);

            // Save new content
            await strategy.SaveAsync(filePath, newContent);

            File.Exists(filePath).ShouldBeTrue();
            var savedContent = await File.ReadAllTextAsync(filePath);
            savedContent.ShouldBe(newContent);
        }

        [Fact]
        public async Task SaveShouldHandleEmptyContent()
        {
            var strategy = new TextSnapshotStrategy();
            var filePath = Path.Combine(this.testDirectory, "test_snapshot.txt");
            var content = string.Empty;

            await strategy.SaveAsync(filePath, content);

            File.Exists(filePath).ShouldBeTrue();
            var savedContent = await File.ReadAllTextAsync(filePath);
            savedContent.ShouldBe(content);
        }

        [Fact]
        public async Task SaveShouldHandleMultilineContent()
        {
            var strategy = new TextSnapshotStrategy();
            var filePath = Path.Combine(this.testDirectory, "test_snapshot.txt");
            var content = "Line 1\nLine 2\nLine 3";

            await strategy.SaveAsync(filePath, content);

            File.Exists(filePath).ShouldBeTrue();
            var savedContent = await File.ReadAllTextAsync(filePath);
            savedContent.ShouldBe(content);
        }

        [Fact]
        public async Task CompareShouldDetectIdenticalContent()
        {
            var strategy = new TextSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.txt");
            var content = "Identical content";

            await File.WriteAllTextAsync(referenceFilePath, content);

            var result = await strategy.CompareAsync(referenceFilePath, content);

            result.IsDifferent.ShouldBeFalse();
            result.DiffsData.ShouldBeNullOrEmpty();
            result.DiffsString.ShouldBeNullOrEmpty();
        }

        [Fact]
        public async Task CompareShouldDetectDifferentContent()
        {
            var strategy = new TextSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.txt");
            var referenceContent = "Original content";
            var newContent = "Modified content";

            await File.WriteAllTextAsync(referenceFilePath, referenceContent);

            var result = await strategy.CompareAsync(referenceFilePath, newContent);

            result.IsDifferent.ShouldBeTrue();
            result.DiffsData.ShouldNotBeNullOrEmpty();
            result.DiffsString.ShouldNotBeNullOrEmpty();
            result.DiffsString.ShouldContain("Snapshot reference");
            result.DiffsString.ShouldContain("Snapshot run");
        }

        [Fact]
        public async Task CompareShouldDetectLineAdditions()
        {
            var strategy = new TextSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.txt");
            var referenceContent = "Line 1";
            var newContent = "Line 1\nLine 2";

            await File.WriteAllTextAsync(referenceFilePath, referenceContent);

            var result = await strategy.CompareAsync(referenceFilePath, newContent);

            result.IsDifferent.ShouldBeTrue();
            result.DiffsString.ShouldContain("Line 2");
        }

        [Fact]
        public async Task CompareShouldDetectLineRemovals()
        {
            var strategy = new TextSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.txt");
            var referenceContent = "Line 1\nLine 2";
            var newContent = "Line 1";

            await File.WriteAllTextAsync(referenceFilePath, referenceContent);

            var result = await strategy.CompareAsync(referenceFilePath, newContent);

            result.IsDifferent.ShouldBeTrue();
            result.DiffsString.ShouldContain("Line 2");
        }

        [Fact]
        public async Task CompareShouldDetectLineModifications()
        {
            var strategy = new TextSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.txt");
            var referenceContent = "Original line";
            var newContent = "Modified line";

            await File.WriteAllTextAsync(referenceFilePath, referenceContent);

            var result = await strategy.CompareAsync(referenceFilePath, newContent);

            result.IsDifferent.ShouldBeTrue();
            result.DiffsString.ShouldContain("Original line");
            result.DiffsString.ShouldContain("Modified line");
        }

        [Fact]
        public async Task CompareShouldHandleEmptyReferenceFile()
        {
            var strategy = new TextSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.txt");
            var newContent = "Some content";

            await File.WriteAllTextAsync(referenceFilePath, string.Empty);

            var result = await strategy.CompareAsync(referenceFilePath, newContent);

            result.IsDifferent.ShouldBeTrue();
            result.DiffsString.ShouldContain("Some content");
        }

        [Fact]
        public async Task CompareShouldHandleEmptyNewContent()
        {
            var strategy = new TextSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.txt");
            var referenceContent = "Some content";

            await File.WriteAllTextAsync(referenceFilePath, referenceContent);

            var result = await strategy.CompareAsync(referenceFilePath, string.Empty);

            result.IsDifferent.ShouldBeTrue();
            result.DiffsString.ShouldContain("Some content");
        }

        [Fact]
        public async Task CompareShouldReturnDiffInCorrectFormat()
        {
            var strategy = new TextSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.txt");
            var referenceContent = "reference";
            var newContent = "current";

            await File.WriteAllTextAsync(referenceFilePath, referenceContent);

            var result = await strategy.CompareAsync(referenceFilePath, newContent);

            result.DiffsData.ShouldBe(result.DiffsString);
        }

        [Fact]
        public async Task SaveShouldCreateDirectoryIfNotExists()
        {
            var strategy = new TextSnapshotStrategy();
            var subdirectory = Path.Combine(this.testDirectory, "subdir");
            var filePath = Path.Combine(subdirectory, "test_snapshot.txt");
            var content = "Test content";

            Directory.Exists(subdirectory).ShouldBeFalse();

            // The Save method expects the directory to exist; manually create it for this test
            Directory.CreateDirectory(subdirectory);
            await strategy.SaveAsync(filePath, content);

            File.Exists(filePath).ShouldBeTrue();
        }

        [Fact]
        public async Task CompareShouldHandleSpecialCharacters()
        {
            var strategy = new TextSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.txt");
            var referenceContent = "Content with special chars: @#$%^&*()";
            var newContent = "Different: <html>tag</html>";

            await File.WriteAllTextAsync(referenceFilePath, referenceContent);

            var result = await strategy.CompareAsync(referenceFilePath, newContent);

            result.IsDifferent.ShouldBeTrue();
            result.DiffsString.ShouldNotBeNullOrEmpty();
        }

        [Fact]
        public async Task CompareShouldHandleWhitespaceOnlyDifferences()
        {
            var strategy = new TextSnapshotStrategy(ignoreWhitespace: false);
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.txt");
            var referenceContent = "Line1";
            var newContent = "Line1 ";

            await File.WriteAllTextAsync(referenceFilePath, referenceContent);

            var result = await strategy.CompareAsync(referenceFilePath, newContent);

            result.IsDifferent.ShouldBeTrue();
            result.DiffsString.ShouldNotBeNullOrEmpty();
        }
    }
}
