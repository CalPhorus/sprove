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
using System.Collections.Generic;

namespace Sprove
{

    /// <summary>
    /// Represents a project made up of C# source files,
    /// </summary>
    public sealed class Project
    {

        private readonly string _name;
        private List<string>    _sourceFiles    = new List<string>();
        private bool            _isLibrary      = false;

        /// <summary>
        ///
        /// </summary>
        internal List<string> SourceFiles
        {
            get{ return _sourceFiles; }
        }

        /// <summary>
        ///
        /// </summary>
        public string Name{ get{ return _name; } }

        /// <summary>
        ///
        /// </summary>
        public bool IsLibrary
        {
            get{ return _isLibrary; }
            set{ _isLibrary = value; }
        }

        /// <summary>
        ///
        /// </summary>
        internal Project( string name )
        {
            _name = name;

            if( null == name || string.Empty == name )
            {
                throw new ArgumentException( "Project name cannot be empty!" );
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="files">
        /// </param>
        public Project AddSourceFiles( string[] files )
        {
            _sourceFiles.AddRange( files );
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="files">
        /// </param>
        public Project AddSourceFiles( string files )
        {
            _sourceFiles.Add( files );
            return this;
        }


    }

} // namespace Sprove

