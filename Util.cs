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
            if (input.Contains("\n") && input.Count( c => c == '\n') > 1 && !input.Trim().EndsWith("\n"))
            {
                return input.Split("\n");
            }
            else
            {
                return input.Split(",").Select(e => e.Replace("\n","")).ToArray();
            }
        }

        public static int[] Parse(string[] input)
        {
            return input.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => Int32.Parse(s)).ToArray();
        }
    }
}
