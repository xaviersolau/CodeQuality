// ----------------------------------------------------------------------
// <copyright file="AClass2.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------


using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace SoloX.CodeQuality.Prod.Example
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AClass2
    {
        private readonly object test;

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="test"></param>
        protected AClass2(object test)
        {
            this.test = test;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object TestCase()
        {
            return this.test;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<object> TestCase2Async()
        {
            await Task.Delay(1000).ConfigureAwait(false);
            return this.test;
        }

        /// <summary>
        /// Test ignore logging rules.
        /// </summary>
        /// <param name="logger"></param>
        public void LogSomeThing(ILogger<AClass2> logger)
        {
            logger.LogInformation($"object is {this.test}");
        }
    }
}
