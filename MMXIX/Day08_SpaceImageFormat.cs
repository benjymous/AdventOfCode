using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day08 : IPuzzle
    {
        public string Name { get { return "2019-08";} }
 
        public static int Part1(string input)
        {
            var image = new NPSA.Image(input, 25, 6);
            return image.GetChecksum();
        }

        public static string Part2(string input, System.IO.TextWriter console)
        {
            var image = new NPSA.Image(input, 25, 6);
            console.WriteLine(image.ToString());
            return image.ToString().GetSHA256String();
        }
        
        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input, console));
        }
    }
}