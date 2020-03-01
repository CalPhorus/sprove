#!/usr/bin/env python3
import os
import sys
import subprocess

# The directory where this script is located
scriptPath      = os.path.dirname( os.path.realpath( __file__ ) )
# The name of the project
projectName     = "sprove"
# The path that the bootstrap will result in
sprovePath      = os.path.join( scriptPath, projectName ) + ".exe"
# The location of the project's source code
projectSrcPath  = os.path.join( scriptPath,
                    os.path.join( "Source", f"{projectName}" ) )
# Shortcut for subprocess.PIPE
subprocPipe     = subprocess.PIPE

# mcs is the C# compiler on non-Windows systems. This checks if the system is
# not Windows.
def UseMCS():
    return 'win32' != sys.platform

# Finds mcs on non-Windows systems.
def FindMCS():
    args    = [ "which", "mcs" ]
    which   = subprocess.run( args, stdout=subprocPipe, stderr=subprocPipe )

    if 0 == which.returncode:
        # Return without any newline characters
        return which.stdout.decode( "utf-8" ).replace( '\n', '' )

    notFoundMsg = f"mcs is needed in order to bootstrap {projectName}!"
    installMsg  = "Please install mcs before attempting to build again!"

    # Can't find it -- exit with message.
    raise SystemExit( f"{notFoundMsg} {installMsg}" )

# Finds csc.exe on Windows systems using the vswhere.exe binary in
# Source/Vendor/vswhere.
def FindCSC():
    args    = [ scriptPath + "/Source/Vendor/vswhere/vswhere.exe",
                "-latest", "-utf8", "-nologo",
                "-property", "installationPath", "-format", "value"
              ]
    which  = subprocess.run( args, stdout=subprocPipe, stderr=subprocPipe )

    # vswhere.exe always returns a 0 value, even if Visual Studio was not found.
    # Checking which.stdout solves the issue -- if a value is not an empty
    # string, then it was found. Otherwise, it was not.
    if b"" != which.stdout:
        # Return the path to csc.exe
        resWithNewlines = which.stdout.decode( "utf-8" )
        returnedPath    = resWithNewlines.replace( '\n', '' ).replace( '\r', '' )
        msbuildPath     = os.path.join( returnedPath, "MSBuild" )

        for dirpath, dirnames, filenames in os.walk( msbuildPath ):
            for filename in [ f for f in filenames if f.endswith( "csc.exe" ) ]:
                return os.path.join( dirpath, filename )

    notFoundMsg = "At least Visual Studio 2017 is required in order to"
    notFoundMsg = notFoundMsg + f" bootstrap {projectName}!"
    installMsg  = "Please install at least Visual Studio 2017 before attempting"
    installMsg  = installMsg + " to build again!"

    # Can't find it -- exit with message
    raise SystemExit( f"{notFoundMsg} {installMsg}" )

# Finds all .cs files in the provided directory, and all subdirectories in
# the provided directory.
def FindAllCSFilesInDir( directory ):
    files = []

    for dirpath, dirnames, filenames in os.walk( directory ):
        for filename in [ f for f in filenames if f.endswith( ".cs" ) ]:
            files.append( os.path.join( dirpath, filename ) )
    return files

class CompilerData:
    outputName  = f"{projectName}"
    defines     = []
    files       = []
    references  = []

# Class used to compile C# files.
class CSCompiler:
    def __init__(self):
        if UseMCS():
            self.compiler = FindMCS()
        else:
            self.compiler = FindCSC()

    def Compile( self, compilerData: CompilerData ):
        define      = ''
        references  = ''
        if len( compilerData.defines ):
            define = "-define:" + ','.join( compilerData.defines )

        if len( compilerData.references ):
            references = "-r:" + ','.join( compilerData.references )

        args    = [ self.compiler,
                    "-out:" + compilerData.outputName + ".exe",
                    define,
                    references,
                  ] + compilerData.files

        result  = subprocess.run( args, stdout=subprocPipe, stderr=subprocPipe )
        stdout  = result.stdout.decode( "utf-8" )
        stderr  = result.stderr.decode( "utf-8" )

        if "" != stdout:
            print( stdout )

        if "" != stderr:
            print( stderr )

        # Print version information if errors occur in build.
        if 0 != result.returncode:
            args    = [ self.compiler, "--version" ]
            version = subprocess.run( args, stdout=subprocPipe,
                stderr=subprocPipe )
            stdout = version.stdout.decode( "utf-8" )
            stderr = version.stderr.decode( "utf-8" )

            if "" != stdout:
                print( stdout )
            if "" != stderr:
                print( stderr )

        return result.returncode

def main():
    compileData = CompilerData()
    compiler    = CSCompiler()
    compileData.files.extend( FindAllCSFilesInDir( projectSrcPath ) )
    compileData.defines.append( "SPROVE_BOOTSTRAP" )
    #compileData.references.append( "System.Runtime.InteropServices.RuntimeInformation.dll" )

    returncode  = compiler.Compile( compileData )

    if 0 != returncode:
        return 1

    bootstrapArgs = []

    if UseMCS():
        bootstrapArgs.append( "mono" )

    bootstrapArgs.append( sprovePath )

    bootstrap = subprocess.run( bootstrapArgs )

    # Remove sprove.exe -- the bootstrap has been completed and the final
    # version of sprove.exe has been built. If there was an error,
    # the bootstrap of sprove.exe is going to be rebuilt anyways.
    os.remove( os.path.join( scriptPath, "sprove.exe" ) )

    # Return the result of the build.
    return bootstrap.returncode

if "__main__" == __name__:
    # raising a SystemExit exception is faster than calling sys.exit, as
    # sys.exit raises a SystemExit exception anyways.
    raise SystemExit( main() )

