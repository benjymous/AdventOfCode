using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day04 : IPuzzle
    {
        public string Name { get { return "2019-04";} }

        public static bool CheckCriteria(string num, bool strict)
        {
            // Two adjacent digits are the same (like 22 in 122345).
            // Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679).
            bool adjacent = false;
            var pairs = new Dictionary<char, bool>();
            for (var i=0; i<5; i++) 
            {
                if (num[i]==num[i+1])
                {
                    adjacent = true;
                    pairs[num[i]] = true;
                }
                if (num[i] > num[i+1])
                {
                    // decreasing
                    return false;
                }
            }
            if (!adjacent) return false;

            if (strict)
            {
                // the two adjacent matching digits are not part of a larger group of matching digits
                for (var i=0; i<4; i++) 
                {
                    if (num[i]==num[i+1] && num[i+1]==num[i+2])
                    {
                        pairs[num[i]] = false;
                        // actually a triple
                    }
                }
            }

            return pairs.Values.Where(v => v==true).Any();
        }
 
        public static int Part1(string input)
        {
            var data = input.Split("-");
            var low = int.Parse(data[0]);
            var high = int.Parse(data[1]);

            int count = 0;

            for (var i=low; i<high; ++i) 
            {
                if (CheckCriteria(i.ToString(), false)) count++;
            }

            return count;
        }

        public static int Part2(string input)
        {
            var data = input.Split("-");
            var low = int.Parse(data[0]);
            var high = int.Parse(data[1]);

            int count = 0;

            for (var i=low; i<high; ++i) 
            {
                if (CheckCriteria(i.ToString(), true)) count++;
            }

            return count;
        }

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - "+Part1(input));
            Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
