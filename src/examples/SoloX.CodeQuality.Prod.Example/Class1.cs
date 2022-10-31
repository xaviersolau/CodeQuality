// ----------------------------------------------------------------------
// <copyright file="Class1.cs" company="Xavier Solau">
// Copyright © 2021 Xavier Solau.
// Licensed under the MIT license.
// See LICENSE file in the project root for full license information.
// </copyright>
// ----------------------------------------------------------------------

using System;
using System.Linq;
using System.Collections.Generic;

namespace SoloX.CodeQuality.Prod.Example
{

    /// <summary>
    /// This is some comment.
    /// </summary>
    public class Class1<TTest>
    {
        private readonly IList<IInterface1> values;

        /// <summary>
        /// 
        /// </summary>
        public static readonly string StaticValue = "tttt";

        /// <summary>
        /// 
        /// </summary>
        public const string ConstValue = "tttt";


        /// <summary>
        /// 
        /// </summary>
        public Class1()
        {
            var test = 10;

            if (test > 5)
            {
                this.values = new List<IInterface1>(test);
            }
        }

        /// <summary>
        /// Test add value.
        /// </summary>
        /// <param name="value"></param>
        public void AddMethod(IInterface1 value)
        {
            this.values.Add(value ?? throw new ArgumentException("sdf"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int MethodPublic()
        {
            try
            {
                return 2 * MethodOne("");
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public void MethodWithNullable1(string? data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var hash = data.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string MethodWithNullable2(string? data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return data;
        }

        private static int MethodOne(string str1)
        {
            var str2 = "fqsdfqs";

            return str1.Equals(str2, StringComparison.Ordinal) ? 1 : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int MethodTwo(int? value)
        {
            using (var tmp = new Test())
            {

            }


            return value != null ? value.Value : 10;
        }

        /// <summary>
        /// 
        /// </summary>
        public IInterface1 FirstValue => this.values.First();

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IInterface1> Values => this.values.Take(2);
    }
}
