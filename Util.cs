using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Advent
{
    public class Util
    {
        public static string[] Split(string input)
        {
            int commaCount = input.Count( c => c == ',');
            int linefeedCount = input.Count( c => c == '\n');
            if (linefeedCount > commaCount)
            {
                return input.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
            else
            {
                return input.Split(",").Select(e => e.Replace("\n","")).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }
        }

        public static int[] Parse(string[] input)
        {
            return input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Int32.Parse(s)).ToArray();
        }

        public static string GetInput(IPuzzle puzzle)
        {
            return System.IO.File.ReadAllText(System.IO.Path.Combine("Data",puzzle.Name+".txt"));
        }
    }
}
