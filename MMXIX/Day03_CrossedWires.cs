using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.Utils.Vectors;

namespace Advent.MMXIX
{
    public class Day03 : IPuzzle
    {
        public string Name { get { return "2019-03";} }

        public enum SearchMode 
        {
            Closest,
            Shortest
        }

        public static int FindIntersection(string input, SearchMode mode)
        {
            var lines = input.Split("\n");

            var minDist = int.MaxValue;

            Dictionary<string, int> seen = new Dictionary<string, int>();
            
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var instructions = line.Split(",");

                var position = new ManhattanVector2(0,0);
                int steps = 0;
                Dictionary<string, int> current = new  Dictionary<string, int>();

                foreach (var i in instructions)
                {
                    if (string.IsNullOrEmpty(i)) continue;
                    var distance = int.Parse(i.Substring(1));

                    for (var j=0; j<distance; ++j)
                    {
                        switch (i[0])
                        {
                            case 'R':                        
                            position.X ++;
                            break;

                            case 'L':
                            position.X --;
                            break;

                            case 'U':
                            position.Y --;
                            break;
                            
                            case 'D':
                            position.Y ++;
                            break;
                        }

                        steps++;

                        var searchKey = position.ToString();
                        if (seen.ContainsKey(searchKey))
                        {
                            int dist = int.MaxValue;
                            if (mode == SearchMode.Closest)
                            {
                                dist = position.Distance(ManhattanVector2.Zero);
                            }
                            else
                            {
                                dist = steps + seen[searchKey];
                                //Console.WriteLine($"Intersection at {position} Distances {steps}, {seen[searchKey]} = {dist}");                        
                            }
                            minDist = Math.Min(minDist, dist);
                        }
                        else
                        {
                            current[searchKey] = steps; 
                        }
                    }                    
                } 

                foreach (var s in current) seen[s.Key] = s.Value;                         
            }

            return minDist;
        }
 
        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+FindIntersection(input, SearchMode.Closest));  
            logger.WriteLine("- Pt2 - "+FindIntersection(input, SearchMode.Shortest));  
        }
    }
}
