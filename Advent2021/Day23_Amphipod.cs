using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day23 : IPuzzle
    {
        public string Name => "2021-23";

        static readonly Dictionary<(int x, int y), char> destinations = new() { { (3, 2), 'A' },  { (5, 2), 'B' }, { (7, 2), 'C' },  { (9, 2), 'D' },  { (3, 3), 'A' },  { (5, 3), 'B' }, { (7, 3), 'C' }, { (9, 3), 'D' }, { (3, 4), 'A' }, { (5, 4), 'B' }, { (7, 4), 'C' }, { (9, 4), 'D' }, { (3, 5), 'A' }, { (5, 5), 'B' }, { (7, 5), 'C' }, { (9, 5), 'D' } };
        static readonly Dictionary<char, int> destinationCol = new() { { 'A', 3 }, { 'B', 5 }, { 'C', 7 }, { 'D', 9 } };
        static readonly HashSet<int> doorCells = new() { 3, 5, 7, 9 };
        static readonly Dictionary<char, int> moveCosts = new() { { 'A', 1 }, { 'B', 10 }, { 'C', 100 }, { 'D', 1000 } };

        static bool IsClear(int x1, int x2, HashSet<(int x, int y)> openCells)
        {
            for (var i = Math.Min(x1, x2); i <= Math.Max(x1, x2); i++)
            {
                if (!openCells.Contains((i, 1))) return false;
            }
            return true;
        }

        static string Key(Dictionary<(int x, int y), char> critters) => string.Concat(critters.OrderBy(kvp => (kvp.Value, kvp.Key)).Select(kvp => $"{kvp.Key.x}{kvp.Key.y}{kvp.Value}"));

        public static int ShrimpStacker(IEnumerable<string> input)
        {
            var map = Util.ParseSparseMatrix<char>(input);

            PriorityQueue<(HashSet<(int x, int y)> openCells, Dictionary<(int x, int y), char> critters, int score), int> queue = new();
            Dictionary<string, int> cache = new();

            queue.Enqueue((map.Where(kvp => kvp.Value == '.').Select(kvp => kvp.Key).ToHashSet(), map.Where(kvp => kvp.Value >= 'A' && kvp.Value <= 'D').ToDictionary(kvp => kvp.Key, kvp => kvp.Value), 0), 0);
            int bestScore = int.MaxValue;

            while (queue.TryDequeue(out var state, out var _))
            { 
                CloseCompleted(state.critters, state.openCells);

                if (!state.critters.Any())
                {
                    bestScore = Math.Min(bestScore, state.score);
                    continue;
                }

                var key = Key(state.critters);
                if (cache.TryGetValue(key, out int lastScore) && lastScore <= state.score) continue;
                cache[key] = state.score;

                List<(KeyValuePair<(int x, int y), char> critter, (int x, int y) destination, int spaces, bool home)> moves = new();

                foreach (var critter in state.critters)
                {
                    bool escaped = false;
                    if (critter.Key.y == 1 || state.openCells.Contains((critter.Key.x, critter.Key.y - 1)))
                    {
                        // see if this one can go home
                        int destX = destinationCol[critter.Value];
                        if (IsClear(critter.Key.x + Math.Sign(destX - critter.Key.x), destX, state.openCells))
                        {
                            for (var destY = 5; destY >= 2; --destY)
                            {
                                if (state.openCells.Contains((destX, destY)) && !state.openCells.Contains((destX, destY + 1)) && !state.critters.ContainsKey((destX, destY + 1)))
                                {
                                    moves.Add((critter, (destX, destY), Math.Abs(critter.Key.x - destX) + (destY - 1) + ((critter.Key.y != 1) ? critter.Key.y - 1 : 0), true));
                                    escaped = true;
                                    break;
                                }
                            }
                        }     
                    }
                    if (escaped) continue;

                    // move out of starting room into corridor
                    if (critter.Key.y >= 2 && state.openCells.Contains((critter.Key.x, critter.Key.y - 1)))
                    {
                        for (int dir = -1; dir <= 1; dir += 2)
                        {
                            for (int i = 1; i < 9; ++i)
                            {
                                var move = (pos: critter.Key.x + (i * dir), 1);
                                if (!doorCells.Contains(move.pos))
                                {
                                    if (state.openCells.Contains(move)) moves.Add((critter, move, critter.Key.y - 1 + i, false));                                  
                                    else break;                                
                                }
                            }
                        }                             
                    }
                }

                foreach (var move in moves)
                {
                    int newScore = state.score + move.spaces * moveCosts[move.critter.Value];
                    if (newScore > bestScore) continue;

                    var newCritters = new Dictionary<(int, int), char>(state.critters);
                    newCritters.Remove(move.critter.Key);
                    newCritters[move.destination] = move.critter.Value;

                    if (cache.TryGetValue(Key(newCritters), out lastScore) && lastScore < state.score) continue;

                    queue.Enqueue((state.openCells.Where(v => v != move.destination).Append(move.critter.Key).ToHashSet(), newCritters, newScore), newScore * (newCritters.Count - (move.home ? 1 : 0)));
                }
            }

            return bestScore;
        }

        static void CloseCompleted(Dictionary<(int x, int y), char> critters, HashSet<(int x, int y)> opencells)
        {
            var completed = destinations.Where(kvp => critters.ContainsKey(kvp.Key) && critters[kvp.Key] == kvp.Value && !critters.ContainsKey((kvp.Key.x, kvp.Key.y + 1)) && !opencells.Contains((kvp.Key.x, kvp.Key.y + 1))).Select(kvp => kvp.Key);
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