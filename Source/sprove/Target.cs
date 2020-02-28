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
using System.Runtime.InteropServices;

namespace Sprove
{

    /// <summary>
    ///
    /// </summary>
    public class Target
    {
        private readonly BuildConfig _buildConfig;
        private readonly BuildTarget _buildTarget;

        internal static BuildTarget Host()
        {
            BuildTarget result = BuildTarget.Unknown;

            if( RuntimeInformation.IsOSPlatform( OSPlatform.Linux ) )
            {
                result = BuildTarget.Linux;
            }
            else if( RuntimeInformation.IsOSPlatform( OSPlatform.OSX ) )
            {
                result = BuildTarget.MacOS;
            }
            else if( RuntimeInformation.IsOSPlatform( OSPlatform.Windows ) )
            {
                result = BuildTarget.Windows;
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        public BuildConfig Config{ get{ return _buildConfig; } }

        /// <summary>
        ///
        /// </summary>
        public BuildTarget TargetOS{ get{ return _buildTarget; } }

        /// <summary>
        ///
        /// </summary>
        public Target( BuildConfig buildConfig, BuildTarget buildTarget )
        {
            _buildConfig = buildConfig;
            _buildTarget = buildTarget;
        }
    }

} // namespace Sprove

