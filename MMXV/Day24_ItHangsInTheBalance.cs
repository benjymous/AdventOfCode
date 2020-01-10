using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXV
{
    public class Day24 : IPuzzle
    {
        public string Name { get { return "2015-24";} }

        public static IEnumerable<IEnumerable<int>> Groupings(IEnumerable<int> available, int target)
        {
            foreach (var first in available)
            {
                if (first == target)
                {
                    yield return new List<int>{first};
                }
                else if (first < target)
                {
                    int shortest = int.MaxValue;
                    Int64 QE = Int64.MaxValue;
                    HashSet<string> seen = new HashSet<string>();
                    foreach (var child in Groupings(available.Where(x => x != first), target-first))
                    {
                        if (child.Count() <= shortest)
                        {
                            var key = string.Join(",", child.OrderByDescending(x=>x));
                            if (seen.Contains(key)) continue;

                            seen.Add(key);
                            
                            var qe = child.Product();
                            if (qe < QE)
                            {
                                QE = qe;
                                shortest = child.Count();
                                var l = new List<int>(){first};
                                l.AddRange(child);

                                yield return l;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        public static bool CanCreateChildGroups(IEnumerable<int> remaining, int target)
        {
            var g = Groupings(remaining, target);
            return g.Any();
        }
 
        public static Int64 Solve(string input, int numGroups)
        {
            var parcels = Util.Parse32(input).OrderByDescending(x => x);

            int totalSize = parcels.Sum();
            int groupSize = totalSize/numGroups;

            var groups = Groupings(parcels, groupSize);

            int smallestGroup = int.MaxValue;
            Int64 QE = Int64.MaxValue;

            HashSet<string> seen = new HashSet<string>();

            foreach (var g in groups)//.Reverse())// .OrderBy(g => g.Count()))
            {
                if (smallestGroup < int.MaxValue && g.Count() > smallestGroup) break;

                var key = string.Join(",", g.OrderByDescending(x=>x));
                if (seen.Contains(key)) continue;

                seen.Add(key);

                var qe = g.Product();
                if (qe < QE)
                {
                    if (CanCreateChildGroups(parcels.Where(x => !g.Contains(x)), groupSize))
                    {
                        QE = qe;
                        //Console.WriteLine($"{g.Count()} {qe} {string.Join(", ", g)}");
                        smallestGroup = g.Count();   
                    }                
                }
                
            }

            return QE;
        }

        public static Int64 Part1(string input)
        {
            return Solve(input, 3);
        }

        public static Int64 Part2(string input)
        {
            return Solve(input, 4);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}