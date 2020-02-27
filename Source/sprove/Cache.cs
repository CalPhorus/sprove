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

                // If this fails, an exception will be thrown, so don't check
                // the return value.
                Directory.CreateDirectory( CacheTmpDir );

                // Initialization successful at this point.
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

