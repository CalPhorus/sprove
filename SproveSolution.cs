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
using Sprove;

class SproveSolution : Solution
{
    struct Version
    {
        public int major;
        public int minor;
        public int patch;
    }

    private Version version;
    private string  sourceDir;
    private Project sprove;

    public SproveSolution( Target target ) : base( target )
    {
        sourceDir       = Path.Combine( "Source", "sprove" );
        version         = new Version();
        version.major   = 0;
        version.minor   = 0;
        version.patch   = 0;
        sprove          = CreateProject( "sprove" );

        sprove
            .AddSourceFiles(
                new string[]
                {
                    sourceDir + "/UnknownHostException.cs",
                    sourceDir + "/VerbTypeException.cs",
                    sourceDir + "/HelpTextAttribute.cs",
                    sourceDir + "/OptionAttribute.cs",
                    sourceDir + "/SolutionLoader.cs",
                    sourceDir + "/VerbAttribute.cs",
                    sourceDir + "/WarningLevel.cs",
                    sourceDir + "/SolutionRoot.cs",
                    sourceDir + "/BuildConfig.cs",
                    sourceDir + "/CompileData.cs",
                    sourceDir + "/BuildTarget.cs",
                    sourceDir + "/CLIParser.cs",
                    sourceDir + "/Solution.cs",
                    sourceDir + "/Compiler.cs",
                    sourceDir + "/Project.cs",
                    sourceDir + "/ProjectBuilder.cs",
                    sourceDir + "/SproveDirectory.cs",
                    sourceDir + "/Target.cs",
                    sourceDir + "/Cache.cs",
                    sourceDir + "/Main.cs",
                }
            );
    }

    public override bool PreBuild()
    {
        if( !base.PreBuild() )
        {
            return false;
        }

        string versionText = @"
// Copyright 2020 Anthony Smith
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the ""Software""), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using System;

namespace Sprove
{{
    internal static class Version
    {{
        private static int _major = {0};
        private static int _minor = {1};
        private static int _patch = {2};

        /// <summary>
        ///
        /// </summary>
        public static int Major
        {{
            get{{ return _major; }}
        }}

        /// <summary>
        ///
        /// </summary>
        public static int Minor
        {{
            get{{ return _minor; }}
        }}

        /// <summary>
        ///
        /// </summary>
        public static int Patch
        {{
            get{{ return _patch; }}
        }}

        /// <summary>
        ///
        /// </summary>
        public static string GetVersionString()
        {{
            return _major + ""."" + _minor + ""."" + _patch;
        }}
    }}
}} // namespace Sprove

";

        string versionOutput = string.Format( versionText, version.major,
            version.minor, version.patch );

        string versionFile = CreateFile( "Version.cs", versionOutput );
        if( string.Empty != versionFile )
        {
            sprove.AddSourceFiles( versionFile );
        }
        else
        {
            return false;
        }

        return true;
    }

}

