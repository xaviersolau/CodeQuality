// ----------------------------------------------------------------------
// <copyright file="IInterface1.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace SoloX.CodeQuality.Prod.Example
{
    /// <summary>
    /// 
    /// </summary>
    public class TestEventArgs : EventArgs
    {

    }

    /// <summary>
    /// doc of the interface.
    /// </summary>
    public interface IInterface1
    {
        /// <summary>
        /// return result.
        /// </summary>
        string Result { get; }

        /// <summary>
        /// 
        /// </summary>
        event EventHandler<TestEventArgs> TestEvent;

        /// <summary>
        /// return result.
        /// </summary>
        string TestValue { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<object> Test();
    }
}
