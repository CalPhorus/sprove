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
using System.Globalization;
using System.Reflection;

namespace Sprove
{

    /// <summary>
    ///
    /// </summary>
    internal sealed class CLIParser<OptionType>
    {

        /// <summary>
        /// Default constructor.
        /// </summary>
        public CLIParser()
        {}

        private FieldInfo[] GetFields()
        {
            Type            type            = typeof( OptionType );
            BindingFlags    bindingFlags;

            bindingFlags = BindingFlags.NonPublic |
                           BindingFlags.Instance  |
                           BindingFlags.Public;

            return type.GetFields( bindingFlags );
        }

        private string Spaces( int count )
        {
            return new string( ' ', count );
        }

        private struct HelpData
        {
            public string   shortCode;
            public string   longCode;
            public string[] helpText;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="flags">
        /// </param>
        public void DisplayHelp()
        {
            FieldInfo[]     fields              = GetFields();
            List<HelpData>  help                = new List<HelpData>();
            int             longestShortCode    = 0;
            int             longestLongCode     = 0;
            int             longestHelpText     = 0;
            int             lineLength          = 0;
            string          lineSeparator       = string.Empty;

            foreach( FieldInfo field in fields )
            {
                Attribute[] attrs   = field.GetCustomAttributes( false )
                    as Attribute[];
                HelpData    data    = new HelpData();
                int         helpLen = 0;

                data.shortCode = string.Empty;
                data.longCode  = string.Empty;
                data.helpText  = new string[ 1 ];

                foreach( Attribute attr in attrs )
                {
                    if( attr is OptionAttribute )
                    {
                        OptionAttribute option = attr as OptionAttribute;
                        data.shortCode = option.ShortCode;
                        data.longCode  = option.LongCode;
                    }
                    else if( attr is VerbAttribute )
                    {
                        VerbAttribute verb = attr as VerbAttribute;
                        
                        if( verb.Code.StartsWith( "--" ) )
                        {
                            data.longCode = verb.Code;
                        }
                        else
                        {
                            data.shortCode = verb.Code;
                        }
                    }
                    else if( attr is HelpTextAttribute )
                    {
                        HelpTextAttribute helpText = attr as HelpTextAttribute;
                        string helpTextValue = helpText.HelpText;

                        string[]    split = helpTextValue.Split( ' ' );
                        int         lines = 1;
                        int         len   = 0;

                        for( int wordIndex = 0;
                             split.Length > wordIndex;
                             ++wordIndex )
                        {
                            // do not add space for last word.
                            int addSpace =
                                ( wordIndex + 1 < split.Length ? 1 : 0 );

                            // if greater than 50, move to next line.
                            if( len + split[ wordIndex ].Length + addSpace > 50 )
                            {
                                lines++;
                                len = 0;
                            }

                            len += split[ wordIndex ].Length + addSpace;
                        }

                        data.helpText = new string[ lines ];

                        int wordInd = 0;
                        for( int index = 0; lines > index; ++index )
                        {
                            string text = string.Empty;

                            for( ; split.Length > wordInd; ++wordInd )
                            {
                                int     addSpace =
                                    ( wordInd + 1 < split.Length ? 1 : 0 );
                                string  word     = split[ wordInd ];
                                int     lineLen;

                                lineLen = text.Length + word.Length +
                                    addSpace;

                                if( lineLen > 50 )
                                {
                                    // Move to next line
                                    break;
                                }

                                text += word + Spaces( addSpace );
                            }

                            data.helpText[ index ] = text;

                            if( helpLen < data.helpText[ index ].Length )
                            {
                                helpLen = data.helpText[ index ].Length;
                            }
                        }
                    }
                }

                if( longestShortCode < data.shortCode.Length )
                {
                    longestShortCode = data.shortCode.Length;
                }

                if( longestLongCode < data.longCode.Length )
                {
                    longestLongCode = data.longCode.Length;
                }

                if( longestHelpText < helpLen)
                {
                    longestHelpText = helpLen;
                }

                int currentLineLength = longestShortCode + longestLongCode +
                    longestHelpText + 9;

                if( lineLength < currentLineLength )
                {
                    lineLength = currentLineLength;
                }

                help.Add( data );
            }

            lineSeparator = new string( '-', lineLength );

            Console.WriteLine( "Usage: {0} [options]",
                Environment.GetCommandLineArgs()[ 0 ] );
            Console.WriteLine();

            Console.WriteLine( "Options:" );
            Console.WriteLine( lineSeparator );
            foreach( HelpData data in help )
            {
                // Format output: help text can have more than one line, 
                // so this renders each line. The flags will always be
                // output on the first line for the help text.
                for( int index = 0; data.helpText.Length > index; ++index )
                {
                    string text = string.Format( "| {0} | {1} |",
                        Spaces( longestShortCode ), Spaces( longestLongCode ) );

                    // Add flags to line if first line.
                    if( 0 == index )
                    {
                        int numSpacesShort =
                            longestShortCode - data.shortCode.Length;
                        int numSpacesLong  = 
                            longestLongCode - data.longCode.Length;

                        text = string.Format( "| {0}{1} | {2}{3} |",
                            Spaces( numSpacesShort ), data.shortCode,
                            data.longCode, Spaces( numSpacesLong ) );
                    }

                    string line = data.helpText[ index ];

                    int padding = lineLength - text.Length - 2 - line.Length;
                    text = string.Format( "{0} {1}{2}|", text, line,
                        Spaces( padding ) );

                    Console.WriteLine( text );
                }

                Console.WriteLine( lineSeparator );
            }
        }

        private bool SetField( ref OptionType flags, FieldInfo field, string cmd, string arg )
        {
            bool isEnum = field.FieldType.IsEnum;

            if( isEnum )
            {
                try
                {
                    field.SetValue( flags, Enum.Parse( field.FieldType, arg, true ) );
                }
                catch( Exception )
                {
                    Console.WriteLine( "Invalid argument: {0}:{1}", cmd, arg );
                    
                    string  expected    = string.Empty;
                    Type    enumType    = field.FieldType;
                    Array   enumValues  = Enum.GetValues( enumType );
                    int     index       = 0;

                    foreach( int item in enumValues )
                    {
                        int     spaces = ( index++ + 1 < enumValues.Length ? 1 : 0 );
                        string  value  = Enum.GetName( enumType, item );
                        expected += cmd + ":" + value + Spaces( spaces );
                    }

                    Console.WriteLine( "Valid values are: {0}", expected );

                    return false;
                }

                return true;
            }
            else
            {
                // Attempt to set it as a string
                try
                {
                    field.SetValue( flags, arg );
                }
                catch( Exception exception )
                {
                    Console.WriteLine( exception );
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="arguments">
        /// </param>
        /// <param name="flags">
        /// </param>
        /// <param name="extras">
        /// </param>
        public bool Parse( string[] arguments, ref OptionType flags,
                           ref List<string> extras )
        {
            FieldInfo[] fields = GetFields();

            for( int index = 0; arguments.Length > index; ++index )
            {

                CultureInfo invariant   = CultureInfo.InvariantCulture;
                bool        captured    = false;
                string      arg         =
                    arguments[ index ].ToUpper( invariant );

                foreach( FieldInfo field in fields )
                {
                    object[] attrs;

                    attrs = field.GetCustomAttributes( false );

                    foreach( object attr in attrs )
                    {
                        if( attr is VerbAttribute )
                        {
                            VerbAttribute verb = attr as VerbAttribute;
                            // All types associated with a VerbAttribute should
                            // be a `bool` type.
                            if( typeof( bool ) != field.FieldType )
                            {
                                string error = "A boolean type was expected for a verb!";
                                throw new VerbTypeException( error );
                            }

                            string code = verb.Code.ToUpper( invariant );


                            if( arg.Equals( code ) )
                            {
                                field.SetValue( flags, true );

                                // The argument was captured by the requested
                                // flags.
                                captured = true;
                            }
                        }
                        else if( attr is OptionAttribute )
                        {
                            // arguments passed via command line should use
                            // ":" to separate the flag and the value the
                            // flag will be set to.
                            string[] args = arg.Split( ':' );

                            if( 2 != args.Length )
                            {
                                // There should be exactly two elements in the
                                // array if it is not a verb attribute or extra.
                                // It could be a verb that has not been iterated
                                // through quite yet.
                                continue;
                            }

                            OptionAttribute option    = attr as OptionAttribute;
                            string          longCode  = option.LongCode;
                            string          shortCode = option.ShortCode;
                            string          cmd       = args[ 0 ];

                            // Pass to SetField what was passend on the command
                            // line, not what is being used to test.
                            string[]        split     = arguments[ index ].Split( ':' );
                            string          cmd2      = split[ 0 ];
                            string          value     = split[ 1 ];

                            longCode  = longCode.ToUpper( invariant );
                            shortCode = shortCode.ToUpper( invariant );
                            cmd       = cmd.ToUpper( invariant );

                            if( cmd == shortCode || cmd == longCode )
                            {
                                if( !SetField( ref flags, field, cmd2, value ) )
                                {
                                    return false;
                                }
                                captured = true;
                            }
                        }
                    }
                }

                if( !captured )
                {
                    extras.Add( arguments[ index ] );
                }
            }

            return true;
        }
    }

} // namespace Sprove

