using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day23 : IPuzzle
    {
        public string Name => "2021-23";

        static readonly Dictionary<byte, char> destinations = new() { { Convert(3, 2), 'A' },  { Convert(5, 2), 'B' }, { Convert(7, 2), 'C' },  { Convert(9, 2), 'D' },  { Convert(3, 3), 'A' },  { Convert(5, 3), 'B' }, { Convert(7, 3), 'C' }, { Convert(9, 3), 'D' }, { Convert(3, 4), 'A' }, { Convert(5, 4), 'B' }, { Convert(7, 4), 'C' }, { Convert(9, 4), 'D' }, { Convert(3, 5), 'A' }, { Convert(5, 5), 'B' }, { Convert(7, 5), 'C' }, { Convert(9, 5), 'D' } };
        static readonly int[] destinationCol = new int[] { 3, 5, 7, 9 };
        static readonly HashSet<int> doorCells = new() { 3, 5, 7, 9 };
        static readonly int[] moveCosts = new int[] { 1, 10, 100, 1000 };

        static bool IsClear(int x1, int x2, ulong openCells) => !Util.RangeInclusive(Math.Min(x1, x2), Math.Max(x1, x2)).Where(x => !Contains(openCells, Convert(x, 1))).Any();
        static (ulong, uint) Key(Dictionary<byte, char> critters) => (critters.Keys.Select(k => 1UL<<k).Sum(), critters.OrderBy(kvp => kvp.Key).Select(kvp => (uint)(kvp.Value-'A')).Aggregate(0U, (p, v) => (p << 2) + v));
        static byte Convert(int x, int y) => (byte)(x - 1 + (y - 1) * 11);
        static (int x, int y) Convert(byte pos) => (pos % 11 + 1, pos / 11 + 1);
        static ulong HashNum(IEnumerable<(int x, int y)> positions) => positions.Select(p => 1UL << Convert(p.x, p.y)).Sum();
        static bool Contains(ulong set, byte bit) => ((set >> bit) & 1) == 1;

        public static int ShrimpStacker(IEnumerable<string> input)
        {
            var map = Util.ParseSparseMatrix<char>(input);

            PriorityQueue<(ulong openCells, Dictionary<byte, char> critters, int score), int> queue = new();
            Dictionary<(ulong, uint), int> cache = new();

            queue.Enqueue((HashNum(map.Where(kvp => kvp.Value == '.').Select(kvp => kvp.Key)), map.Where(kvp => kvp.Value >= 'A' && kvp.Value <= 'D').ToDictionary(kvp => Convert(kvp.Key.x, kvp.Key.y), kvp => kvp.Value), 0), 0);
            int bestScore = int.MaxValue;

            while (queue.TryDequeue(out var state, out var _))
            { 
                CloseCompleted(state.critters, state.openCells);

                if (!state.critters.Any())
                {
                    bestScore = Math.Min(bestScore, state.score);
                    continue;
                }

                List<(KeyValuePair<byte, char> critter, byte destination, int spaces, bool home)> moves = new();

                foreach (var critter in state.critters)
                {
                    bool escaped = false;
                    var critterPos = Convert(critter.Key);
                    if (critterPos.y == 1 || Contains(state.openCells, (byte)(critter.Key - 11)))
                    {
                        // see if this one can go home
                        int destX = destinationCol[critter.Value-'A'];
                        if (IsClear(critterPos.x + Math.Sign(destX - critterPos.x), destX, state.openCells))
                        {
                            for (var destY = 5; destY >= 2; --destY)
                            {
                                if (Contains(state.openCells, Convert(destX, destY)) && !Contains(state.openCells, Convert(destX, destY + 1)) && !state.critters.ContainsKey(Convert(destX, destY + 1)))
                                {
                                    moves.Add((critter, Convert(destX, destY), Math.Abs(critterPos.x - destX) + (destY - 1) + ((critterPos.y != 1) ? critterPos.y - 1 : 0), true));
                                    escaped = true;
                                    break;
                                }
                            }
                        }     
                    }
                    if (escaped) continue;

                    // move out of starting room into corridor
                    if (critterPos.y >= 2 && Contains(state.openCells, (byte)(critter.Key-11)))
                    {
                        for (int dir = -1; dir <= 1; dir += 2)
                        {
                            for (int i = 1; i < 9; ++i)
                            {
                                int x = critterPos.x + (i * dir);
                                if (!doorCells.Contains(x))
                                {
                                    var move = Convert(x, 1);
                                    if (Contains(state.openCells, move)) moves.Add((critter, move, critterPos.y - 1 + i, false));                                  
                                    else break;                                
                                }
                            }
                        }                             
                    }
                }

                foreach (var move in moves)
                {
                    int newScore = state.score + move.spaces * moveCosts[move.critter.Value-'A'];
                    if (newScore > bestScore) continue;

                    var newCritters = new Dictionary<byte, char>(state.critters);
                    newCritters.Remove(move.critter.Key);
                    newCritters[move.destination] = move.critter.Value;

                    var key = Key(newCritters);
                    if (cache.TryGetValue(key, out var lastScore) && lastScore <= newScore ) continue;
                    cache[key] = newScore;

                    queue.Enqueue((state.openCells - (1UL << move.destination) + (1UL << move.critter.Key), newCritters, newScore), newScore * (newCritters.Count - (move.home ? 1 : 0)));
                }
            }

            return bestScore;
        }

        static void CloseCompleted(Dictionary<byte, char> critters, ulong opencells)
        {
            var completed = destinations.Where(kvp => critters.ContainsKey(kvp.Key) && critters[kvp.Key] == kvp.Value && !critters.ContainsKey((byte)(kvp.Key + 11)) && !Contains(opencells, (byte)(kvp.Key+11))).Select(kvp => kvp.Key);
            while (completed.Any()) completed.ForEach(key => critters.Remove(key));
        }

        public static int Part1(string input)
        {
            return ShrimpStacker(input.Split('\n'));
        }

        public static int Part2(string input)
        {
            var lines = input.Split('\n');
            return ShrimpStacker(lines.Take(3).Union(new string[] { "  #D#C#B#A#", "  #D#B#A#C#" }).Union(lines.Skip(3)));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}