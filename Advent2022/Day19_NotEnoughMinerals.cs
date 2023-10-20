using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day19 : IPuzzle
    {
        public string Name => "2022-19";

        [Regex("Blueprint (.+): Each ore robot costs (.) ore. Each clay robot costs (.) ore. Each obsidian robot costs (.) ore and (.+) clay. Each geode robot costs (.) ore and (.+) obsidian.")]
        public record struct Blueprint(int Id, int OreCostOre, int ClayCostOre, int ObsidianCostOre, int ObsidianCostClay, int GeodeCostOre, int GeodeCostObsidian)
        {
            public readonly IEnumerable<State> PossibleMoves(State current)
            {
                if (current.Mins-- > 0)
                {
                    if ((current.NumOre >= GeodeCostOre) && (current.NumObsidian >= GeodeCostObsidian))
                    {
                        yield return new State(current.Mins, (current.NumOre - GeodeCostOre), current.NumClay, (current.NumObsidian - GeodeCostObsidian), current.NumGeode, current.NumOreRobots, current.NumClayRobots, current.NumObsidianRobots, (current.NumGeodeRobots + 1));
                        yield break;
                    }
                    else if (current.Mins > 1)
                    {
                        if ((current.NumOre >= ObsidianCostOre) && (current.NumClay >= ObsidianCostClay))
                            yield return new State(current.Mins, (current.NumOre - ObsidianCostOre), (current.NumClay - ObsidianCostClay), current.NumObsidian, current.NumGeode, current.NumOreRobots, current.NumClayRobots, (current.NumObsidianRobots + 1), current.NumGeodeRobots);

                        if (current.Mins > 2)
                        {
                            if (current.NumOre >= ClayCostOre)
                                yield return new State(current.Mins, (current.NumOre - ClayCostOre), current.NumClay, current.NumObsidian, current.NumGeode, current.NumOreRobots, (current.NumClayRobots + 1), current.NumObsidianRobots, current.NumGeodeRobots);

                            if (current.NumOre >= OreCostOre)
                                yield return new State(current.Mins, (current.NumOre - OreCostOre), current.NumClay, current.NumObsidian, current.NumGeode, (current.NumOreRobots + 1), current.NumClayRobots, current.NumObsidianRobots, current.NumGeodeRobots);
                        }
                    }
                }
                yield return current;
            }

            public readonly (int score, int geodes) Run(int initialMinutes)
            {
                int best = 0;
                var self = this;
                State[] generation = { new State(initialMinutes) };
                while (generation.Length != 0)
                {
                    best = generation.Max(state => state.NumGeode);
                    generation = generation.SelectMany(s => s.PossibleMoves(self)).Distinct().OrderByDescending(state => state.GetResourceScore(self)).Take(64).ToArray();
                }
                return (best * Id, best);
            }
        }

        public record struct State(int Mins, int NumOre = 0, int NumClay = 0, int NumObsidian = 0, int NumGeode = 0, int NumOreRobots = 1, int NumClayRobots = 0, int NumObsidianRobots = 0, int NumGeodeRobots = 0)
        {
            public readonly IEnumerable<State> PossibleMoves(Blueprint bp)
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

            public readonly int GetResourceScore(Blueprint bp) => NumOre + (NumOreRobots * Mins) +
                       (NumClay + (NumClayRobots * Mins)) * bp.ClayCostOre +
                       (NumObsidian + (NumObsidianRobots * Mins)) * bp.ObsidianCostClay * bp.ClayCostOre +
                       (NumGeode + (NumGeodeRobots * Mins)) * bp.GeodeCostObsidian * bp.ObsidianCostClay * bp.ClayCostOre;
        }

        public static int Part1(string input)
        {
            return Util.RegexParse<Blueprint>(input).AsParallel().Sum(bp => bp.Run(24).score);
        }

        public static long Part2(string input)
        {
            return Util.RegexParse<Blueprint>(input).Take(3).AsParallel().Product(bp => bp.Run(32).geodes);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}