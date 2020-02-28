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
using System.Reflection;

namespace Sprove
{

    /// <summary>
    ///
    /// </summary>
    internal class SolutionLoader
    {

        private Solution _loadedSolution = null;

        /// <summary>
        ///
        /// </summary>
        public Solution LoadedSolution
        {
            get{ return _loadedSolution; }
        }

        /// <summary>
        ///
        /// </summary>
        public SolutionLoader()
        {}

        private string DetermineNamespace( string location )
        {
            string[]    components;
            string      dir;
            string      toRemove;
            string      result      = string.Empty;

            toRemove = SolutionRoot.RootDirectory;

            if( File.Exists( location ) )
            {
                // A file. Get directory parent.
                dir = Path.GetDirectoryName( location );
            }
            else if( Directory.Exists( location ) )
            {
                // Not a file, but a directory.
                dir = location;

                if( dir.EndsWith( string.Empty + Path.DirectorySeparatorChar ) ||
                    dir.EndsWith( string.Empty + Path.AltDirectorySeparatorChar ) )
                {
                    toRemove += Path.DirectorySeparatorChar;
                }
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
            if( 1 == components.Length &&
                string.Empty != components[ 0 ] )
            {
                // add the '.' character explicitly at the end so that it
                // does not need to be added later.
                return components[ 0 ] + ".";
            }
            else if( 1 >= components.Length )
            {
                // Either empty array or empty component.
                // Can't do anything if it is empty.
                return string.Empty;
            }

            // Join again with the '.' character, so that it can produce a valid
            // namespace. Add the '.' character explicitly at the end so that it
            // does not need to be added later.
            result = String.Join( ".", components ) + ".";

            return result;
        }

        private bool NeedsCompilation( string fileName, string compileOutput )
        {
            if( !File.Exists( compileOutput ) )
            {
                // If the output does not exist, a compilation needs to happen.
                return true;
            }

            DateTime scriptDate = File.GetLastWriteTime( fileName );
            DateTime outputDate = File.GetLastWriteTime( compileOutput );

            // If scriptDate is newer than outputDate, then a compilation is
            // needed.
            return 0 < DateTime.Compare( scriptDate, outputDate );
        }

        private bool CreateAssembly( string fileName, string assemblyLocation,
                                     string assemblyNamespace )
        {
            if( NeedsCompilation( fileName, assemblyLocation ) )
            {
                Assembly    self        = Assembly.GetExecutingAssembly();
                CompileData compileData = new CompileData();
                Compiler    compiler    = new Compiler();
                
                if( string.Empty != assemblyNamespace )
                {
                    string tempFile = Path.Combine( Cache.CacheTmpDir,
                        assemblyNamespace + Solution.ExpectedFileName + ".cs" );
                    string readFile = File.ReadAllText( fileName );

                    readFile = "namespace " + assemblyNamespace + "{" + readFile + "}";

                    File.WriteAllText( tempFile, readFile );

                    fileName = tempFile;
                }

                // Build a dynamic library that can be loaded at runtime.
                compileData.isLibrary           = true;

                // Allow debug info to be included so user can test for errors
                // in their project. They will have some at some point.
                compileData.includeDebugInfo    = true;

                // Where the heck does it go? Oh yeah, it'll get saved to
                // assemblyLocation! Brilliant.
                compileData.name                = assemblyLocation;

                // The source files? Just one: fileName.
                compileData.sourceFiles         = new List<string>();
                compileData.sourceFiles.Add( fileName );

                // To care about the users or not to, that is the question.
                // Treating warnings as errors = not caring. In this case,
                // I don't care.
                compileData.warningsAreErrors   = true;

                // Warning level? TO THE MAX!
                compileData.warningLevel = WarningLevel.Level4;

                // Add this assembly to the list of referenced assemblies.
                compileData.referencedAssemblies = new List<string>();
                compileData.referencedAssemblies.Add( self.Location );

                if( !compiler.Compile( compileData ) )
                {
                    // Couldn't build it, how am I supposed to load it?
                    return false;
                }
            }

            return true;
        }

        private bool LoadAssembly( string assemblyLocation, ref Assembly outAssembly )
        {
            bool result = false;

            try
            {
                outAssembly = Assembly.LoadFile( assemblyLocation );

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
        /// <param name="location">
        /// </param>
        /// <returns>
        /// </returns>
        public bool Load( string location, Target target )
        {
            if( !Directory.Exists( location ) )
            {
                // Not a directory, it is either a file or does not exist.
                return false;
            }

            bool    result              = false;
            string  assemblyNamespace   = DetermineNamespace( location );
            string  assemblyLocation    = Cache.CacheDir;
            string  fileName            = Path.Combine( location,
                Solution.ExpectedFileName );
            string  assemblyName        =
                assemblyNamespace + Solution.ExpectedClassName + ".dll";

            if( !File.Exists( fileName ) )
            {
                // Does not exist -- cannot open an assembly that does not
                // exist.
                return false;
            }

            assemblyLocation = Path.Combine( Cache.CacheDir, assemblyName );

            if( !CreateAssembly( fileName, assemblyLocation, assemblyNamespace ) )
            {
                return false;
            }

            Assembly loadedAssembly = null;
            if( !LoadAssembly( assemblyLocation, ref loadedAssembly ) )
            {
                return false;
            }

            // Load the solution from here.
            Solution    solution        = null;
            Type        solutionType    =
                loadedAssembly.GetType(
                    assemblyNamespace + Solution.ExpectedClassName );

            if( null == solutionType )
            {
                // Expected class not there.
                return false;
            }

            solution = Activator.CreateInstance( solutionType,
                new object[]{ target } ) as Solution;

            if( null == solution )
            {
                // Could not instantiate an object of the expected type.
                return false;
            }

            _loadedSolution = solution;
            
            result = true;
            return result;
        }

    }

} // namespace Sprove

