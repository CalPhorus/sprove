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
    /// Finds the first directory that contains a file matching the value
    /// `Solution.ExpectedFileName`.
    /// </summary>
    internal static class SolutionRoot
    {

        private static string _rootDirectory = null;

        /// <summary>
        /// The root directory for the solution.
        /// </summary>
        public static string RootDirectory{ get{ return _rootDirectory; } }

        /// <summary>
        ///
        /// </summary>
        /// <param name="OutDirectory">
        /// </param>
        /// <return>
        /// </return>
        private static bool FindRoot( ref string OutDirectory )
        {
            bool    Result              = false;
            string  CurrentDirectory    = Directory.GetCurrentDirectory();
            string  ProjectFile         = Solution.ExpectedFileName;
            string  ProjectLocation     = Path.Combine( CurrentDirectory,
                ProjectFile );

            for( ; !File.Exists( ProjectLocation ); )
            {
                try
                {
                    DirectoryInfo Parent =
                        Directory.GetParent( CurrentDirectory );

                    // If CurrentDirectory is the root directory, the returned
                    // parent directory is null. Check before attempting to use
                    // it to prevent a possible NullReferenceException error.
                    if( null != Parent )
                    {
                        CurrentDirectory = Parent.FullName;
                    }
                    else
                    {
                        // Do no further processing -- it has been checked
                        // through the root directory that there is no
                        // file matching the one being searched for.
                        return false;
                    }

                    // Update location to parent location.
                    ProjectLocation =
                        Path.Combine( CurrentDirectory, ProjectFile );
                }
                catch( Exception exception )
                {
                    Console.WriteLine( exception );
                    break;
                }
            }

            ProjectLocation = Path.Combine( CurrentDirectory, ProjectFile );

            // Double check -- the only way to actually get here is to have
            // either found the file, or gotten an exception in
            // Directory.GetParent.
            if( File.Exists( ProjectLocation ) )
            {
                OutDirectory    = CurrentDirectory;
                Result          = true;
            }

            return Result;
        }

        /// <summary>
        /// </summary>
        /// <return>
        /// </return>
        public static bool SetRootDirectory()
        {
            bool Result = false;

            try
            {
                string RootDir = null;
                if( FindRoot( ref RootDir ) )
                {
                    _rootDirectory = RootDir;

                    // Set the applications current working directory to the directory
                    // containing the first found project.
                    Directory.SetCurrentDirectory( RootDirectory );

                    // The directory has been set, so this has been a successful
                    // endeavor down the stack.
                    Result = true;
                }
            }
            catch( Exception exception )
            {
                Console.WriteLine( exception );
            }

            return Result;
        }

    }

} // namespace Sprove

