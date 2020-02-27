// Copyright (c) 2020 Anthony Smith. All rights reserved.
using System;
using System.IO;

namespace Sprove
{

    /// <summary>
    /// Finds the first directory that contains a SproveProject.cs file.
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
            string  ProjectFile         = "SproveProject.cs";
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

