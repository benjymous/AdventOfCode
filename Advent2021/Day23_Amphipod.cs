﻿using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2021
{
    public class Day23 : IPuzzle
    {
        public string Name => "2021-23";

        static Dictionary<(int x, int y), char> destinations = new()
        {
            { (3, 2), 'A' },
            { (5, 2), 'B' },
            { (7, 2), 'C' },
            { (9, 2), 'D' },

            { (3, 3), 'A' },
            { (5, 3), 'B' },
            { (7, 3), 'C' },
            { (9, 3), 'D' },

            { (3, 4), 'A' },
            { (5, 4), 'B' },
            { (7, 4), 'C' },
            { (9, 4), 'D' },

            { (3, 5), 'A' },
            { (5, 5), 'B' },
            { (7, 5), 'C' },
            { (9, 5), 'D' },
        };

        static Dictionary<char, int> destinationCol = new()
        {
            { 'A', 3 },
            { 'B', 5 },
            { 'C', 7 },
            { 'D', 9 }
        };

        static HashSet<int> doorCells = new() { 3, 5, 7, 9 };

        static Dictionary<char, int> moveCosts = new()
        {
            { 'A', 1 },
            { 'B', 10 },
            { 'C', 100 },
            { 'D', 1000 },
        };

        static bool isClear(int x1, int x2, HashSet<(int x, int y)> openCells)
        {
            int low = Math.Min(x1, x2);
            int high = Math.Max(x1, x2);
            for (int i = low; i <= high; i++)
            {
                if (!openCells.Contains((i, 1))) return false;
            }
            return true;
        }

        static string Key(Dictionary<(int x, int y), char> critters)
        {
            string key = "";
            foreach (var kvp in critters.OrderBy(kvp => (kvp.Value, kvp.Key)))
            {
                key += kvp.ToString();
            }
            return key; 
        }

        public static int ShrimpStacker(IEnumerable<string> input)
        {
            var map = Util.ParseSparseMatrix<char>(input);


            var initialState = (
                map.Where(kvp => kvp.Value == '.').Select(kvp => kvp.Key).ToHashSet(),
                map.Where(kvp => kvp.Value >= 'A' && kvp.Value <= 'D').ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                0
            );

            PriorityQueue<(HashSet<(int x, int y)> openCells, Dictionary<(int x, int y), char> critters, int score), int> queue = new();
            Dictionary<string, int> seen = new();


            queue.Enqueue(initialState, 0);
            int bestScore = int.MaxValue;

            int steps = 0;

            while (queue.Count > 0)
            {            
                var state = queue.Dequeue();

                if ((steps++ % 100000) == 0)
                {
                    Display(state.openCells, state.critters);
                    Console.WriteLine($"{bestScore}");
                }

                if (CloseCompleted(state.critters, state.openCells))
                {
                    queue.Enqueue((state.openCells, state.critters, state.score), state.critters.Count);
                    continue;
                }

                if (!state.critters.Any())
                {
                    bestScore = Math.Min(bestScore, state.score);
                    Console.WriteLine($"Completed with {state.score} points");
                    //Display(state.openCells, state.critters);
                    continue;
                }

                var key = Key(state.critters);
                if (seen.TryGetValue(key, out int lastScore))
                {
                    if (lastScore <= state.score) continue;
                }

                seen[key] = state.score;

                if (state.score > bestScore) continue;

                //Console.WriteLine($"[{queue.Count}] - current score: {state.score}");
                //Display(state.openCells, state.critters);


                List<(KeyValuePair<(int x, int y), char> critter, (int x, int y) destination, int spaces, bool home)> moves = new();

                foreach (var critter in state.critters)
                {
                    if (critter.Key.y == 1 || state.openCells.Contains((critter.Key.x, critter.Key.y-1)))
                    {
                        // see if this one can go home
                        int destX = destinationCol[critter.Value];
                        
                        if (state.openCells.Contains((destX, 5)) || state.openCells.Contains((destX, 4)) || state.openCells.Contains((destX, 3)) || state.openCells.Contains((destX, 2)) )
                        {
                            int sign = Math.Sign(destX - critter.Key.x);
                            if (isClear(critter.Key.x+sign, destX, state.openCells))
                            {
                                int destY = 0;

                                for (var testY = 5; testY >=2; --testY)
                                {
                                    if (state.openCells.Contains((destX, testY)) && !state.openCells.Contains((destX, testY+1)) && !state.critters.ContainsKey((destX, testY+1)))
                                    {
                                        destY = testY;
                                        break;
                                    }
                                }

                                //if (state.openCells.Contains((destX, 2)) && !state.openCells.Contains((destX, 3)) && !state.critters.ContainsKey((destX, 3)))
                                //{
                                //    destY = 2;
                                //}
                                //if (state.openCells.Contains((destX, 3)))
                                //{
                                //    destY = 3;
                                //}
                                if (destY > 0)
                                {
                                    int distance = Math.Abs(critter.Key.x - destX);
                                    if (critter.Key.y != 1)
                                    {
                                        distance += critter.Key.y - 1;
                                    }
                                    distance += (destY-1);
                                    //Console.WriteLine($"Go home {critter}");
                                    //Display(state.openCells, state.critters);
                                    moves.Add((critter, (destX, destY), distance, true));
                                }
                            }
                        }
                    }
        
                    if (critter.Key.y >= 2 )
                    {
                        // move out of starting room into corridor
                        if (state.openCells.Contains((critter.Key.x, critter.Key.y - 1)))
                        {                             
                            // check corridor right
                            for (int i = 1; i < 9; ++i)
                            {       
                                var move = (critter.Key.x + i, 1);
                                if (state.openCells.Contains(move))
                                {
                                    if (!doorCells.Contains(move.Item1))
                                    {
                                        int distance = critter.Key.y - 1 + i;
                                        //Console.WriteLine($"{critter} could move {distance} to {move}");
                                        moves.Add((critter, move, distance, false));
                                    }
                                }
                                else
                                {
                                    break;
                                }
                               
                            }
                            // check corridor left
                            for (int i = 1; i < 9; ++i)
                            {
                                var move = (critter.Key.x - i, 1);
                                if (state.openCells.Contains(move))
                                {
                                    if (!doorCells.Contains(move.Item1))
                                    {
                                        int distance = critter.Key.y - 1 + i;
                                        //Console.WriteLine($"{critter} could move {distance} to {move}");
                                        moves.Add((critter, move, distance, false));
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                     
                        }
                    }
                }
                //Console.WriteLine($"{moves.Count} possible moves");

                foreach (var move in moves)
                {

                    int newScore = state.score + move.spaces * moveCosts[move.critter.Value];
                    if (newScore > bestScore) continue;

                    var newCells = state.openCells.ToHashSet();
                    var newCritters = state.critters.ToDictionary(kvp=>kvp.Key, kvp => kvp.Value);

                    newCells.Remove(move.destination);
                    newCells.Add(move.critter.Key);
                    newCritters.Remove(move.critter.Key);
                    newCritters[move.destination] = move.critter.Value;

                    if (seen.TryGetValue(Key(newCritters), out lastScore))
                    {
                        if (lastScore < state.score) continue;
                    }

                    //int priority = newScore;
                    int priority = newScore * (newCritters.Count - (move.home ? 1 : 0));
                    queue.Enqueue((newCells, newCritters, newScore), priority);
                }
            }

            return bestScore;
        }

        public static bool CloseCompleted(Dictionary<(int x, int y), char> critters, HashSet<(int x, int y)> opencells)
        {
            bool anyRemoved = false;
            bool removed = true;
            while (removed)
            {
                removed = false;
                foreach (var kvp in destinations)
                {
                    if (critters.ContainsKey(kvp.Key) && critters[kvp.Key] == kvp.Value)
                    {

                        if (!critters.ContainsKey((kvp.Key.x, kvp.Key.y+1)) && !opencells.Contains((kvp.Key.x, kvp.Key.y + 1)))
                        {
                           
                            critters.Remove(kvp.Key);
                            //Console.WriteLine($"{kvp} has reached home");
                            removed = true;
                            anyRemoved = true;
                        }
                    }
                }
            }
            return anyRemoved;
        }

        public static void Display(HashSet<(int x, int y)> openCells, Dictionary<(int x, int y), char> critters)
        {
            Console.WriteLine($"{critters.Count} to rehome");
            Console.WriteLine("   123456789AB");
            for (int y=0; y<7; ++y)
            {
                Console.Write(y);
                Console.Write(" ");
                for (int x=0; x<13; ++x)
                {
                    char c = '#';
                    if (openCells.Contains((x, y))) c = '.';
                    if (critters.ContainsKey((x, y)))
                    {
                        c = critters[(x,y)];
                    }
                    Console.Write(c);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static int Part1(string input)
        {
            return ShrimpStacker(input.Split('\n'));
        }

        public static int Part2(string input)
        {
            var lines = input.Split('\n');
            var insert = new string[] { "  #D#C#B#A#", "  #D#B#A#C#" };
            var unfolded = lines.Take(3).Union(insert).Union(lines.Skip(3));
            return ShrimpStacker(unfolded);
        }

        public void Run(string input, ILogger logger)
        {

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));

        }
    }
}