// ----------------------------------------------------------------------
// <copyright file="Devices.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.CodeQuality.Playwright
{
    /// <summary>
    /// Some Playwright devices.
    /// For more information see https://github.com/microsoft/playwright/blob/main/packages/playwright-core/src/server/deviceDescriptorsSource.json
    /// </summary>
    public static class Devices
    {
        /// <summary>
        /// Safari on desktop.
        /// </summary>
        public const string DesktopSafari = "Desktop Safari";
        /// <summary>
        /// Chrome on desktop.
        /// </summary>
        public const string DesktopChrome = "Desktop Chrome";
        /// <summary>
        /// Edge on desktop.
        /// </summary>
        public const string DesktopEdge = "Desktop Edge";
        /// <summary>
        /// Firefox on desktop.
        /// </summary>
        public const string DesktopFirefox = "Desktop Firefox";

        /// <summary>
        /// Pixel.
        /// </summary>
        public const string Pixel = "Pixel 7";
        /// <summary>
        /// Pixel landscape.
        /// </summary>
        public const string PixelLandscape = "Pixel 7 landscape";

#pragma warning disable IDE1006 // Naming Styles
        /// <summary>
        /// Pixel.
        /// </summary>
        public const string iPhone = "iPhone 15";
        /// <summary>
        /// Pixel landscape.
        /// </summary>
        public const string iPhoneLandscape = "iPhone 15 landscape";
#pragma warning restore IDE1006 // Naming Styles
    }
}
