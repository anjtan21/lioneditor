﻿using System.Collections.Generic;

namespace FFTPatcher.TextEditor
{
    class CompressibleOneShotFile : AbstractFile
    {
        public CompressibleOneShotFile( GenericCharMap map, FFTTextFactory.FileInfo layout, IList<IList<string>> strings )
            : base( map, layout, strings, true )
        {
        }

        public CompressibleOneShotFile( GenericCharMap map, FFTPatcher.TextEditor.FFTTextFactory.FileInfo layout, IList<byte> bytes )
            : base( map, layout, true )
        {
            List<IList<string>> sections = new List<IList<string>>( NumberOfSections );
            System.Diagnostics.Debug.Assert( NumberOfSections == 1 );
            for ( int i = 0; i < NumberOfSections; i++ )
            {
                sections.Add( TextUtilities.ProcessList( TextUtilities.Decompress( bytes, bytes, 0 ), map ) );
                if ( sections[i].Count < SectionLengths[i] )
                {
                    string[] newSection = new string[SectionLengths[i]];
                    sections[i].CopyTo( newSection, 0 );
                    new string[SectionLengths[i] - sections[i].Count].CopyTo( newSection, sections[i].Count );
                    sections[i] = newSection;
                }
            }
            Sections = sections.AsReadOnly();
        }

        protected override IList<byte> ToByteArray()
        {
            IList<uint> offsets;
            return Compress( this.Sections, out offsets );
        }

        protected override IList<byte> ToByteArray( IDictionary<string, byte> dteTable )
        {
            IList<uint> offsets;
            return Compress( dteTable, out offsets );
        }
    }
}
