// ----------------------------------------------------------------------
// <copyright file="PngSnapshotStrategyTest.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using Shouldly;
using SoloX.CodeQuality.Test.Helpers.Snapshot.Impl;
using SkiaSharp;
using Xunit;

namespace SoloX.CodeQuality.Test.Helpers.UTest.Snapshot
{
    public sealed class PngSnapshotStrategyTest : IDisposable
    {
        private readonly string testDirectory;

        public PngSnapshotStrategyTest()
        {
            this.testDirectory = Path.Combine(Path.GetTempPath(), $"PngSnapshotStrategyTest_{Guid.NewGuid()}");
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
        public void FileExtensionShouldReturnPng()
        {
            var strategy = new PngSnapshotStrategy();

            strategy.FileExtension.ShouldBe("png");
        }

        [Fact]
        public void ConstructorShouldAcceptDifferencesThreshold()
        {
            var strategy = new PngSnapshotStrategy(0.1);

            strategy.ShouldNotBeNull();
        }

        [Fact]
        public async Task SaveShouldCreatePngFile()
        {
            var strategy = new PngSnapshotStrategy();
            var filePath = Path.Combine(this.testDirectory, "test_snapshot.png");

            using var bitmap = new SKBitmap(10, 10);
            using var canvas = new SKCanvas(bitmap);
            canvas.DrawColor(SKColors.Red);

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            var stream = new MemoryStream();
            await using var _ = stream.ConfigureAwait(false);
            data.SaveTo(stream);
            stream.Seek(0, SeekOrigin.Begin);
            await strategy.SaveAsync(filePath, stream);

            File.Exists(filePath).ShouldBeTrue();
            var fileSize = new FileInfo(filePath).Length;
            fileSize.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task SaveShouldReplaceExistingFile()
        {
            var strategy = new PngSnapshotStrategy();
            var filePath = Path.Combine(this.testDirectory, "test_snapshot.png");

            // Create initial file
            using (var bitmap1 = new SKBitmap(10, 10))
            {
                using var canvas1 = new SKCanvas(bitmap1);
                canvas1.DrawColor(SKColors.Red);
                using var image1 = SKImage.FromBitmap(bitmap1);
                using var data1 = image1.Encode(SKEncodedImageFormat.Png, 100);

                var stream = new MemoryStream();
                await using var _1 = stream.ConfigureAwait(false);
                data1.SaveTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await strategy.SaveAsync(filePath, stream);
            }

            // Save different image
            using (var bitmap2 = new SKBitmap(20, 20))
            {
                using var canvas2 = new SKCanvas(bitmap2);
                canvas2.DrawColor(SKColors.Blue);
                using var image2 = SKImage.FromBitmap(bitmap2);
                using var data2 = image2.Encode(SKEncodedImageFormat.Png, 100);

                var stream = new MemoryStream();
                await using var _2 = stream.ConfigureAwait(false);
                data2.SaveTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await strategy.SaveAsync(filePath, stream);
            }

            File.Exists(filePath).ShouldBeTrue();
        }

        [Fact]
        public async Task SaveShouldHandleStreamWithPosition()
        {
            var strategy = new PngSnapshotStrategy();
            var filePath = Path.Combine(this.testDirectory, "test_snapshot.png");

            using var bitmap = new SKBitmap(10, 10);
            using var canvas = new SKCanvas(bitmap);
            canvas.DrawColor(SKColors.Green);
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);

            var stream = new MemoryStream();
            data.SaveTo(stream);
            stream.Position = 10; // Set position to non-zero

            await strategy.SaveAsync(filePath, stream);

            File.Exists(filePath).ShouldBeTrue();

            await stream.DisposeAsync().ConfigureAwait(false);
        }

        [Fact]
        public async Task SaveShouldThrowForNonSeekableStream()
        {
            var strategy = new PngSnapshotStrategy();
            var filePath = Path.Combine(this.testDirectory, "test_snapshot.png");

            // Create a non-seekable stream
            using var mockStream = new NonSeekableStream();
            mockStream.Position = 5; // Position > 0

            await Should.ThrowAsync<InvalidOperationException>(
                () => strategy.SaveAsync(filePath, mockStream)).ConfigureAwait(false);
        }

        [Fact]
        public async Task CompareShouldDetectIdenticalImages()
        {
            var strategy = new PngSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.png");

            // Create reference image
            using (var bitmap = new SKBitmap(10, 10))
            {
                using var canvas = new SKCanvas(bitmap);
                canvas.DrawColor(SKColors.Red);
                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                var stream = new MemoryStream();
                await using var _1 = stream.ConfigureAwait(false);
                data.SaveTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await strategy.SaveAsync(referenceFilePath, stream);
            }

            // Compare with identical image
            using (var bitmap2 = new SKBitmap(10, 10))
            {
                using var canvas2 = new SKCanvas(bitmap2);
                canvas2.DrawColor(SKColors.Red);
                using var image2 = SKImage.FromBitmap(bitmap2);
                using var data2 = image2.Encode(SKEncodedImageFormat.Png, 100);

                var compareStream = new MemoryStream();
                data2.SaveTo(compareStream);
                compareStream.Seek(0, SeekOrigin.Begin);

                var result = await strategy.CompareAsync(referenceFilePath, compareStream).ConfigureAwait(false);

                result.IsDifferent.ShouldBeFalse();
                result.DiffsData.ShouldBeNull();

                await compareStream.DisposeAsync().ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task CompareShouldDetectDifferentImages()
        {
            var strategy = new PngSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.png");

            // Create reference image
            using (var bitmap = new SKBitmap(10, 10))
            {
                using var canvas = new SKCanvas(bitmap);
                canvas.DrawColor(SKColors.Red);
                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                var stream = new MemoryStream();
                await using var _1 = stream.ConfigureAwait(false);
                data.SaveTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await strategy.SaveAsync(referenceFilePath, stream);
            }

            // Compare with different image
            using (var bitmap2 = new SKBitmap(10, 10))
            {
                using var canvas2 = new SKCanvas(bitmap2);
                canvas2.DrawColor(SKColors.Blue);
                using var image2 = SKImage.FromBitmap(bitmap2);
                using var data2 = image2.Encode(SKEncodedImageFormat.Png, 100);

                var compareStream = new MemoryStream();
                data2.SaveTo(compareStream);
                compareStream.Seek(0, SeekOrigin.Begin);

                var result = await strategy.CompareAsync(referenceFilePath, compareStream).ConfigureAwait(false);

                result.IsDifferent.ShouldBeTrue();
                result.DiffsData.ShouldNotBeNull();
                result.DiffsString.ShouldContain("See Png Diffs file");

                await compareStream.DisposeAsync().ConfigureAwait(false);
                if (result.DiffsData is not null)
                {
                    await result.DiffsData.DisposeAsync().ConfigureAwait(false);
                }
            }
        }

        [Fact]
        public async Task CompareShouldDetectImageSizeDifferences()
        {
            var strategy = new PngSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.png");

            // Create reference image (10x10)
            using (var bitmap = new SKBitmap(10, 10))
            {
                using var canvas = new SKCanvas(bitmap);
                canvas.DrawColor(SKColors.Red);
                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                var stream = new MemoryStream();
                await using var _1 = stream.ConfigureAwait(false);
                data.SaveTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await strategy.SaveAsync(referenceFilePath, stream);
            }

            // Compare with different size image (20x20)
            using (var bitmap2 = new SKBitmap(20, 20))
            {
                using var canvas2 = new SKCanvas(bitmap2);
                canvas2.DrawColor(SKColors.Red);
                using var image2 = SKImage.FromBitmap(bitmap2);
                using var data2 = image2.Encode(SKEncodedImageFormat.Png, 100);

                var compareStream = new MemoryStream();
                data2.SaveTo(compareStream);
                compareStream.Seek(0, SeekOrigin.Begin);

                var result = await strategy.CompareAsync(referenceFilePath, compareStream).ConfigureAwait(false);

                result.IsDifferent.ShouldBeTrue();
                result.DiffsData.ShouldNotBeNull();

                await compareStream.DisposeAsync().ConfigureAwait(false);
                if (result.DiffsData is not null)
                {
                    await result.DiffsData.DisposeAsync().ConfigureAwait(false);
                }
            }
        }

        [Fact]
        public async Task CompareShouldRespectDifferencesThreshold()
        {
            var strategy = new PngSnapshotStrategy(differencesThreshold: 0.5);
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.png");

            // Create reference image (mostly red)
            using (var bitmap = new SKBitmap(10, 10))
            {
                using var canvas = new SKCanvas(bitmap);
                canvas.DrawColor(SKColors.Red);
                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                var stream = new MemoryStream();
                await using var _1 = stream.ConfigureAwait(false);
                data.SaveTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await strategy.SaveAsync(referenceFilePath, stream);
            }

            // Create slightly different image
            using (var bitmap2 = new SKBitmap(10, 10))
            {
                using var canvas2 = new SKCanvas(bitmap2);
                canvas2.DrawColor(SKColors.Red);
                using var paint = new SKPaint { Color = SKColors.Blue };
                canvas2.DrawRect(new SKRect(0, 0, 2, 2), paint);
                using var image2 = SKImage.FromBitmap(bitmap2);
                using var data2 = image2.Encode(SKEncodedImageFormat.Png, 100);

                var compareStream = new MemoryStream();
                data2.SaveTo(compareStream);
                compareStream.Seek(0, SeekOrigin.Begin);

                var result = await strategy.CompareAsync(referenceFilePath, compareStream).ConfigureAwait(false);

                result.DiffsString.ShouldContain("See Png Diffs file");

                await compareStream.DisposeAsync().ConfigureAwait(false);
                if (result.DiffsData is not null)
                {
                    await result.DiffsData.DisposeAsync().ConfigureAwait(false);
                }
            }
        }

        [Fact]
        public async Task CompareShouldReturnDiffImageStream()
        {
            var strategy = new PngSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.png");

            // Create reference image
            using (var bitmap = new SKBitmap(10, 10))
            {
                using var canvas = new SKCanvas(bitmap);
                canvas.DrawColor(SKColors.Red);
                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                var stream = new MemoryStream();
                await using var _1 = stream.ConfigureAwait(false);
                data.SaveTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await strategy.SaveAsync(referenceFilePath, stream);
            }

            // Compare with different image
            using (var bitmap2 = new SKBitmap(10, 10))
            {
                using var canvas2 = new SKCanvas(bitmap2);
                canvas2.DrawColor(SKColors.Blue);
                using var image2 = SKImage.FromBitmap(bitmap2);
                using var data2 = image2.Encode(SKEncodedImageFormat.Png, 100);

                var compareStream = new MemoryStream();
                data2.SaveTo(compareStream);
                compareStream.Seek(0, SeekOrigin.Begin);

                var result = await strategy.CompareAsync(referenceFilePath, compareStream).ConfigureAwait(false);

                result.DiffsData.ShouldNotBeNull();
                result.DiffsData.Position.ShouldBe(0);
                result.DiffsData.Length.ShouldBeGreaterThan(0);

                await compareStream.DisposeAsync().ConfigureAwait(false);
                if (result.DiffsData is not null)
                {
                    await result.DiffsData.DisposeAsync().ConfigureAwait(false);
                }
            }
        }

        [Fact]
        public async Task CompareShouldHandleStreamAtPosition()
        {
            var strategy = new PngSnapshotStrategy();
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.png");

            // Create reference image
            using (var bitmap = new SKBitmap(10, 10))
            {
                using var canvas = new SKCanvas(bitmap);
                canvas.DrawColor(SKColors.Red);
                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                var stream = new MemoryStream();
                await using var _1 = stream.ConfigureAwait(false);
                data.SaveTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await strategy.SaveAsync(referenceFilePath, stream);
            }

            // Compare with identical image but stream at non-zero position
            using (var bitmap2 = new SKBitmap(10, 10))
            {
                using var canvas2 = new SKCanvas(bitmap2);
                canvas2.DrawColor(SKColors.Red);
                using var image2 = SKImage.FromBitmap(bitmap2);
                using var data2 = image2.Encode(SKEncodedImageFormat.Png, 100);

                var compareStream = new MemoryStream();
                data2.SaveTo(compareStream);

                var result = await strategy.CompareAsync(referenceFilePath, compareStream).ConfigureAwait(false);

                result.IsDifferent.ShouldBeFalse();

                await compareStream.DisposeAsync().ConfigureAwait(false);
            }
        }

        [Fact]
        public async Task CompareShouldReturnDiffsStringWithThresholdInfo()
        {
            var strategy = new PngSnapshotStrategy(differencesThreshold: 0.3);
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.png");

            // Create reference image
            using (var bitmap = new SKBitmap(10, 10))
            {
                using var canvas = new SKCanvas(bitmap);
                canvas.DrawColor(SKColors.Red);
                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                var stream = new MemoryStream();
                await using var _1 = stream.ConfigureAwait(false);
                data.SaveTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await strategy.SaveAsync(referenceFilePath, stream);
            }

            // Compare with different image
            using (var bitmap2 = new SKBitmap(10, 10))
            {
                using var canvas2 = new SKCanvas(bitmap2);
                canvas2.DrawColor(SKColors.Blue);
                using var image2 = SKImage.FromBitmap(bitmap2);
                using var data2 = image2.Encode(SKEncodedImageFormat.Png, 100);

                var compareStream = new MemoryStream();
                data2.SaveTo(compareStream);
                compareStream.Seek(0, SeekOrigin.Begin);

                var result = await strategy.CompareAsync(referenceFilePath, compareStream).ConfigureAwait(false);

                result.DiffsString.ShouldContain("threshold 0.3");

                await compareStream.DisposeAsync().ConfigureAwait(false);
                if (result.DiffsData is not null)
                {
                    await result.DiffsData.DisposeAsync().ConfigureAwait(false);
                }
            }
        }

        [Fact]
        public async Task CompareShouldHandleZeroThreshold()
        {
            var strategy = new PngSnapshotStrategy(differencesThreshold: 0.0);
            var referenceFilePath = Path.Combine(this.testDirectory, "reference.png");

            // Create reference image
            using (var bitmap = new SKBitmap(5, 5))
            {
                using var canvas = new SKCanvas(bitmap);
                canvas.DrawColor(SKColors.White);
                using var image = SKImage.FromBitmap(bitmap);
                using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                var stream = new MemoryStream();
                await using var _1 = stream.ConfigureAwait(false);
                data.SaveTo(stream);
                stream.Seek(0, SeekOrigin.Begin);
                await strategy.SaveAsync(referenceFilePath, stream);
            }

            // Compare with identical image
            using (var bitmap2 = new SKBitmap(5, 5))
            {
                using var canvas2 = new SKCanvas(bitmap2);
                canvas2.DrawColor(SKColors.White);
                using var image2 = SKImage.FromBitmap(bitmap2);
                using var data2 = image2.Encode(SKEncodedImageFormat.Png, 100);

                var compareStream = new MemoryStream();
                data2.SaveTo(compareStream);
                compareStream.Seek(0, SeekOrigin.Begin);

                var result = await strategy.CompareAsync(referenceFilePath, compareStream).ConfigureAwait(false);

                result.IsDifferent.ShouldBeFalse();

                await compareStream.DisposeAsync().ConfigureAwait(false);
            }
        }

        // Helper class for testing non-seekable streams
        private class NonSeekableStream : Stream
        {
            private readonly MemoryStream innerStream = new MemoryStream();

            public override bool CanRead => this.innerStream.CanRead;
            public override bool CanSeek => false;
            public override bool CanWrite => this.innerStream.CanWrite;
            public override long Length => this.innerStream.Length;
            public override long Position
            {
                get => this.innerStream.Position;
                set => this.innerStream.Position = value;
            }

            public override void Flush() => this.innerStream.Flush();
            public override int Read(byte[] buffer, int offset, int count) => this.innerStream.Read(buffer, offset, count);
            public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
            public override void SetLength(long value) => this.innerStream.SetLength(value);
            public override void Write(byte[] buffer, int offset, int count) => this.innerStream.Write(buffer, offset, count);

            protected override void Dispose(bool disposing)
            {
                this.innerStream?.Dispose();
                base.Dispose(disposing);
            }
        }
    }
}
