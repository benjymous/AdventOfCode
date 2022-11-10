using System;
using System.Collections.Generic;

namespace AoC.Utils
{
    public static class Extensions
    {
        // iterate over bits, returns sequence like 1,2,4,8 (only returning set bits in input)
        public static IEnumerable<int> BitSequence(this int v)
        {
            for (int k = 1; k <= v; k <<= 1)
            {
                if ((v & k) > 0)
                    yield return k;
            }
        }

        public static IEnumerable<byte> BitSequence(this byte v)
        {
            for (byte k = 1; k <= v; k <<= 1)
            {
                if ((v & k) > 0)
                    yield return k;
            }
        }

        public static IEnumerable<int> BinarySequence(this int v)
        {
            for (Int64 k = 1; k <= v; k <<= 1)
            {
                yield return ((v & k) > 0) ? 1 : 0;
            }
        }

        public static IEnumerable<Int64> BitSequence(this Int64 v)
        {
            for (Int64 k = 1; k <= v; k <<= 1)
            {
                if ((v & k) > 0)
                    yield return k;
            }
        }

        public static IEnumerable<bool> BinarySequence(this Int64 v)
        {
            for (Int64 k = 1; k <= v; k <<= 1)
            {
                yield return ((v & k) > 0);
            }
        }

        public static IEnumerable<int> BinarySequenceBigEndian(this int v, int start=8)
        {
            for (int i = start; i > 0; i >>= 1) yield return (v & i) > 0 ? 1 : 0;
        }


        public static string ToEngineeringNotation(this double d)
        {
            string fmt = "N2";
            double exponent = Math.Log10(Math.Abs(d));
            if (Math.Abs(d) >= 1)
            {
                return (int)Math.Floor(exponent) switch
                {
                    0 or 1 or 2 => d.ToString(),
                    3 or 4 or 5 => (d / 1e3).ToString(fmt) + "k",
                    6 or 7 or 8 => (d / 1e6).ToString(fmt) + "M",
                    9 or 10 or 11 => (d / 1e9).ToString(fmt) + "G",
                    12 or 13 or 14 => (d / 1e12).ToString(fmt) + "T",
                    15 or 16 or 17 => (d / 1e15).ToString(fmt) + "P",
                    18 or 19 or 20 => (d / 1e18).ToString(fmt) + "E",
                    21 or 22 or 23 => (d / 1e21).ToString(fmt) + "Z",
                    _ => (d / 1e24).ToString(fmt) + "Y",
                };
            }
            else if (Math.Abs(d) > 0)
            {
                return d.ToString(fmt);
                // switch ((int)Math.Floor(exponent))
                // {
                //     case -1: case -2: case -3:
                //         return (d * 1e3).ToString(fmt) + "m";
                //     case -4: case -5: case -6:
                //         return (d * 1e6).ToString(fmt) + "μ";
                //     case -7: case -8: case -9:
                //         return (d * 1e9).ToString(fmt) + "n";
                //     case -10: case -11: case -12:
                //         return (d * 1e12).ToString(fmt) + "p";
                //     case -13: case -14: case -15:
                //         return (d * 1e15).ToString(fmt) + "f";
                //     case -16: case -17: case -18:
                //         return (d * 1e18).ToString(fmt) + "a";
                //     case -19: case -20: case -21:
                //         return (d * 1e21).ToString(fmt) + "z";
                //     default:
                //         return (d * 1e24).ToString() + "y";
                // }
            }
            else
            {
                return "0";
            }
        }

        public static int ParseHex(this char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'A' && c <= 'F') return c - 'A' + 10;
            if (c >= 'a' && c <= 'f') return c - 'a' + 10;
            throw new Exception("Bad hex");
        }
    }


}