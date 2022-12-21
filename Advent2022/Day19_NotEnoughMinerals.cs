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
            public Blueprint(byte id, byte oreCostOre, byte clayCostOre, byte obsidianCostOre, byte obsidianCostClay, byte geodeCostOre, byte geodeCostObsidian) 
                => (Id, OreCostOre, ClayCostOre, ObsidianCostOre, ObsidianCostClay, GeodeCostOre, GeodeCostObsidian) = (id, oreCostOre, clayCostOre, obsidianCostOre, obsidianCostClay, geodeCostOre, geodeCostObsidian);

            public readonly byte Id, OreCostOre, ClayCostOre, ObsidianCostOre, ObsidianCostClay, GeodeCostOre, GeodeCostObsidian;

            public IEnumerable<State> PossibleMoves(State current)
            {
                if (current.mins-- > 0)
                {
                    if ((current.numOre >= GeodeCostOre) && (current.numObsidian >= GeodeCostObsidian))
                    {
                        yield return new State(current.mins, (byte)(current.numOre - GeodeCostOre), current.numClay, (byte)(current.numObsidian - GeodeCostObsidian), current.numGeode, current.numOreR, current.numClayR, current.numObsR, (byte)(current.numGeoR + 1));
                        yield break;
                    }
                    else if (current.mins > 1)
                    {
                        if ((current.numOre >= ObsidianCostOre) && (current.numClay >= ObsidianCostClay))
                            yield return new State(current.mins, (byte)(current.numOre - ObsidianCostOre), (byte)(current.numClay - ObsidianCostClay), current.numObsidian, current.numGeode, current.numOreR, current.numClayR, (byte)(current.numObsR + 1), current.numGeoR);

                        if (current.mins > 2)
                        {
                            if (current.numOre >= ClayCostOre)
                                yield return new State(current.mins, (byte)(current.numOre - ClayCostOre), current.numClay, current.numObsidian, current.numGeode, current.numOreR, (byte)(current.numClayR + 1), current.numObsR, current.numGeoR);

                            if (current.numOre >= OreCostOre)
                                yield return new State(current.mins, (byte)(current.numOre - OreCostOre), current.numClay, current.numObsidian, current.numGeode, (byte)(current.numOreR + 1), current.numClayR, current.numObsR, current.numGeoR);
                        }
                    }
                }
                yield return current;
            }
        }

        public record struct State(sbyte mins, byte numOre, byte numClay, byte numObsidian, byte numGeode, byte numOreR, byte numClayR, byte numObsR, byte numGeoR)
        {
            public IEnumerable<State> PossibleMoves(Blueprint bp)
            {
                var self = this;
                return mins == 0 ? Enumerable.Empty<State>() : bp.PossibleMoves(this).Select(m => m.Tick(self));
            }

            State Tick(State parent)
            {
                numOre += parent.numOreR;
                numClay += parent.numClayR;
                numObsidian += parent.numObsR;
                numGeode += parent.numGeoR;
                return this;
            }

            public int GetResourceScore(Blueprint bp) => numOre + (numOreR * mins) +
                       (numClay + (numClayR * mins)) * bp.ClayCostOre +
                       (numObsidian + (numObsR * mins)) * bp.ObsidianCostClay * bp.ClayCostOre +
                       (numGeode + (numGeoR * mins)) * bp.GeodeCostObsidian * bp.ObsidianCostClay * bp.ClayCostOre;
        }

        public static (int score, int geodes) RunBlueprint(Blueprint bp, sbyte initialMinutes)
        {
            int best = 0;
            State[] generation = { new State(initialMinutes, 0, 0, 0, 0, 1, 0, 0, 0) };
            while (generation.Any())
            {
                generation = generation.SelectMany(s => s.PossibleMoves(bp)).Distinct().OrderByDescending(state => state.GetResourceScore(bp)).Take(64).ToArray();
                if (generation.Any()) best = Math.Max(best, generation.Max(state => state.numGeode));
            }        
            return (best * bp.Id, best);
        }

        public static int Part1(string input)
        {
            return Util.RegexParse<Blueprint>(input).AsParallel().Sum(bp => RunBlueprint(bp, 24).score);
        }

        public static long Part2(string input)
        {
            return Util.RegexParse<Blueprint>(input).Take(3).AsParallel().Product(bp => RunBlueprint(bp, 32).geodes);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}