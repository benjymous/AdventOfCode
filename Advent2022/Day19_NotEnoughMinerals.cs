using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day19 : IPuzzle
    {
        public string Name => "2022-19";

        public class Blueprint
        { 
            [Regex("Blueprint (.+): Each ore robot costs (.) ore. Each clay robot costs (.) ore. Each obsidian robot costs (.) ore and (.+) clay. Each geode robot costs (.) ore and (.+) obsidian.")]
            public Blueprint(int id, int oreCostOre, int clayCostOre, int obsidianCostOre, int obsidianCostClay, int geodeCostOre, int geodeCostObsidian) 
            {
                (Id, OreCostOre, ClayCostOre, ObsidianCostOre, ObsidianCostClay, GeodeCostOre, GeodeCostObsidian) = (id, oreCostOre, clayCostOre, obsidianCostOre, obsidianCostClay, geodeCostOre, geodeCostObsidian);
            }

            public readonly int Id, OreCostOre, ClayCostOre, ObsidianCostOre, ObsidianCostClay, GeodeCostOre, GeodeCostObsidian;

        }
        public static int Part1(string input)
        {
            var blueprints = Util.RegexParse<Blueprint>(input).ToArray();

            return 0;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            string test = @"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.";

            Console.WriteLine(Part1(test));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}