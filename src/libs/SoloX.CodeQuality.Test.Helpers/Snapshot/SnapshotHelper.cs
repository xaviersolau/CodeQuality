// ----------------------------------------------------------------------
// <copyright file="SnapshotHelper.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using DiffPlex.Renderer;
using SkiaSharp;

namespace SoloX.CodeQuality.Test.Helpers.Snapshot
{
    /// <summary>
    /// Provides utility methods for managing and creating snapshots of data (Text and Images).
    /// </summary>
    public class SnapshotHelper
    {
        public const string DefaultFolder = "Snapshots";

        private readonly string rootPath;
        private readonly string snapshotsFolder;
        private readonly string? intermediateFolder;

        /// <summary>
        /// Initializes a new instance of the SnapshotHelper class using the specified intermediate folder for snapshot
        /// storage.
        /// </summary>
        /// <param name="intermediateFolder">The intermediate path to the folder where snapshot files will be stored.</param>
        public SnapshotHelper(string intermediateFolder)
            : this(options => options.IntermediateFolder = intermediateFolder)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SnapshotHelper class, optionally allowing configuration of snapshot
        /// options.
        /// </summary>
        /// <remarks>If the RootPath property is not set in the options, it defaults to the root directory
        /// of the calling code's test project. The SnapshotsFolder and IntermediateFolder properties are also initialized
        /// based on the configured options.</remarks>
        /// <param name="config">An optional action that configures the SnapshotHelperOptions before initialization. If provided, this action
        /// is invoked with a new instance of SnapshotHelperOptions.</param>
        public SnapshotHelper(Action<SnapshotHelperOptions>? config = null)
        {
            var options = new SnapshotHelperOptions();

            config?.Invoke(options);

            this.rootPath = options.RootPath ?? GetLocationFromCallingCodeProjectRoot(null, 2);

            this.snapshotsFolder = options.SnapshotsFolder;
            this.intermediateFolder = options.IntermediateFolder;
        }


        /// <summary>
        /// Asynchronously compares the specified snapshot with the provided text content.
        /// </summary>
        /// <param name="snapshotName">The name of the snapshot to compare against. Cannot be null or empty.</param>
        /// <param name="snapshotText">The text content to compare with the snapshot. Cannot be null.</param>
        /// <param name="forceReplaceSnapshot">Indicates if the snapshot must be replaced.</param>
        /// <returns>A task that represents the asynchronous comparison operation.</returns>
        /// <exception cref="SnapshotException">Thrown when the method is called, as the implementation is not yet provided.</exception>
        /// <remarks>When the snapshot reference file does not exist, it will be created with the provided snapshotText content. If the
        /// snapshot reference file exists and the content does not match, a SnapshotException is thrown with the differences between
        /// the expected and actual content.</remarks>
        public Task CompareTextSnapshotAsync(string snapshotName, string snapshotText, bool forceReplaceSnapshot = false)
        {
            return this.CompareBaseSnapshotAsync(
                snapshotName,
                snapshotText,
                forceReplaceSnapshot,
                "txt",
                InternalWriteTextSnapshotAsync,
                InternalCompareTextSnapshotAsync);
        }

        /// <summary>
        /// Asynchronously compares the specified snapshot with the provided Png image content.
        /// </summary>
        /// <param name="snapshotName">The name of the snapshot to compare against. Cannot be null or empty.</param>
        /// <param name="snapshotPng">The text content to compare with the snapshot. Cannot be null.</param>
        /// <param name="differencesThreshold">The differencies threshold to report a comparaison difference between 0.0 and 1.0 with 0.0 being exactly no diferences.</param>
        /// <param name="forceReplaceSnapshot">Indicates if the snapshot must be replaced.</param>
        /// <returns>A task that represents the asynchronous comparison operation.</returns>
        /// <exception cref="SnapshotException">Thrown when the method is called, as the implementation is not yet provided.</exception>
        /// <remarks>When the snapshot reference file does not exist, it will be created with the provided snapshotPng content. If the
        /// snapshot reference file exists and the content does not match, a SnapshotException is thrown with the differences between
        /// the expected and actual content.</remarks>
        public Task ComparePngSnapshotAsync(string snapshotName, Stream snapshotPng, double differencesThreshold = 0.0, bool forceReplaceSnapshot = false)
        {
            return this.CompareBaseSnapshotAsync(
                snapshotName,
                snapshotPng,
                forceReplaceSnapshot,
                "png",
                InternalWritePngSnapshotAsync,
                (string snapshotReferenceFile, Stream snapshotData) => InternalComparePngSnapshotAsync(snapshotReferenceFile, snapshotData, differencesThreshold));
        }

        /// <summary>
        /// Compute the folder location form the calling code assembly project root.
        /// </summary>
        /// <param name="folder">The relative folder or null.</param>
        /// <returns>Folder location in the project of the calling code assembly.</returns>
        public static string GetLocationFromCallingCodeProjectRoot(string? folder = null)
        {
            return GetLocationFromCallingCodeProjectRoot(folder, 2);
        }

        private delegate Task SaveHandler<TData>(string snapshotFile, TData snapshotData);

        private delegate Task<(bool IsSame, TData? DiffsData, string? DiffsString)> CompareHandler<TData>(string snapshotReferenceFile, TData snapshotData);

        private async Task CompareBaseSnapshotAsync<TData>(
            string snapshotName,
            TData snapshotData,
            bool forceReplaceSnapshot,
            string fileExt,
            SaveHandler<TData> saveHandler,
            CompareHandler<TData> compareHandler)
            where TData : class
        {
            ArgumentNullException.ThrowIfNull(saveHandler);
            ArgumentNullException.ThrowIfNull(compareHandler);

            var snapshotFolder = string.IsNullOrEmpty(this.intermediateFolder)
                ? this.rootPath
                : Path.Combine(this.rootPath, this.intermediateFolder);

            snapshotFolder = Path.Combine(snapshotFolder, this.snapshotsFolder);

            var snapshotReferenceFile = Path.Combine(snapshotFolder, $"{snapshotName}.snapshot.ref.{fileExt}");
            var snapshotRunFile = Path.Combine(snapshotFolder, $"{snapshotName}.snapshot.run.{fileExt}");
            var snapshotDiffsFile = Path.Combine(snapshotFolder, $"{snapshotName}.snapshot.diffs.{fileExt}");

            forceReplaceSnapshot = forceReplaceSnapshot || !File.Exists(snapshotReferenceFile);

            if (!Directory.Exists(snapshotFolder) && forceReplaceSnapshot)
            {
                Directory.CreateDirectory(snapshotFolder);
            }

            if (forceReplaceSnapshot)
            {
                await saveHandler(snapshotReferenceFile, snapshotData).ConfigureAwait(false);
            }
            else
            {
                var compareResult = await compareHandler(snapshotReferenceFile, snapshotData).ConfigureAwait(false);

                if (forceReplaceSnapshot)
                {
                    await saveHandler(snapshotReferenceFile, snapshotData).ConfigureAwait(false);
                }

                if (!compareResult.IsSame)
                {
                    await saveHandler(snapshotRunFile, snapshotData).ConfigureAwait(false);

                    await saveHandler(snapshotDiffsFile, compareResult.DiffsData!).ConfigureAwait(false);

                    throw new SnapshotException(compareResult.DiffsString!);
                }
            }

            if (File.Exists(snapshotRunFile))
            {
                File.Delete(snapshotRunFile);
            }

            if (File.Exists(snapshotDiffsFile))
            {
                File.Delete(snapshotDiffsFile);
            }
        }

        private static string GetLocationFromCallingCodeProjectRoot(string? folder, int frame = 1)
        {
            var callingAssembly = new StackTrace().GetFrame(frame)!.GetMethod()!.DeclaringType!.Assembly;
            var assemblyFolder = Path.GetDirectoryName(callingAssembly.Location);
            var projectRoot = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(assemblyFolder)))!;

            return string.IsNullOrWhiteSpace(folder) ? projectRoot : Path.Combine(projectRoot, folder);
        }

        private static Task InternalWriteTextSnapshotAsync(string snapshotFile, string snapshotData)
        {
            if (File.Exists(snapshotFile))
            {
                File.Delete(snapshotFile);
            }

            return File.WriteAllTextAsync(snapshotFile, snapshotData);
        }

        private static async Task<(bool IsSame, string? DiffsData, string? DiffsString)> InternalCompareTextSnapshotAsync(string snapshotReferenceFile, string snapshotData)
        {
            var referenceText = await File.ReadAllTextAsync(snapshotReferenceFile).ConfigureAwait(false);
            var snapshotDiffs = UnidiffRenderer.GenerateUnidiff(referenceText, snapshotData, oldFileName: "Snapshot reference", newFileName: "Snapshot run");

            return (IsSame: string.IsNullOrEmpty(snapshotDiffs), DiffsData: snapshotDiffs, DiffsString: snapshotDiffs);
        }

        private static async Task InternalWritePngSnapshotAsync(string snapshotFile, Stream snapshotData)
        {
            if (snapshotData.Position > 0)
            {
                if (!snapshotData.CanSeek)
                {
                    throw new InvalidOperationException("Unable to seek data stream");
                }

                snapshotData.Seek(0, SeekOrigin.Begin);
            }

            if (File.Exists(snapshotFile))
            {
                File.Delete(snapshotFile);
            }

            var outputStream = File.OpenWrite(snapshotFile);
            await using var _ = outputStream.ConfigureAwait(false);

            await snapshotData.CopyToAsync(outputStream).ConfigureAwait(false);
        }

        private static async Task<(bool IsSame, Stream? DiffsData, string? DiffsString)> InternalComparePngSnapshotAsync(string snapshotReferenceFile, Stream snapshotData, double differencesThreshold)
        {
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

                return (
                    IsSame: diffs <= differencesThreshold,
                    DiffsData: diffsStream,
                    DiffsString: $"See Png Diffs file (deltas {diffs.ToString(CultureInfo.InvariantCulture)} <= threshold {differencesThreshold.ToString(CultureInfo.InvariantCulture)})");
            }
        }
    }
}
