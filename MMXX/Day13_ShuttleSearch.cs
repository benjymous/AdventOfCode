using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXX
{
    public class Day13 : IPuzzle
    {
        public string Name { get { return "2020-13";} }
 
        static UInt64 NextMultiple(UInt64 n, UInt64 mult)
        {
            return ((n+(mult-1))/mult) * mult;
        }

        public static UInt64 Part1(string input)
        {
            var lines = Util.Split(input,'\n');
            var timestamp = UInt64.Parse(lines[0]);
            var times = Util.Split(lines[1]);
            var smallest = UInt64.MaxValue;
            UInt64 bestbus = 0;
            foreach(var t in times)
            {
                if (t=="x") continue;
                var bus = UInt64.Parse(t);
                var nexttime = NextMultiple(timestamp, bus); 
                Console.WriteLine($"{bus} - {nexttime}");
                if (nexttime<smallest)
                {
                    smallest=nexttime;
                    bestbus=bus;
                }
            }
            return (smallest-timestamp)*bestbus;
        }

        static bool Check(UInt64 i, List<Tuple<int,int>> vals)
        {
            foreach(var t in vals)
            {
                if ((i+(UInt64)t.Item2) % (UInt64)t.Item1 != 0)
                {
                    return false;
                }
            }
            return true;
        }
        public static UInt64 Part2x(string input, UInt64 start)
        {
            var lines = Util.Split(input,'\n');
            var times = Util.Split(lines[1]);
            int i = 0;
            var nums = new List<Tuple<int,int>>();
            foreach(var t in times)
            {
                if (t!="x")
                {
                    nums.Add(Tuple.Create(Int32.Parse(t), i));
                }
                i++;
            }

            var m = (UInt64)nums.First().Item1;
            
            var n = NextMultiple(start, m);

            while(true) 
            {
                if (Check(n, nums)) 
                {
                    return n;
                }
                n+= m;
                if (n%100000000 ==0) Console.WriteLine(n);
            }

            
        }

        public static UInt64 Part2(string input)
        {
            var lines = Util.Split(input,'\n');
            var times = Util.Split(lines[1]);
            
            var nums = new List<Tuple<UInt64,UInt64>>();
            UInt64 i = 0;
            foreach(var t in times)
            {
                if (t!="x")
                {
                    nums.Add(Tuple.Create(UInt64.Parse(t), i));
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
                inc = CalcLCM(inc, n.Item1);
            }

            return res;
        }

        static UInt64 CalcLCM(UInt64 a, UInt64 b)
        {
            UInt64 num1, num2;
            if (a > b)
            {
                num1 = a; num2 = b;
            }
            else
            {
                num1 = b; num2 = a;
            }

            for (UInt64 i = 1; i < num2; i++)
            {
                if ((num1 * i) % num2 == 0)
                {
                    return i * num1;
                }
            }
            return num1 * num2;
        }

        public void Run(string input, ILogger logger)
        {
            Console.WriteLine(Part2("939\n7,13,x,x,59,x,31,19"));
            //logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
            //104374000000000
        }
    }
}