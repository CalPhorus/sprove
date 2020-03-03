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
using System.IO;

namespace Sprove
{

    /// <summary>
    ///
    /// </summary>
    public class Solution
    {
        internal static readonly string _expectedClassName = "SproveSolution";
        internal static readonly string _expectedFileName  = "SproveSolution.cs";
        private List<Project>           _projects          = new List<Project>();
        private Target                  _target;

        /// <summary>
        ///
        /// </summary>
        internal static string ExpectedClassName
        {
            get{ return _expectedClassName; }
        }

        /// <summary>
        ///
        /// </summary>
        internal static string ExpectedFileName
        {
            get{ return _expectedFileName; }
        }

        /// <summary>
        ///
        /// </summary>
        internal List<Project> Projects
        {
            get{ return _projects; }
        }

        /// <summary>
        ///
        /// </summary>
        internal Target SolutionTarget
        {
            get{ return _target; }
        }

        public Solution( Target target )
        {
            _target = target;
        }

        public virtual bool PreBuild()
        {
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileName">
        /// </param>
        /// <param name="fileText">
        /// </param>
        /// <returns>
        /// </returns>
        public string CreateFile( string fileName, string fileText )
        {
            string result = Path.Combine( Cache.CacheTmpDir, fileName );

            if( !Cache.CreateTempFile( result ) )
            {
                // Cannot create what already exists.
                return string.Empty;
            }

            File.WriteAllText( result, fileText );

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name">
        /// </param>
        /// <returns>
        /// </returns>
        public Project CreateProject( string name )
        {
            Project result;

            try
            {
                result = new Project( name );
                return result;
            }
            catch( ArgumentException exception )
            {
                Console.WriteLine( exception );
                return null;
            }
        }
    }

} // namespace Sprove

