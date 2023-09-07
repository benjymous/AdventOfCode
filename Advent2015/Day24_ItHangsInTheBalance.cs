using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day24 : IPuzzle
    {
        public string Name => "2015-24";

        public static IEnumerable<int[]> Groupings(int[] available, int target)
        {
            foreach (var first in available)
            {
                if (first == target)
                {
                    yield return new[] { first };
                    if (available.Length == 1) yield break;
                }
                else 
                {
                    int restTarget = target - first;
                    var remaining = available.Where(x => x < first && x <= restTarget).ToArray();

                    int shortest = int.MaxValue;
                    long QE = long.MaxValue;
                    
                    foreach (var child in Groupings(remaining, restTarget))
                    {
                        if (child.Length <= shortest)
                        {
                            var qe = child.Product();
                            if (qe < QE)
                            {
                                QE = qe;
                                shortest = child.Length;

                                yield return child.Append(first).ToArray();
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

        public static bool CanCreateChildGroups(int[] remaining, int target) => Groupings(remaining, target).Any();

        public static long Solve(string input, int numGroups)
        {
            var parcels = Util.ParseNumbers<int>(input).OrderDescending().ToArray();

            int groupSize = parcels.Sum() / numGroups;

            var groups = Groupings(parcels, groupSize);

            int smallestGroup = int.MaxValue;
            long QE = long.MaxValue;

            foreach (var g in groups)
            {
                if (g.Length > smallestGroup) break;

                var qe = g.Product();
                if (qe < QE && CanCreateChildGroups(parcels.Except(g).ToArray(), groupSize))
                {
                    QE = qe;
                    smallestGroup = g.Length;
                }
            }

            return QE;
        }

        public static long Part1(string input)
        {
            return Solve(input, 3);
        }

        public static long Part2(string input)
        {
            return Solve(input, 4);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}