using System;
using System.Collections.Generic;
using System.Text;

namespace Advent.MMXIX
{
    public class Day01 : IPuzzle
    {
        public string Name { get { return "2019-01";} }

        public static int GetFuelRequirement(int input)
        {
            return Math.Max(0, (input/3)-2);
        }

        public static int GetFullFuelRequirement(int input)
        {
            int fuel = GetFuelRequirement(input);

            int additional = fuel;
            do {
                additional = GetFuelRequirement(additional);
                fuel += additional;
            } while (additional > 0);

            return fuel;
        }

        public static int FuelCheck01(string input)
        {
            var parts = Util.Split(input);

            int sum = 0;

            foreach (var part in parts)
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    sum += GetFuelRequirement(Int32.Parse(part.Trim()));
                }
            }

            return sum;
        }

        public static int FuelCheck02(string input)
        {
            var parts = Util.Split(input);

            int sum = 0;

            foreach (var part in parts)
            {
                if (!string.IsNullOrWhiteSpace(part))
                {
                    sum += GetFullFuelRequirement(Int32.Parse(part.Trim()));
                }
            }

            return sum;
        }

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - " + FuelCheck01(input));
            Console.WriteLine("- Pt2 - " + FuelCheck02(input));
        }
    }
}
