using AoC.Advent2016.BunniTek;
using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            public IEnumerable<State> PossibleMoves(State current, int maxMinutes)
            {
                current.mins++;

                bool built = false;

                if (current.mins < maxMinutes)
                {
                    if ((current.numOre >= GeodeCostOre) && (current.numObsidian >= GeodeCostObsidian))
                    {
                        yield return new State(current.mins, current.numOre - GeodeCostOre, current.numClay, current.numObsidian - GeodeCostObsidian, current.numGeode, current.numOreR, current.numClayR, current.numObsR, current.numGeoR + 1);
                        yield break;
                    }
                    else
                    {
                        //if (current.mins < (maxMinutes - 1))
                        {
                            if ((current.numOre >= ObsidianCostOre) && (current.numClay >= ObsidianCostClay))
                            {
                                yield return new State(current.mins, current.numOre - ObsidianCostOre, current.numClay - ObsidianCostClay, current.numObsidian, current.numGeode, current.numOreR, current.numClayR, current.numObsR + 1, current.numGeoR);
                                //built = true;
                            }
                        
                            if (current.numOre >= ClayCostOre)
                            {
                                yield return new State(current.mins, current.numOre - ClayCostOre, current.numClay, current.numObsidian, current.numGeode, current.numOreR, current.numClayR + 1, current.numObsR, current.numGeoR);
                                //built = true;
                            }

                            if (current.numOre >= OreCostOre)
                            {
                                yield return new State(current.mins, current.numOre - OreCostOre, current.numClay, current.numObsidian, current.numGeode, current.numOreR + 1, current.numClayR, current.numObsR, current.numGeoR);
                                //built = true;
                            }
                            
                        }
                    }
                }

                if (!built) yield return current;

            }
        }

        public record struct State(int mins, int numOre, int numClay, int numObsidian, int numGeode, int numOreR, int numClayR, int numObsR, int numGeoR)
        {
            public static implicit operator State((int mins, int numOre, int numClay, int numObsidian, int numGeode, int numOreR, int numClayR, int numObsR, int numGeoR) value)
            {
                return new State(value.mins, value.numOre, value.numClay, value.numObsidian, value.numGeode, value.numOreR, value.numClayR, value.numObsR, value.numGeoR);
            }

            public int TryBlueprint(Blueprint bp, Dictionary<State, int> seen, int maxMinutes)
            {
                if (mins == maxMinutes)
                    return numGeode;

                var dupe = (State)MemberwiseClone(); dupe.mins = 0;  

                if (seen.TryGetValue(dupe, out var prev) && prev <= mins) 
                    return numGeode;

                seen[dupe] = mins;

                int best = numGeode;
                var moves = bp.PossibleMoves(this, maxMinutes).OrderByDescending(m => (m.numGeoR, m.numObsR, m.numClayR)).ToArray();
                for (int i=0; i<moves.Length; i++)
                {
                    var m = moves[i];

                    m.numOre += numOreR;
                    m.numClay += numClayR;
                    m.numObsidian += numObsR;
                    m.numGeode += numGeoR;

                    var next = m.TryBlueprint(bp, seen, maxMinutes);
                    if (next > best)
                    {
                        best = next;
                    }
                }

                return best;
            }
        }

        public static (int score, int geodes) RunBlueprint(Blueprint bp, int minutes, ILogger logger)
        {
            var initial = new State(1, 0, 0, 0, 0, 1, 0, 0, 0);

            var seen = new Dictionary<State, int>();
            var bestGeodes = initial.TryBlueprint(bp, seen, minutes);

            int score = bestGeodes * bp.Id;
            lock (logger)
            {
                logger.WriteLine($"blueprint {bp.Id} results in {bestGeodes} geodes Opened : {score}");
            }
            return (score, bestGeodes);
        }
        public static int Part1(string input, ILogger logger)
        {
            var blueprints = Util.RegexParse<Blueprint>(input);
            return blueprints.AsParallel().Sum(bp => RunBlueprint(bp, 25, logger).score);
        }

        public static long Part2(string input, ILogger logger)
        {
            var blueprints = Util.RegexParse<Blueprint>(input).Take(3).ToArray();

            //Util.Test(RunBlueprint(blueprints[0], 32, logger).geodes, 56);
            //Util.Test(RunBlueprint(blueprints[1], 32, logger).geodes, 62);

            //return 0;
            return blueprints.AsParallel().Product(bp => RunBlueprint(bp, 32, logger).geodes);
        }

        public void Run(string input, ILogger logger)
        {
            string test = @"Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.
Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.";

            //Console.WriteLine(Part1(test));
            //Util.Test(RunBlueprint(blueprints[0]), 9);
            //Util.Test(RunBlueprint(blueprints[1]), 24);

            //Console.WriteLine(Part2(test, logger));

            //logger.WriteLine("- Pt1 - " + Part1(input, logger));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}

// 1653

/*
  
[07:59.933] blueprint 1 results in 21 geodes Opened : 21
[09:26.465] blueprint 2 results in 7 geodes Opened : 14
[11:58.767] blueprint 3 results in 14 geodes Opened : 42
[11:58.768] - Pt2 - 2058  
 
*/