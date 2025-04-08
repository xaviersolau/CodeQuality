// ----------------------------------------------------------------------
// <copyright file="INugetConfigConfiguration.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.CodeQuality.Test.Helpers.Solution
{
    /// <summary>
    /// Interface to make 'nuget.config' configuration.
    /// </summary>
    public interface INugetConfigConfiguration
    {
        /// <summary>
        /// Use package sources.
        /// </summary>
        /// <param name="packageSources">Package sources.</param>
        void UsePackageSources(Action<IPackageSources> packageSources);
    }

}
