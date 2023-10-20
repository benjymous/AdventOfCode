using AoC.Utils;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day23 : IPuzzle
    {
        public string Name => "2021-23";

        static readonly FrozenDictionary<sbyte, char> destinations = (new Dictionary<sbyte, char>() { { Convert(3, 2), 'A' }, { Convert(5, 2), 'B' }, { Convert(7, 2), 'C' }, { Convert(9, 2), 'D' }, { Convert(3, 3), 'A' }, { Convert(5, 3), 'B' }, { Convert(7, 3), 'C' }, { Convert(9, 3), 'D' }, { Convert(3, 4), 'A' }, { Convert(5, 4), 'B' }, { Convert(7, 4), 'C' }, { Convert(9, 4), 'D' }, { Convert(3, 5), 'A' }, { Convert(5, 5), 'B' }, { Convert(7, 5), 'C' }, { Convert(9, 5), 'D' } }).ToFrozenDictionary();
        static readonly int[] destinationCol = new int[] { 3, 5, 7, 9 };
        static readonly int[] moveCosts = new int[] { 1, 10, 100, 1000 };
        private static readonly string[] elements = new string[] { "  #D#C#B#A#", "  #D#B#A#C#" };

        static bool IsClear(int x1, int x2, State state) => !Util.RangeInclusive(Math.Min(x1, x2), Math.Max(x1, x2)).Any(x => !state.CellOpen(Convert(x, 1)));
        static sbyte Convert(int x, int y) => (sbyte)(x - 1 + (y - 1) * 11);
        static (int x, int y) Convert(sbyte pos) => (pos % 11 + 1, pos / 11 + 1);

        static void CloseCompleted(State state)
        {
            var completed = destinations.Where(kvp => state.CellOccupied(kvp.Key) && state.Critters[kvp.Key] == kvp.Value && !state.CellOccupied(kvp.Key + 11) && !state.CellOpen(kvp.Key + 11)).Select(kvp => kvp.Key);
            while (completed.Any()) completed.ForEach(key => state.Critters.Remove(key));
        }

        struct State
        {
            public State(ulong openCells, Dictionary<sbyte, char> critters, int score)
            {
                (OpenCells, Critters, Score) = (openCells, critters, score);
                Key = (Critters.Keys.Select(k => 1UL << k).Sum(), Critters.OrderBy(kvp => kvp.Key).Select(kvp => (uint)(kvp.Value - 'A')).Aggregate(0U, (p, v) => (p << 2) + v));
                EstimatedScore = Score + Critters.Select(c => (Pos: Convert(c.Key), c.Value)).Sum(v => v.Pos.x != destinationCol[v.Value - 'A'] ? (v.Pos.y - 1 + Math.Abs(v.Pos.x - destinationCol[v.Value - 'A'])) * moveCosts[v.Value - 'A'] : 0);
            }

            public readonly bool CellOpen(int bit) => ((OpenCells >> bit) & 1) == 1;
            public readonly bool CellOccupied(int cell) => Critters.ContainsKey((sbyte)cell);

            public (ulong, uint) Key;
            public ulong OpenCells;
            public Dictionary<sbyte, char> Critters;
            public int Score, EstimatedScore;
        }

        static IEnumerable<(KeyValuePair<sbyte, char> critter, sbyte destination, int spaces, bool home)> CritterMoves(State state, KeyValuePair<sbyte, char> critter)
        {
            var critterPos = Convert(critter.Key);
            if (critterPos.y == 1 || state.CellOpen(critter.Key - 11)) // can this critter reach its home room?
            {
                int destX = destinationCol[critter.Value - 'A'];
                if (IsClear(critterPos.x + Math.Sign(destX - critterPos.x), destX, state))
                {
                    for (var destY = 5; destY >= 2; --destY)
                    {
                        var dest = Convert(destX, destY);
                        if (state.CellOpen(dest) && !state.CellOpen(dest + 11) && !state.CellOccupied(dest + 11))
                        {
                            yield return (critter, dest, Math.Abs(critterPos.x - destX) + (destY - 1) + ((critterPos.y != 1) ? critterPos.y - 1 : 0), true);
                            yield break;
                        }
                    }
                }
            }

            if (critterPos.y >= 2 && state.CellOpen(critter.Key - 11)) // critter in room, and cell above is empty
            {
                for (int dir = -1; dir <= 1; dir += 2)
                {
                    for (int i = 1; i < 9; ++i)
                    {
                        int x = critterPos.x + (i * dir);
                        if (!destinationCol.Contains(x))
                        {
                            var move = Convert(x, 1);
                            if (state.CellOpen(move)) yield return (critter, move, critterPos.y - 1 + i, false);
                            else break;
                        }
                    }
                }
            }
        }

        public static int ShrimpStacker(IEnumerable<string> input)
        {
            var map = Util.ParseSparseMatrix<char>(input);

            PriorityQueue<State, int> queue = new();
            Dictionary<(ulong, uint), int> cache = new();

            queue.Enqueue(new State(map.Where(kvp => kvp.Value == '.').Select(kvp => kvp.Key).Select(p => 1UL << Convert(p.x, p.y)).Sum(), map.Where(kvp => kvp.Value >= 'A' && kvp.Value <= 'D').ToDictionary(kvp => Convert(kvp.Key.x, kvp.Key.y), kvp => kvp.Value), 0), 0);
            int bestScore = int.MaxValue;

            queue.Operate((state) =>
            {
                foreach (var move in state.Critters.SelectMany(critter => CritterMoves(state, critter)))
                {
                    var newState = new State(state.OpenCells - (1UL << move.destination) + (1UL << move.critter.Key), new Dictionary<sbyte, char>(state.Critters).Minus(move.critter.Key).Plus(move.destination, move.critter.Value), state.Score + move.spaces * moveCosts[move.critter.Value - 'A']);
                    if ((cache.TryGetValue(newState.Key, out var lastScore) && lastScore <= newState.Score) || (newState.Score >= bestScore) || (newState.EstimatedScore >= bestScore)) continue; else cache[newState.Key] = newState.Score;

                    CloseCompleted(newState);

                    if (newState.Critters.Count != 0) queue.Enqueue(newState, newState.EstimatedScore);
                    else bestScore = Math.Min(bestScore, newState.Score);
                }
            });

            return bestScore;
        }

        public static int Part1(string input)
        {
            return ShrimpStacker(input.Split('\n'));
        }

        public static int Part2(string input)
        {
            return ShrimpStacker(input.Split('\n').InsertRangeAt(elements, 3));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}