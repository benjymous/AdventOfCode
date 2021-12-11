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
                foreach (var line in lines)
                {
                    var bits = Util.ExtractNumbers(line.Replace("-", ""));
                    AddGroup(bits);
                }
            }
            List<HashSet<int>> pools = new List<HashSet<int>>();

            public void AddGroup(int[] group)
            {
                List<HashSet<int>> joinedPools = new List<HashSet<int>>();

                foreach (var pool in pools)
                {
                    foreach (var num in group)
                    {
                        if (pool.Contains(num))
                        {
                            joinedPools.Add(pool);
                            break;
                        }
                    }
                }

                if (joinedPools.Count == 1)
                {
                    // exising items don't span any pools
                    var pool = joinedPools.First();
                    foreach (var num in group)
                    {
                        pool.Add(num);
                    }
                }
                else
                {
                    // multiple pools spanned, or new pool
                    HashSet<int> newPool = new HashSet<int>(group);

                    foreach (var oldPool in joinedPools)
                    {
                        pools.Remove(oldPool);
                        newPool.UnionWith(oldPool);
                    }

                    pools.Add(newPool);
                }
            }

            public HashSet<int> FindPool(int number)
            {
                foreach (var pool in pools)
                {
                    if (pool.Contains(number)) return pool;
                }
                return null;
            }

            public int NumGroups() => pools.Count();
        }

        public static int Part1(string input)
        {
            var pools = new Pools(input);
            return pools.FindPool(0).Count();
        }

        public static int Part2(string input)
        {
            var pools = new Pools(input);
            return pools.NumGroups();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}