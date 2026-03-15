// ----------------------------------------------------------------------
// <copyright file="CompareSnapshotResult.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.CodeQuality.Test.Helpers.Snapshot
{
    /// <summary>
    /// Represents the result of comparing two snapshots, including whether differences were found and details about
    /// those differences.
    /// </summary>
    /// <typeparam name="TData">The type used to represent structured difference data.</typeparam>
    /// <param name="IsDifferent">A value indicating whether the compared snapshots are different. Set to true if differences
    /// exist; otherwise, false.</param>
    /// <param name="DiffsData">An object containing structured data that describes the differences between the snapshots, or
    /// null if no differences are present or structured data is not available.</param>
    /// <param name="DiffsString">A string representation of the differences between the snapshots, or null if no differences
    /// are present or a string representation is not available.</param>
    public record CompareSnapshotResult<TData>(bool IsDifferent, TData? DiffsData, string? DiffsString);
}