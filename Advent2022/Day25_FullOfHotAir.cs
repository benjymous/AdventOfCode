using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day25 : IPuzzle
    {
        public string Name => "2022-25";

        static readonly IEnumerable<(char c, int v)> snafuCodex = "=-012".Select((c, i) => (c, i - 2));
        static readonly Dictionary<char, int> CharToDecimal = snafuCodex.ToDictionary(v => v.c, v => v.v);
        static readonly Dictionary<int, char> CharDigits = snafuCodex.ToDictionary(v => v.v, v => v.c);

        public static long SnafuToDecimal(string snafu)
        {
            long val = 0;

            for (long i = snafu.Length - 1, mult=1; i >= 0; i--, mult*=5)
            {
                val += mult * CharToDecimal[snafu[(int)i]];
            }
            return val;
        }

        public static string DecimalToSnafu(long value)
        {
            Stack<char> res = new(25);
            while (value > 0)
            {
                value = Math.DivRem(value, 5, out var remainder);
                if (remainder > 2)
                {
                    remainder -= 5;
                    value++;
                }
                res.Push(CharDigits[(int)remainder]);
            }

            return res.AsString();
        }

        public static string Part1(string input)
        {
            return DecimalToSnafu(Util.Split(input).Sum(SnafuToDecimal));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}