// Copyright (c) 2020 Anthony Smith. All rights reserved.
using System;

namespace Sprove
{

    internal class Sprove
    {

        static void DisplayHelp()
        {}

        static int Main( string[] Arguments )
        {
            if( !SolutionRoot.SetRootDirectory() )
            {
                return 1;
            }

            if( !Cache.Initialize() )
            {
                return 1;
            }

            return 0;
        }

    }

}

