using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day11 : IPuzzle
    {
        public string Name => "2016-11";

        public readonly struct State
        {
            public State(string input, QuestionPart part)
            {
                Dictionary<string, byte> nameLookup = new();
                Floors = Util.Split(input, '\n').Select(line => Tokenise(line, nameLookup)).ToArray();

                if (part.Two())
                {
                    byte nextTwoIds = (byte)(nameLookup.Values.Max() * 6);
                    Floors[0].chips += nextTwoIds;
                    Floors[0].generators += nextTwoIds;
                }
            }

            static (byte chips, byte generators) Tokenise(string line, Dictionary<string, byte> nameLookup) => line.Split(' ').Select(v => v.Length > 3 ? v[..3] : v).OverlappingPairs().Select(pair => (pair.second == "mic" ? nameLookup.GetIndexBit(pair.first) : 0, pair.second == "gen" ? nameLookup.GetIndexBit(pair.first) : 0)).Aggregate(((byte)0, (byte)0), (curr, next) => ((byte)(curr.Item1 + next.Item1), (byte)(curr.Item2 + next.Item2)));

            State(State previous, int newFloor, int moveChips, int moveGens)
            {
                Steps = previous.Steps + 1;
                CurrentFloor = newFloor;
                var oldFloor = previous.CurrentFloor;

                Floors = previous.Floors.ToArray();

                Floors[oldFloor].chips -= (byte)moveChips;
                Floors[newFloor].chips += (byte)moveChips;

                Floors[oldFloor].generators -= (byte)moveGens;
                Floors[newFloor].generators += (byte)moveGens;

                if (IsValid(Floors[oldFloor]) & IsValid(Floors[newFloor])) Remaining = CalcRemaining();
            }

            static readonly byte empty = 0;

            public IEnumerable<State> GetMoves()
            {
                var currentChips = Floors[CurrentFloor].chips.BitSequence();
                var currentGens = Floors[CurrentFloor].generators.BitSequence();

                if (CurrentFloor < 3)
                {
                    foreach (var chip1 in currentChips)
                    {
                        foreach (var chip2 in currentChips)
                            if (chip1 < chip2) yield return new State(this, CurrentFloor + 1, chip1 + chip2, empty);

                        yield return new State(this, CurrentFloor + 1, chip1, empty);

                        foreach (var gen in currentGens) yield return new State(this, CurrentFloor + 1, chip1, gen);
                    }
                }

                foreach (var gen1 in currentGens)
                {
                    if (CurrentFloor > 0) yield return new State(this, CurrentFloor - 1, empty, gen1);

                    if (CurrentFloor < 3)
                    {
                        foreach (var gen2 in currentGens)
                            if (gen1 < gen2) yield return new State(this, CurrentFloor + 1, empty, gen1 + gen2);

                        yield return new State(this, CurrentFloor + 1, empty, gen1);
                    }
                }
            }

            static bool IsValid((byte chips, byte generators) floor) => floor.chips == 0 || floor.generators == 0 || ((floor.generators & floor.chips) == floor.chips);
            int CalcRemaining() => Floors.Take(3).Sum(f => byte.PopCount(f.chips) + byte.PopCount(f.generators));
            public ulong Key() => (Floors.Take(3).Aggregate(0UL, (prev, curr) => ((prev << 14) + (ulong)(curr.chips << 7) + curr.generators)) << 4) + (ulong)CurrentFloor;

            readonly (byte chips, byte generators)[] Floors;
            readonly int CurrentFloor = 0;
            public readonly int Steps = 0;
            //public readonly bool Valid = true;
            public readonly int Remaining = int.MaxValue;
        }

        private static int FindBestPath(State initialState)
        {
            PriorityQueue<State, int> queue = new(new[] { (initialState, 0) });
            Dictionary<ulong, int> cache = new();

            int bestScore = int.MaxValue, closest = int.MaxValue;

            queue.Operate((state) =>
            {
                foreach (var move in state.GetMoves().Where(move => move.Steps < bestScore && move.Remaining - closest <=2))
                {
                    if (move.Remaining == 0 && bestScore > move.Steps) bestScore = move.Steps;
                    else 
                    {
                        if (cache.NotSeenHigher(move.Key(), move.Steps)) continue;

                        closest = Math.Min(move.Remaining, closest);
                        queue.Enqueue(move, move.Remaining);
                    }
                }
            });

            return bestScore;
        }

        public static int Part1(string input)
        {
            return FindBestPath(new State(input, QuestionPart.Part1));
        }

        public static int Part2(string input)
        {
            return FindBestPath(new State(input, QuestionPart.Part2));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}