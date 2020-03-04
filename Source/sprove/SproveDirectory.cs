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
using System.IO;

namespace Sprove
{

    /// <summary>
    ///
    /// </summary>
    internal static class SproveDirectory
    {

        private static string _binDir;

        /// <summary>
        ///
        /// </summary>
        public static string BinaryDir
        {
            get{ return _binDir; }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>
        /// </returns>
        public static bool Initialize()
        {
            bool    result      = false;
            string  binaries    = "Binaries";

            try
            {
                _binDir = CreateDirectory( binaries );
                result = true;
            }
            catch( Exception exception )
            {
                Console.WriteLine( exception );
            }

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="directory">
        /// </param>
        /// <returns>
        /// </returns>
        public static string CreateDirectory( string directory )
        {
            string  toBuild = Path.Combine( SolutionRoot.RootDirectory, directory );

            if( !Directory.Exists( toBuild ) )
            {
                try
                {
                    Directory.CreateDirectory( toBuild );
                }
                catch( Exception exception )
                {
                    // Don't handle the exception here.
                    throw exception;
                }
            }

            return toBuild;
        }

    }

} // namespace Sprove

