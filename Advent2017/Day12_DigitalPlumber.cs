using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day12 : IPuzzle
    {
        public string Name => "2017-12";

        class Pools
        {
            public Pools(string input)
            {
                var lines = Util.Split(input, '\n');
                foreach (var line in lines.OrderByDescending(l => l.Length))
                {
                    AddGroup(Util.ExtractNumbers(line.Replace("-", "")).ToHashSet());
                }
            }

            List<HashSet<int>> pools = new();

            public void AddGroup(HashSet<int> group)
            {
                var joinedPools = pools.Where(p => p.Overlaps(group)).ToArray();
                group.UnionWith(joinedPools.SelectMany(i => i));
                pools = pools.Except(joinedPools).Append(group).ToList();
            }

            public HashSet<int> FindPool(int number) => pools.FirstOrDefault(p => p.Contains(number));

            public int NumGroups() => pools.Count;
        }

        public static int Part1(string input)
        {
            return new Pools(input).FindPool(0).Count;
        }

        public static int Part2(string input)
        {
            return new Pools(input).NumGroups();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}