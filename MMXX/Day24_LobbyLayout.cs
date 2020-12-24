using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Utils;
using System.Text;

namespace Advent.MMXX
{
    public class Day24 : IPuzzle
    {
        public string Name { get { return "2020-24";} }

        static (int x, int y, int z) TranslateHex((int x, int y, int z)pos, string dir)
        {
            switch(dir)
            {
                case "ne": return (pos.x+1, pos.y-1, pos.z);
                case "e":  return (pos.x+1, pos.y, pos.z-1);
                case "se": return (pos.x, pos.y+1, pos.z-1);
                case "sw": return (pos.x-1, pos.y+1, pos.z);
                case "w":  return (pos.x-1, pos.y, pos.z+1);
                case "nw": return (pos.x, pos.y-1, pos.z+1);
            }
            throw new Exception("unexpected direction" +dir);
        }
 
        static string SplitCommands(string line)
        {   
            line=line.Replace("se", " se ");
            line=line.Replace("ne", " ne ");
            line=line.Replace("sw", " sw ");
            line=line.Replace("nw", " nw ");
            while(line.Contains("ww")) line=line.Replace("ww", "w w");
            while(line.Contains("ee")) line=line.Replace("ee", "e e");
            while(line.Contains("ew")) line=line.Replace("ew", "e w");
            while(line.Contains("we")) line=line.Replace("we", "w e");
            while(line.Contains("  ")) line=line.Replace("  ", " ");
            return line.Trim();
        }

        static (int x, int y, int z) FollowPath(string path)
        {
            (int x, int y, int z) pos = (0,0,0);

            var steps = path.Split(" ");
            foreach (var step in steps)
            {
                pos = TranslateHex(pos, step);
            }
            return pos;
        }

        public static int Part1(string input)
        {
            var data = input.Trim().Split("\n").Select(l => SplitCommands(l));

            var counts = new Dictionary<(int x, int y, int z) , int>();

            foreach (var line in data)
            {
                counts.IncrementAtIndex(FollowPath(line));
            }
            return counts.Where(kvp => (kvp.Value%2==1)).Count();
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}