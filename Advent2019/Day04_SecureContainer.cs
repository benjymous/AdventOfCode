using System;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day04 : IPuzzle
    {
        public string Name => "2019-04";

        public static bool HasAdjacentPair(int[] digits, bool strict)
        {
            // Two adjacent digits are the same (like 22 in 122345).
            int count = 1;
            for (var i = 1; i <= 6; i++)
            {
                if (i < 6 && digits[i] == digits[i - 1])
                {
                    if (strict) count++;
                    else return true;
                }
                else if (strict)
                {
                    if (count == 2) return true;
                    count = 1;
                }
            }

            return false;
        }

        static int ToNumber(int[] digits)
        {
            int res = 0;
            for (int i = 5, mult = 1; i >= 0; --i, mult *= 10) res += digits[i] * mult;
            return res;
        }

        static void IncDigits(int[] digits, int digit = 5)
        {
            if (++digits[digit] > 9)
            {
                digits[digit] = 0;
                IncDigits(digits, digit - 1);
            }
        }

        private static int CheckRange(string low, string high, bool strict)
        {
            int count = 0;
            var current = low.Select(x => x - '0').ToArray();
            int end = int.Parse(high);
            int digitCheck = high[0] - '0';

            while (true)
            {
                // Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679).
                for (int i = 0; i < 5; i++)
                {
                    if (current[i + 1] < current[i])
                    {
                        current[i + 1] = current[i];
                        if (i < 4) current[i + 2] = Math.Min(current[i], current[i + 2]);
                    }
                }

                if (current[0] >= digitCheck && ToNumber(current) >= end) break;
                if (HasAdjacentPair(current, strict)) count++;

                IncDigits(current);
            }

            return count;
        }

        public static int Part1(string input)
        {
            var data = input.Split("-");
            return CheckRange(data[0], data[1], false);
        }

        public static int Part2(string input)
        {
            var data = input.Split("-");
            return CheckRange(data[0], data[1], true);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
