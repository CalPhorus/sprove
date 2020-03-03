// Copyright 2020 Anthony Smith
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using System;

//TODO(anthony): Determine if there is a way to get the type that VerbAttribute
// is being applied to and raise a VerbTypeException if it is not being applied
// to a boolean type.

namespace Sprove
{

    /// <summary>
    ///
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple=false)]
    internal class VerbAttribute : Attribute
    {
        private string _code   = string.Empty;

        /// <summary>
        ///
        /// </summary>
        public string Code
        {
            get{ return _code; }
            set{ _code = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public VerbAttribute()
        {}
    }

} // namespace Sprove

