// ***********************************************************************
// Copyright (c) 2008 Charlie Poole
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// ***********************************************************************

using System.Reflection;
using System.Text;
using NUnit.Framework.Api;

namespace NUnit.Framework.Internal
{
    /// <summary>
    /// ParameterizedMethodSuite holds a collection of individual
    /// TestMethods with their arguments applied.
    /// </summary>
    public class ParameterizedMethodSuite : TestSuite
    {
        private MethodInfo method;

        /// <summary>
        /// Construct from a MethodInfo
        /// </summary>
        /// <param name="method"></param>
        public ParameterizedMethodSuite(MethodInfo method)
            : base(method.ReflectedType.FullName, method.Name)
        {
            this.method = method;
            this.maintainTestOrder = true;
        }

        /// <summary>
        /// Gets the MethodInfo for which this suite is being built.
        /// </summary>
        public MethodInfo Method
        {
            get { return method; }
        }

        /// <summary>
        /// Override Run, setting Fixture to that of the Parent.
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public override TestResult Run(ITestListener listener)
        {
            if (this.Parent != null)
            {
                // TODO: Clean this up
                TestSuite suite = this.Parent as TestSuite;
                if (suite != null)
                {
                    this.Fixture = suite.Fixture;
                    this.setUpMethods = suite.SetUpMethods;
                    this.tearDownMethods = suite.TearDownMethods;
                }
            }
          
            return base.Run(listener);
        }

        /// <summary>
        /// The name used for the top-level element in the
        /// XML representation of this test
        /// </summary>
        /// <value></value>
        public override string ElementName
        {
            get
            {
#if CLR_2_0 || CLR_4_0
                if (this.Method.ContainsGenericParameters)
                    return "generic-method";
#endif
                
                return "method";
            }
        }
    }
}