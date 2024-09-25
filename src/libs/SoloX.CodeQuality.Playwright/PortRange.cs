// ----------------------------------------------------------------------
// <copyright file="PortRange.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.CodeQuality.Playwright
{
    /// <summary>
    /// Network port range definition.
    /// </summary>
    /// <param name="StartPort">Including port range start.</param>
    /// <param name="EndPort">Excluding port range end.</param>
    public record PortRange(int StartPort, int EndPort);
}
