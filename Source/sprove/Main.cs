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

namespace Sprove
{

    internal class SproveOptions
    {
        [Option( ShortCode = "-t", LongCode = "--target" )]
        [HelpText( "What operating system should projects be built for? Default: host system. Example: --target:Linux." )]
        public BuildTarget target;

        [Option( LongCode = "--config" )]
        [HelpText( "What configuration should projects be built for? Default: Debug. Example: --config:Production." )]
        public BuildConfig config = BuildConfig.Debug;

        [Option( LongCode = "--create-solution" )]
        [HelpText( "Create a solution that can be built with this tool." )]
        public string solutionName = string.Empty;

        [Verb( Code = "--test" )]
        [HelpText( "Run tests as part of the build." )]
        public bool runTests = false;

        [Verb( Code = "-h" )]
        [HelpText( "Display this text" )]
        public bool help = false;

#if SPROVE_BOOTSTRAP
#else // SPROVE_BOOTSTRAP
        [Verb( Code = "-v" )]
        [HelpText( "Get version info." )]
        public bool version = false;
#endif // SPROVE_BOOTSTRAP

    }

    internal class Sprove
    {

        static int Main( string[] arguments )
        {
            SproveOptions              options = new SproveOptions();
            CLIParser<SproveOptions>   parser  = new CLIParser<SproveOptions>();
            List<string>               extras  = new List<string>();

            try
            {
                options.target = Target.Host();
            }
            catch( UnknownHostException unknownHost )
            {
                Console.WriteLine( unknownHost );
                return 1;
            }

            if( !SolutionRoot.SetRootDirectory() )
            {
                return 1;
            }

            if( !Cache.Initialize() )
            {
                return 1;
            }

            if( !parser.Parse( arguments, ref options, ref extras ) )
            {
                return 1;
            }

            if( 0 < extras.Count )
            {
                bool isBad = false;
                foreach( string cmdArg in extras )
                {
                    if( cmdArg.StartsWith( "-" ) )
                    {
                        Console.WriteLine( "Unknown command line option: {0}", cmdArg );
                        isBad = true;
                    }
                }

                if( isBad )
                {
                    return 1;
                }
            }

            if( !SproveDirectory.Initialize() )
            {
                return 1;
            }

            if( options.help )
            {
                parser.DisplayHelp();
                return 0;
            }

#if SPROVE_BOOTSTRAP
#else // SPROVE_BOOTSTRAP
            if( options.version )
            {
                string versionString = global::Sprove.Version.GetVersionString();
                Console.WriteLine( "sprove - version {0}. Copyright (c) 2020 Anthony Smith.",
                    versionString );
                return 0;
            }
#endif // SPROVE_BOOTSTRAP
            if( string.Empty != options.solutionName )
            {
                //TODO(anthony): Generate file.
                return 0;
            }

            SolutionLoader loader = new SolutionLoader();

            Target target = new Target( options.config, options.target );
            if( !loader.Load( SolutionRoot.RootDirectory, target ) )
            {
                return 1;
            }

            Solution loadedSolution = loader.LoadedSolution;

            if( !loadedSolution.PreBuild() )
            {
                return 1;
            }

            ProjectBuilder projectBuilder = new ProjectBuilder();
            foreach( Project project in loadedSolution.Projects )
            {
                if( !projectBuilder.Build( project ) )
                {
                    return 1;
                }
            }

            return 0;
        }

    }

}

