// Copyright (c) 2020 Anthony Smith. All rights reserved.
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

            if( null != compileData.sourceFiles ||
                0 < compileData.sourceFiles.Count )
            {
                sourceFiles = compileData.sourceFiles.ToArray();
            }

            //TODO(anthony): Actually build the output location
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

            parameters.CompilerOptions =
                string.Join( " ", compileOptions.ToArray() );

            compileResult = provider.CompileAssemblyFromFile( parameters,
                sourceFiles );

            if( 0 < compileResult.Errors.Count )
            {
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

