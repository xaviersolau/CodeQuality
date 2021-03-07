// ----------------------------------------------------------------------
// <copyright file="Test.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;

namespace SoloX.CodeQuality.Prod.Example
{
    /// <summary>
    /// 
    /// </summary>
    public class Test : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dispose"></param>
        protected virtual void Dispose(bool dispose)
        {
            if (dispose)
            {
                // dispose managed resources
            }
            // dispose native resources
        }
    }
}
