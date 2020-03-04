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
    internal class ProjectBuilder
    {
        private Compiler    _compiler       = new Compiler();
        private CompileData _compileData    = new CompileData();

        /// <summary>
        ///
        /// </summary>
        public ProjectBuilder()
        {
            ResetData();
        }

        private void ResetData()
        {
            _compileData.defines                = new List<string>();
            _compileData.includeDebugInfo       = false;
            _compileData.isLibrary              = false;
            _compileData.name                   = string.Empty;
            _compileData.referencedAssemblies   = new List<string>();
            _compileData.sourceFiles            = new List<string>();
            _compileData.warningsAreErrors      = false;
            _compileData.warningLevel           = WarningLevel.Level4;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Build( Project project )
        {
            string extension = ".exe";
            if( project.IsLibrary )
            {
                extension = ".dll";
            }

            _compileData.name       = Path.Combine( SproveDirectory.BinaryDir,
                project.Name.Replace( " ", "" ) + extension );
            _compileData.isLibrary  = project.IsLibrary;
            _compileData.sourceFiles.AddRange( project.SourceFiles.ToArray() );

            bool result = _compiler.Compile( _compileData );

            Console.WriteLine( "Hi" );

            ResetData();
            return result;
        }
    }

} // namespace Sprove

