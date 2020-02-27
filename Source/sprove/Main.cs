// Copyright (c) 2020 Anthony Smith. All rights reserved.
using System;

namespace Sprove
{

    class Sprove
    {

        static void DisplayHelp()
        {}

        static int Main( string[] Arguments )
        {
            if( !Cache.Initialize() )
            {
                return 1;
            }

            return 0;
        }

    }

}

