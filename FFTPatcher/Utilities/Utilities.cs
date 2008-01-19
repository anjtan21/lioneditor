﻿/*
    Copyright 2007, Joe Davidson <joedavidson@gmail.com>

    This file is part of FFTPatcher.

    FFTPatcher is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    FFTPatcher is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with FFTPatcher.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;

namespace FFTPatcher
{
    public static class Utilities
    {
        public static byte ByteFromBooleans( bool msb, bool six, bool five, bool four, bool three, bool two, bool one, bool lsb )
        {
            bool[] flags = new bool[] { lsb, one, two, three, four, five, six, msb };
            byte result = 0;

            for( int i = 0; i < 8; i++ )
            {
                if( flags[i] )
                {
                    result |= (byte)(1 << i);
                }
            }

            return result;
        }

        public static byte MoveToUpperAndLowerNibbles( int upper, int lower )
        {
            return (byte)(((upper & 0x0F) << 4) | (lower & 0x0F));
        }

        /// <summary>
        /// Creates an array of booleans from a byte. Index 0 in the array is the least significant bit.
        /// </summary>
        public static bool[] BooleansFromByte( byte b )
        {
            bool[] result = new bool[8];
            for( int i = 0; i < 8; i++ )
            {
                result[i] = ((b >> i) & 0x01) > 0;
            }

            return result;
        }

        /// <summary>
        /// Creates an array of booleans from a byte. Index 0 in the array is the most significant bit.
        /// </summary>
        public static bool[] BooleansFromByteMSB( byte b )
        {
            bool[] result = new bool[8];
            for( int i = 0; i < 8; i++ )
            {
                result[i] = ((b >> (7 - i)) & 0x01) > 0;
            }

            return result;
        }

        public static void CopyBoolArrayToBooleans( bool[] bools,
            ref bool msb,
            ref bool six,
            ref bool five,
            ref bool four,
            ref bool three,
            ref bool two,
            ref bool one,
            ref bool lsb )
        {
            lsb = bools[0];
            one = bools[1];
            two = bools[2];
            three = bools[3];
            four = bools[4];
            five = bools[5];
            six = bools[6];
            msb = bools[7];
        }

        public static void CopyByteToBooleans( byte b,
            ref bool msb,
            ref bool six,
            ref bool five,
            ref bool four,
            ref bool three,
            ref bool two,
            ref bool one,
            ref bool lsb )
        {
            CopyBoolArrayToBooleans( BooleansFromByte( b ), ref msb, ref six, ref five, ref four, ref three, ref two, ref one, ref lsb );
        }

        public static UInt16 BytesToUShort( byte lsb, byte msb )
        {
            UInt16 result = 0;
            result += lsb;
            result += (UInt16)(msb << 8);
            return result;
        }

        public static UInt32 BytesToUInt32( byte[] bytes )
        {
            UInt32 result = 0;
            result += bytes[0];
            result += (UInt32)(bytes[1] << 8);
            result += (UInt32)(bytes[2] << 16);
            result += (UInt32)(bytes[3] << 24);

            return result;
        }

        public static bool CompareArrays<T>( T[] one, T[] two ) where T : IComparable, IEquatable<T>
        {
            if( one.Length != two.Length )
                return false;
            for( long i = 0; i < one.Length; i++ )
            {
                if( !one[i].Equals( two[i] ) )
                    return false;
            }

            return true;
        }
    }
}