using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day13 : IPuzzle
    {
        public string Name => "2020-13";

        static UInt64 NextMultiple(UInt64 n, UInt64 mult)
        {
            return ((n + (mult - 1)) / mult) * mult;
        }

        public static UInt64 Part1(string input)
        {
            var lines = Util.Split(input, '\n');
            var timestamp = UInt64.Parse(lines[0]);
            var times = Util.Split(lines[1]);
            var smallest = UInt64.MaxValue;
            UInt64 bestbus = 0;
            foreach (var t in times)
            {
                if (t == "x") continue;
                var bus = UInt64.Parse(t);
                var nexttime = NextMultiple(timestamp, bus);
                Console.WriteLine($"{bus} - {nexttime}");
                if (nexttime < smallest)
                {
                    smallest = nexttime;
                    bestbus = bus;
                }
            }
            return (smallest - timestamp) * bestbus;
        }

        static bool Check(UInt64 i, List<(int, int)> vals)
        {
            foreach (var t in vals)
            {
                if ((i + (UInt64)t.Item2) % (UInt64)t.Item1 != 0)
                {
                    return false;
                }
            }
            return true;
        }
        public static UInt64 Part2x(string input, UInt64 start)
        {
            var lines = Util.Split(input, '\n');
            var times = Util.Split(lines[1]);
            int i = 0;
            var nums = new List<(int, int)>();
            foreach (var t in times)
            {
                if (t != "x")
                {
                    nums.Add((Int32.Parse(t), i));
                }
                i++;
            }

            var m = (UInt64)nums.First().Item1;

            var n = NextMultiple(start, m);

            while (true)
            {
                if (Check(n, nums))
                {
                    return n;
                }
                n += m;
                if (n % 100000000 == 0) Console.WriteLine(n);
            }


        }

        public static UInt64 Part2(string input)
        {
            var lines = Util.Split(input, '\n');
            var times = Util.Split(lines[1]);

            var nums = new List<(UInt64, UInt64)>();
            UInt64 i = 0;
            foreach (var t in times)
            {
                if (t != "x")
                {
                    nums.Add((UInt64.Parse(t), i));
                }
                i++;
            }
            UInt64 res = nums[0].Item1;
            var inc = res;

            foreach (var n in nums.Skip(1))
            {
                var mod = n.Item1 - (n.Item2 % n.Item1);
                while (res % n.Item1 != mod)
                {
                    res += inc;
                }
                inc = Util.LCM(inc, n.Item1);
            }

            return res;
        }


        public void Run(string input, ILogger logger)
        {
            Console.WriteLine(Part2("939\n7,13,x,x,59,x,31,19"));
            //logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
            //104374000000000
        }
    }
}