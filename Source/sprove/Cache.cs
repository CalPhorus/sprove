// Copyright (c) 2020 Anthony Smith. All rights reserved.
using System;

namespace Sprove
{

    /// <summary>
    ///
    /// </summary>
    static internal class Cache
    {

        private static readonly string _cacheDir = ".sprove";

        /// <summary>
        ///
        /// </summary>
        public static string CacheDir { get { return _cacheDir; } }

    }

} // namespace Sprove

