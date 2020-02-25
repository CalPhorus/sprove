// Copyright (c) 2020 Anthony Smith. All rights reserved.
using System;
using System.Collections.Generic;

namespace Sprove
{

    /// <summary>
    ///
    /// </summary>
    internal struct CompileData
    {

        /// <summary>
        ///
        /// </summary>
        public List<string> defines;

        /// <summary>
        ///
        /// </summary>
        public bool includeDebugInfo;

        /// <summary>
        ///
        /// </summary>
        public bool isLibrary;

        /// <summary>
        ///
        /// </summary>
        public string name;

        /// <summary>
        ///
        /// </summary>
        public List<string> referencedAssemblies;

        /// <summary>
        ///
        /// </summary>
        public List<string> sourceFiles;

        /// <summary>
        ///
        /// </summary>
        public bool warningsAreErrors;

        /// <summary>
        ///
        /// </summary>
        public WarningLevel warningLevel;

    }

} // namespace Sprove

