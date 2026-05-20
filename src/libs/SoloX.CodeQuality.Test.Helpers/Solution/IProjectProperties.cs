// ----------------------------------------------------------------------
// <copyright file="IProjectProperties.cs" company="Xavier Solau">
// Copyright © 2021-2026 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

namespace SoloX.CodeQuality.Test.Helpers.Solution
{
    public interface IProjectProperties
    {
        /// <summary>
        /// Adds a property with the specified name and value.
        /// </summary>
        /// <param name="name">The name of the property to add. Cannot be null or empty.</param>
        /// <param name="value">The value to assign to the property.</param>
        /// <returns>Self.</returns>
        IProjectProperties Add(string name, string? value);

        /// <summary>
        /// Adds one or more name/value string property pairs to the current collection.
        /// </summary>
        /// <remarks>If a property with the same name already exists, its value may be overwritten
        /// depending on the implementation.</remarks>
        /// <param name="properties">An array of tuples, each containing a property name and its
        /// corresponding value to add. The property name cannot be null or empty.</param>
        /// <returns>Self.</returns>
        IProjectProperties Add(params (string name, string? value)[] properties);
    }
}
