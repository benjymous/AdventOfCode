using System;
using System.Linq;

namespace Advent.MMXIX
{
    public class Day01 : IPuzzle
    {
        public string Name { get { return "2019-01";} }

        public static int GetFuelRequirement(int moduleWeight) => Math.Max(0, (moduleWeight/3)-2);

        public static int GetFullFuelRequirement(int moduleWeight)
        {
            int fuel = GetFuelRequirement(moduleWeight);

            int additional = fuel;
            while (additional > 0)
            {
                fuel += additional = GetFuelRequirement(additional);
            } 

            return fuel;
        }

        public int FuelCheck01(string input) => Util.Parse(input).Select(module => GetFuelRequirement(module)).Sum();
        public int FuelCheck02(string input) => Util.Parse(input).Select(module => GetFullFuelRequirement(module)).Sum();

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - " + FuelCheck01(input));
            Console.WriteLine("- Pt2 - " + FuelCheck02(input));
        }
    }
}
