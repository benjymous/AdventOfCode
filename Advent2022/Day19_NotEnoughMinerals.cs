using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day19 : IPuzzle
    {
        public string Name => "2022-19";

        [Regex("Blueprint (.+): Each ore robot costs (.) ore. Each clay robot costs (.) ore. Each obsidian robot costs (.) ore and (.+) clay. Each geode robot costs (.) ore and (.+) obsidian.")]
        public record struct Blueprint(byte Id, byte OreCostOre, byte ClayCostOre, byte ObsidianCostOre, byte ObsidianCostClay, byte GeodeCostOre, byte GeodeCostObsidian)
        {
            public IEnumerable<State> PossibleMoves(State current)
            {
                if (current.Mins-- > 0)
                {
                    if ((current.NumOre >= GeodeCostOre) && (current.NumObsidian >= GeodeCostObsidian))
                    {
                        yield return new State(current.Mins, (byte)(current.NumOre - GeodeCostOre), current.NumClay, (byte)(current.NumObsidian - GeodeCostObsidian), current.NumGeode, current.NumOreRobots, current.NumClayRobots, current.NumObsidianRobots, (byte)(current.NumGeodeRobots + 1));
                        yield break;
                    }
                    else if (current.Mins > 1)
                    {
                        if ((current.NumOre >= ObsidianCostOre) && (current.NumClay >= ObsidianCostClay))
                            yield return new State(current.Mins, (byte)(current.NumOre - ObsidianCostOre), (byte)(current.NumClay - ObsidianCostClay), current.NumObsidian, current.NumGeode, current.NumOreRobots, current.NumClayRobots, (byte)(current.NumObsidianRobots + 1), current.NumGeodeRobots);

                        if (current.Mins > 2)
                        {
                            if (current.NumOre >= ClayCostOre)
                                yield return new State(current.Mins, (byte)(current.NumOre - ClayCostOre), current.NumClay, current.NumObsidian, current.NumGeode, current.NumOreRobots, (byte)(current.NumClayRobots + 1), current.NumObsidianRobots, current.NumGeodeRobots);

                            if (current.NumOre >= OreCostOre)
                                yield return new State(current.Mins, (byte)(current.NumOre - OreCostOre), current.NumClay, current.NumObsidian, current.NumGeode, (byte)(current.NumOreRobots + 1), current.NumClayRobots, current.NumObsidianRobots, current.NumGeodeRobots);
                        }
                    }
                }
                yield return current;
            }
        }

        public record struct State(sbyte Mins, byte NumOre, byte NumClay, byte NumObsidian, byte NumGeode, byte NumOreRobots, byte NumClayRobots, byte NumObsidianRobots, byte NumGeodeRobots)
        {
            public IEnumerable<State> PossibleMoves(Blueprint bp)
            {
                var self = this;
                return Mins == 0 ? Enumerable.Empty<State>() : bp.PossibleMoves(this).Select(m => m.Tick(self));
            }

            State Tick(State parent)
            {
                NumOre += parent.NumOreRobots;
                NumClay += parent.NumClayRobots;
                NumObsidian += parent.NumObsidianRobots;
                NumGeode += parent.NumGeodeRobots;
                return this;
            }

            public int GetResourceScore(Blueprint bp) => NumOre + (NumOreRobots * Mins) +
                       (NumClay + (NumClayRobots * Mins)) * bp.ClayCostOre +
                       (NumObsidian + (NumObsidianRobots * Mins)) * bp.ObsidianCostClay * bp.ClayCostOre +
                       (NumGeode + (NumGeodeRobots * Mins)) * bp.GeodeCostObsidian * bp.ObsidianCostClay * bp.ClayCostOre;
        }

        public static (int score, int geodes) RunBlueprint(Blueprint bp, sbyte initialMinutes)
        {
            int best = 0;
            State[] generation = { new State(initialMinutes, 0, 0, 0, 0, 1, 0, 0, 0) };
            while (generation.Any())
            {
                generation = generation.SelectMany(s => s.PossibleMoves(bp)).Distinct().OrderByDescending(state => state.GetResourceScore(bp)).Take(64).ToArray();
                if (generation.Any()) best = Math.Max(best, generation.Max(state => state.NumGeode));
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