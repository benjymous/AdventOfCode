using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day25 : IPuzzle
    {
        public string Name => "2022-25";

        static readonly Dictionary<char, int> DigitChars = new()
        {
            { '0', 0 },
            { '1', 1 },
            { '2', 2 },
            { '-', -1 },
            { '=', -2 }
        };

        static readonly Dictionary<long, char> CharDigits = new()
        {
            { 0, '0' },
            { 1, '1' },
            { 2, '2' },
            { -1, '-' },
            { -2, '=' }
        };

        public static long SnafuToLong(string snafu)
        {
            long val = 0;

            for (long i = snafu.Length - 1, mult=1; i >= 0; i--, mult*=5)
            {
                val += mult * DigitChars[snafu[(int)i]];
            }
            return val;
        }

        public static string IntToSnafu(long val)
        {
            List<char> res = new();
            while (val > 0)
            {
                val = Math.DivRem(val, 5, out var rem);
                if (rem > 2)
                {
                    rem -= 5;
                    val++;
                }
                res.Add(CharDigits[rem]);               
            }
            
            return ((IEnumerable<char>)res).Reverse().AsString();
        }

        public static string Part1(string input)
        {
            return IntToSnafu(Util.Split(input).Sum(SnafuToLong));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}