// Copyright (c) 2020 Anthony Smith. All rights reserved.

namespace Sprove
{

    /// <summary>
    /// The warning level to set the compiler.
    /// </summary>
    public enum WarningLevel : int
    {
        /// <summary>
        /// This is the equivalent of -warn:0.
        /// </summary>
        Level0 = 0,
        /// <summary>
        /// This is the equivalent of -warn:1.
        /// </summary>
        Level1,
        /// <summary>
        /// This is the equivalent of -warn:2.
        /// </summary>
        Level2,
        /// <summary>
        /// This is the equivalent of -warn:3.
        /// </summary>
        Level3,
        /// <summary>
        /// This is the equivalent of -warn:4.
        /// </summary>
        Level4
    }

} // namespace Sprove

