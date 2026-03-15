// ----------------------------------------------------------------------
// <copyright file="PngSnapshotStrategy.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using SkiaSharp;

namespace SoloX.CodeQuality.Test.Helpers.Snapshot.Impl
{
    /// <summary>
    /// Provides a snapshot strategy for saving and comparing PNG image data streams using a configurable difference
    /// threshold.
    /// </summary>
    /// <remarks>This class implements the ISnapshotStrategy interface for PNG images, enabling snapshot
    /// persistence and visual comparison of image data. Differences between images are highlighted and quantified based
    /// on the configured threshold. The comparison operation produces a diff image stream if differences are detected.
    /// This strategy is suitable for scenarios such as visual regression testing or automated image
    /// validation.</remarks>
    public class PngSnapshotStrategy : ISnapshotStrategy<Stream>
    {
        private readonly double differencesThreshold;

        /// <summary>
        /// Initializes a new instance of the PngSnapshotStrategy class with the specified differences threshold.
        /// </summary>
        /// <remarks>A differences threshold of 0.0 means that any difference between images will be
        /// detected. Increasing the threshold can be useful to ignore minor variations such as compression
        /// artifacts.</remarks>
        /// <param name="differencesThreshold">The minimum percentage of pixel differences required to consider two PNG images as different.
        /// Must be between 0.0 and 1.0 with 0.0 being exactly the same image.</param>
        public PngSnapshotStrategy(double differencesThreshold = 0.0)
        {
            this.differencesThreshold = differencesThreshold;
        }

        /// <inheritdoc/>
        public string FileExtension => "png";

        /// <inheritdoc/>
        public async Task SaveAsync(string snapshotFile, Stream snapshotData)
        {
            MakeSureSnapshotDataPositionIsZero(snapshotData);

            if (File.Exists(snapshotFile))
            {
                File.Delete(snapshotFile);
            }

            var outputStream = File.OpenWrite(snapshotFile);
            await using var _ = outputStream.ConfigureAwait(false);

            await snapshotData.CopyToAsync(outputStream).ConfigureAwait(false);
        }

        private static void MakeSureSnapshotDataPositionIsZero(Stream snapshotData)
        {
            if (snapshotData.Position > 0)
            {
                if (!snapshotData.CanSeek)
                {
                    throw new InvalidOperationException("Unable to seek data stream");
                }

                snapshotData.Seek(0, SeekOrigin.Begin);
            }
        }

        /// <inheritdoc/>
        public async Task<CompareSnapshotResult<Stream>> CompareAsync(string snapshotReferenceFile, Stream snapshotData)
        {
            MakeSureSnapshotDataPositionIsZero(snapshotData);

            double sumDif = 0;

            var referenceData = File.OpenRead(snapshotReferenceFile);
            await using var _1 = referenceData.ConfigureAwait(false);

            var snapshotDataCopy = new MemoryStream();
            await using var _2 = snapshotDataCopy.ConfigureAwait(false);

            await snapshotData.CopyToAsync(snapshotDataCopy).ConfigureAwait(false);
            snapshotDataCopy.Seek(0, SeekOrigin.Begin);

            // Load both images
            using var bitmap1 = SKBitmap.Decode(referenceData);
            using var bitmap2 = SKBitmap.Decode(snapshotDataCopy);

            var maxWidth = Math.Max(bitmap1.Width, bitmap2.Width);
            var maxHeight = Math.Max(bitmap1.Height, bitmap2.Height);
            var minWidth = Math.Min(bitmap1.Width, bitmap2.Width);
            var minHeight = Math.Min(bitmap1.Height, bitmap2.Height);

            // Create a result image to hold the differences
            using (var resultBitmap = new SKBitmap(maxWidth, maxHeight))
            {
                for (var y = 0; y < minHeight; y++)
                {
                    for (var x = 0; x < minWidth; x++)
                    {
                        // Get the pixel color from each image
                        var pixel1 = bitmap1.GetPixel(x, y);
                        var pixel2 = bitmap2.GetPixel(x, y);

                        // Compare the pixels
                        if (pixel1 != pixel2)
                        {
                            var dif = (byte)((Math.Abs(pixel1.Red - pixel2.Red) + Math.Abs(pixel1.Green - pixel2.Green) + Math.Abs(pixel1.Blue - pixel2.Blue)) / 3);

                            // Highlight differences with a red color
                            resultBitmap.SetPixel(x, y, new SKColor(dif, 0, 0)); // Red for difference

                            sumDif += dif / 255.0;
                        }
                        else
                        {
                            // 0 if no difference
                            resultBitmap.SetPixel(x, y, new SKColor(0, 0, 0));
                        }
                    }
                    for (var x = minWidth; x < maxWidth; x++)
                    {
                        // Highlight differences with a red color
                        resultBitmap.SetPixel(x, y, new SKColor(255, 0, 0)); // Red for difference
                        sumDif += 1.0;
                    }
                }
                for (var y = minHeight; y < maxHeight; y++)
                {
                    for (var x = 0; x < maxWidth; x++)
                    {
                        // Highlight differences with a red color
                        resultBitmap.SetPixel(x, y, new SKColor(255, 0, 0)); // Red for difference
                        sumDif += 1.0;
                    }
                }

                MemoryStream? diffsStream = null;

                if (sumDif > 0)
                {
                    // Save the resulting image with highlighted differences
                    using var image = SKImage.FromBitmap(resultBitmap);
                    using var data = image.Encode(SKEncodedImageFormat.Png, 100);

                    diffsStream = new MemoryStream();
                    data.SaveTo(diffsStream);
                    diffsStream.Seek(0, SeekOrigin.Begin);
                }

                var diffs = sumDif / (maxWidth * maxHeight);

                return new CompareSnapshotResult<Stream>(
                    IsDifferent: diffs > this.differencesThreshold,
                    DiffsData: diffsStream,
                    DiffsString: $"See Png Diffs file (deltas {diffs.ToString(CultureInfo.InvariantCulture)} <= threshold {this.differencesThreshold.ToString(CultureInfo.InvariantCulture)})");
            }
        }
    }
}