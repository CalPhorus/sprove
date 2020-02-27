// Copyright (c) 2020 Anthony Smith. All rights reserved.
using System;
using System.IO;

namespace Sprove
{

    /// <summary>
    ///
    /// </summary>
    static internal class Cache
    {

        private static bool             _isInit         = false;
        private static readonly string  _cacheDir       = ".sprove";
        private static readonly string  _cacheTmpDir    =
            Path.Combine( CacheDir, "tmp" );

        /// <summary>
        /// Evaluates to `true` if the cache is initialized, `false` otherwise.
        /// </summary>
        public static bool IsInit{ get{ return _isInit; } }

        /// <summary>
        /// The location of the cache directory, relative to the root directory
        /// used for the top-level project.
        /// </summary>
        public static string CacheDir { get { return _cacheDir; } }

        /// <summary>
        /// The temporary directory used by the application.
        /// </summary>
        public static string CacheTmpDir{ get{ return _cacheTmpDir; } }


        /// <summary>
        /// Initializes the cache, creating the directories and files
        /// required by the application.
        /// </summary>
        /// <return>
        /// Returns `true` on successful initialization, `false` on error.
        /// </return>
        public static bool Initialize()
        {
            if( IsInit )
            {
                // Already initialized -- don't do things twice.
                return true;
            }

            // Default to failure, only setting to success on success.
            bool Result = false;

            try
            {
                DirectoryInfo cacheDirInfo =
                    Directory.CreateDirectory( CacheDir );

                // The cache directory should be a hidden directory. On *nix,
                // the way to make a directory hidden is to have the name of
                // the directory start with the character '.'. On other systems,
                // such as Windows, flags need to be set. C# takes care of this
                // with the FileAttributes.Hidden flag.
                if( !cacheDirInfo.Attributes.HasFlag( FileAttributes.Hidden ) )
                {
                    cacheDirInfo.Attributes |= FileAttributes.Hidden;
                }

                Directory.CreateDirectory( CacheTmpDir );

                Result = true;
            }
            catch( Exception exception )
            {
                Console.WriteLine( exception );
            }

            // Set initialized by the result of the initialization.
            _isInit = Result;
            return Result;
        }
    }

} // namespace Sprove

