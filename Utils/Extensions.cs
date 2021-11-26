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
                yield return ((v & k) > 0) ? true : false;
            }
        }


        public static string ToEngineeringNotation(this double d)
        {
            string fmt = "N2";
            double exponent = Math.Log10(Math.Abs(d));
            if (Math.Abs(d) >= 1)
            {
                switch ((int)Math.Floor(exponent))
                {
                    case 0:
                    case 1:
                    case 2:
                        return d.ToString();
                    case 3:
                    case 4:
                    case 5:
                        return (d / 1e3).ToString(fmt) + "k";
                    case 6:
                    case 7:
                    case 8:
                        return (d / 1e6).ToString(fmt) + "M";
                    case 9:
                    case 10:
                    case 11:
                        return (d / 1e9).ToString(fmt) + "G";
                    case 12:
                    case 13:
                    case 14:
                        return (d / 1e12).ToString(fmt) + "T";
                    case 15:
                    case 16:
                    case 17:
                        return (d / 1e15).ToString(fmt) + "P";
                    case 18:
                    case 19:
                    case 20:
                        return (d / 1e18).ToString(fmt) + "E";
                    case 21:
                    case 22:
                    case 23:
                        return (d / 1e21).ToString(fmt) + "Z";
                    default:
                        return (d / 1e24).ToString(fmt) + "Y";
                }
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

        public static string MultipleWithS(this int value, string str)
        {
            if (value == 1)
            {
                return str;
            }
            else
            {
                return $"{value} {str}s";
            }
        }
    }


}