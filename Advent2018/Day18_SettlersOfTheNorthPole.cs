using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace AoC.Advent2018
{
    public class Day18 : IPuzzle
    {
        public string Name => "2018-18";

        const char OPEN = '.';
        const char TREES = '|';
        const char LUMBERYARD = '#';

        public static char Step(char current, IEnumerable<char> neighbours)
        {
            switch (current)
            {
                case OPEN:
                    // An open acre will become filled with trees if three or more adjacent acres
                    // contained trees. 
                    // Otherwise, nothing happens.
                    if (neighbours.Where(n => n == TREES).Count() >= 3) return TREES;
                    break;

                case TREES:
                    // An acre filled with trees will become a lumberyard if three or more adjacent
                    // acres were lumberyards. 
                    // Otherwise, nothing happens.
                    if (neighbours.Where(n => n == LUMBERYARD).Count() >= 3) return LUMBERYARD;
                    break;

                case LUMBERYARD:
                    // An acre containing a lumberyard will remain a lumberyard if it was adjacent
                    // to at least one other lumberyard and at least one acre containing trees.
                    // Otherwise, it becomes open.
                    if (neighbours.Where(n => n == LUMBERYARD).Any() &&
                        neighbours.Where(n => n == TREES).Any()) return LUMBERYARD;
                    else return OPEN;
            }

            return current;
        }

        static IEnumerable<char> Flat(char[][] data)
        {
            foreach (var line in data)
                foreach (var ch in line)
                    yield return ch;
        }

        public static int CalcHash(char[][] data)
        {
            return new BigInteger(Flat(data).Select(x => (byte)x).ToArray()).GetHashCode();
        }

        static int Count(char type, char[][] state)
        {
            var all = Flat(state);
            return all.Where(c => c == type).Count();
        }

        static char GetAt(char[][] input, int x, int y)
        {
            if (y < 0 || y >= input.Length) return '-';
            if (x < 0 || x >= input[y].Length) return '-';
            return input[y][x];
        }

        public static int Run(string input, int iterations)
        {
            var currentState = Util.Split(input).Select(line => line.ToArray()).ToArray();
            var newState = (char[][])currentState.Clone();

            var previous = new Queue<int>();
            int targetStep = -1;

            for (var i = 0; i < iterations; ++i)
            {
                if (targetStep == i)
                {
                    return Count(TREES, currentState) * Count(LUMBERYARD, currentState);
                }
                else if (targetStep == -1)
                { 
                    var hash = CalcHash(currentState);

                    var matchIdx = i - previous.Count;
                    foreach (var prev in previous)
                    {
                        matchIdx++;
                        if (prev == hash)
                        {
                            var cycleLength = i - matchIdx + 1;

                            if (cycleLength == 1)
                            {
                                targetStep = i + 1;
                            }
                            else
                            {
                                int cycleOffset = (i % cycleLength);
                                targetStep = i + ((iterations - cycleOffset) % cycleLength);
                            }                      
                        }
                    }

                    previous.Enqueue(hash);
                    if (previous.Count > 50) previous.Dequeue();
                }

                for (var y = 0; y < currentState.Length; ++y)
                {
                    List<char> line = new();
                    for (var x = 0; x < currentState[y].Length; ++x)
                    {
                        var cell = GetAt(currentState, x, y);
                        List<char> neighbours = new();

                        for (var y1 = y - 1; y1 <= y + 1; ++y1)
                        {
                            for (var x1 = x - 1; x1 <= x + 1; ++x1)
                            {
                                if (x != x1 || y != y1)
                                {
                                    neighbours.Add(GetAt(currentState, x1, y1));
                                }
                            }
                        }

                        line.Add(Step(cell, neighbours));
                    }
                    newState[y] = line.ToArray();
                }
                currentState = (char[][])newState.Clone();
            }

            return Count(TREES, currentState) * Count(LUMBERYARD, currentState);
        }

        public static int Part1(string input)
        {
            return Run(input, 10);
        }

        public static int Part2(string input)
        {
            return Run(input, 1000000000);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
