using System;
using System.Collections.Generic;
using System.Text;

namespace Advent
{
    public class Util
    {
        public static string[] Split(string input)
        {
            if (input.Contains("\n"))
            {
                return input.Split("\n");
            }
            else
            {
                return input.Split(",");
            }
        }
    }
}
