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
using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace Sprove
{

    /// <summary>
    ///
    /// </summary>
    internal class Compiler
    {
        private CSharpCodeProvider provider     = new CSharpCodeProvider();
        private TempFileCollection tempFiles;

        /// <summary>
        ///
        /// </summary>
        public Compiler()
        {
            tempFiles = new TempFileCollection( Cache.CacheTmpDir, true );
        }

        /// <summary>
        /// Compile files into a binary.
        /// </summary>
        /// <param name="compileData">
        /// The data to be used in compilation.
        /// </param>
        /// <returns>
        /// Returns `true` on successful compilation, `false` on error.
        /// </returns>
        /// <see cref="CompileData" />
        public bool Compile( CompileData compileData )
        {
            CompilerParameters  parameters      = new CompilerParameters();
            CompilerResults     compileResult;
            List<string>        compileOptions  = new List<string>();
            string[]            sourceFiles     = new string[ 0 ];

            if( null != compileData.sourceFiles &&
                0 < compileData.sourceFiles.Count )
            {
                sourceFiles = compileData.sourceFiles.ToArray();
            }

            // Where to build the assembly
            parameters.OutputAssembly          = compileData.name;

            // Library or executable?
            parameters.GenerateExecutable      = !compileData.isLibrary;

            // Actually build a file.
            parameters.GenerateInMemory        = false;

            // What warning level?
            parameters.WarningLevel            = ( int )compileData.warningLevel;

            // Treat warnings as errors
            parameters.TreatWarningsAsErrors   = compileData.warningsAreErrors;

            // Temporary files.
            parameters.TempFiles               = tempFiles;

            // Build with debug info?
            parameters.IncludeDebugInformation = compileData.includeDebugInfo;

            // Defines, e.g. -define:DEBUG
            if( null != compileData.defines && 0 < compileData.defines.Count )
            {
                compileOptions.Add( "-define:" +
                    string.Join( ",", compileData.defines.ToArray() ) );
            }

            // Referenced assemblies. Default: System.dll
            parameters.ReferencedAssemblies.Add( "System.dll" );
            if( null != compileData.referencedAssemblies &&
                0 < compileData.referencedAssemblies.Count )
            {
                // string[]
                var assemblies = compileData.referencedAssemblies.ToArray();
                parameters.ReferencedAssemblies.AddRange( assemblies );
            }

            // CompilerOptions expects each option to be space separated.
            parameters.CompilerOptions =
                string.Join( " ", compileOptions.ToArray() );

            compileResult = provider.CompileAssemblyFromFile( parameters,
                sourceFiles );

            if( 0 < compileResult.Errors.Count )
            {
                Console.WriteLine();
                Console.WriteLine( "Failed to build {0} due to errors:",
                    compileData.name );
                foreach( CompilerError error in compileResult.Errors )
                {
                    Console.WriteLine( "    {0}", error.ToString() );
                    Console.WriteLine();
                }
            }

            return 0 == compileResult.Errors.Count;
        }
    }

} // namespace Sprove

