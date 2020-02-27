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
using System.Reflection;

namespace Sprove
{

    /// <summary>
    ///
    /// </summary>
    internal class SolutionLoader
    {

        public SolutionLoader()
        {}

        private string DetermineNamespace( string location )
        {
            string[]    components;
            string      dir;
            string      toRemove;
            string      result      = string.Empty;

            toRemove = SolutionRoot.RootDirectory + Path.DirectorySeparatorChar;

            if( File.Exists( location ) )
            {
                // A file. Get directory parent.
                dir = Path.GetDirectoryName( location );
            }
            else if( Directory.Exists( location ) )
            {
                // Not a file, but a directory.
                dir = location;
            }
            else
            {
                // Not an existing directory or file. Invalid location.
                return string.Empty;
            }

            // Remove all alternative directory separator characters.
            dir = dir.Replace( Path.AltDirectorySeparatorChar,
                Path.DirectorySeparatorChar );

            // Remove root directory and spaces from dir.
            dir = dir.Replace( toRemove, "" ).Replace( " ", "" );

            // Split dir based on the directory separator character.
            components = dir.Split( Path.DirectorySeparatorChar );

            // If components.Length is less than 2, no need to do anything
            // special, just return.
            if( 1 == components.Length )
            {
                return components[ 0 ];
            }
            else if( 0 == components.Length )
            {
                // Can't do anything if it is empty.
                return string.Empty;
            }

            // Join again with the '.' character, so that it can produce a valid
            // namespace.
            result = String.Join( ".", components );

            return result;
        }

    }

} // namespace Sprove

