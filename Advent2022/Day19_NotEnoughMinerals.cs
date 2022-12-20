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
            public Blueprint(byte id, byte oreCostOre, byte clayCostOre, byte obsidianCostOre, byte obsidianCostClay, byte geodeCostOre, byte geodeCostObsidian) 
                => (Id, OreCostOre, ClayCostOre, ObsidianCostOre, ObsidianCostClay, GeodeCostOre, GeodeCostObsidian) = (id, oreCostOre, clayCostOre, obsidianCostOre, obsidianCostClay, geodeCostOre, geodeCostObsidian);

            public readonly byte Id, OreCostOre, ClayCostOre, ObsidianCostOre, ObsidianCostClay, GeodeCostOre, GeodeCostObsidian;

            public IEnumerable<State> PossibleMoves(State current, int maxMinutes)
            {
                current.mins++;

                if (current.mins < maxMinutes)
                {
                    if ((current.numOre >= GeodeCostOre) && (current.numObsidian >= GeodeCostObsidian))
                    {
                        yield return new State(current.mins, (byte)(current.numOre - GeodeCostOre), current.numClay, (byte)(current.numObsidian - GeodeCostObsidian), current.numGeode, current.numOreR, current.numClayR, current.numObsR, (byte)(current.numGeoR + 1));
                        yield break;
                    }
                    else
                    {
                        if (current.mins < (maxMinutes - 1))
                        {
                            if ((current.numOre >= ObsidianCostOre) && (current.numClay >= ObsidianCostClay))
                            {
                                yield return new State(current.mins, (byte)(current.numOre - ObsidianCostOre), (byte)(current.numClay - ObsidianCostClay), current.numObsidian, current.numGeode, current.numOreR, current.numClayR, (byte)(current.numObsR + 1), current.numGeoR);
                            }
                        
                            if (current.numOre >= ClayCostOre)
                            {
                                yield return new State(current.mins, (byte)(current.numOre - ClayCostOre), current.numClay, current.numObsidian, current.numGeode, current.numOreR, (byte)(current.numClayR + 1), current.numObsR, current.numGeoR);
                            }

                            if (current.numOre >= OreCostOre)
                            {
                                yield return new State(current.mins, (byte)(current.numOre - OreCostOre), current.numClay, current.numObsidian, current.numGeode, (byte)(current.numOreR + 1), current.numClayR, current.numObsR, current.numGeoR);
                            }                         
                        }
                    }
                }

                yield return current;

            }
        }

        public record struct State(byte mins, byte numOre, byte numClay, byte numObsidian, byte numGeode, byte numOreR, byte numClayR, byte numObsR, byte numGeoR)
        {
            public static implicit operator State((byte mins, byte numOre, byte numClay, byte numObsidian, byte numGeode, byte numOreR, byte numClayR, byte numObsR, byte numGeoR) value) 
                => new(value.mins, value.numOre, value.numClay, value.numObsidian, value.numGeode, value.numOreR, value.numClayR, value.numObsR, value.numGeoR);

            public int TryBlueprint(Blueprint bp, Dictionary<int, int> seen, int maxMinutes)
            {
                if (mins == maxMinutes)
                    return numGeode;

                var key = GetHashCode();

                if (seen.TryGetValue(key, out var prev) && prev <= mins)
                    return numGeode;

                seen[key] = mins;

                int best = numGeode;
                var moves = bp.PossibleMoves(this, maxMinutes).ToArray();
                for (int i=0; i<moves.Length; i++)
                {
                    var m = moves[i];

                    m.numOre += numOreR;
                    m.numClay += numClayR;
                    m.numObsidian += numObsR;
                    m.numGeode += numGeoR;

                    var next = m.TryBlueprint(bp, seen, maxMinutes);
                    best = Math.Max(best, next);
                }

                return best;
            }

            public override int GetHashCode() => HashCode.Combine(numOre, numClay, numObsidian, numGeode, numOreR, numClayR, numObsR, numGeoR);
        }

        public static (int score, int geodes) RunBlueprint(Blueprint bp, int minutes, ILogger logger)
        {
            var initial = new State(1, 0, 0, 0, 0, 1, 0, 0, 0);

            var seen = new Dictionary<int, int>();
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

            return blueprints.Product(bp => RunBlueprint(bp, 33, logger).geodes);
        }

        public void Run(string input, ILogger logger)
        {
            //logger.WriteLine("- Pt1 - " + Part1(input, logger));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}

/*
  
[07:59.933] blueprint 1 results in 21 geodes Opened : 21
[09:26.465] blueprint 2 results in 7 geodes Opened : 14
[11:58.767] blueprint 3 results in 14 geodes Opened : 42
[11:58.768] - Pt2 - 2058  
 
*/