using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day11 : IPuzzle
    {
        public string Name => "2020-11";

        public class State
        {
            public State(int w, int h, HashSet<int> seats, QuestionPart p)
            {
                Width = w;
                Height = h;
                part = p;
                Seats = seats;
            }

            public int Height { get; private set; }
            public int Width { get; private set; }

            readonly QuestionPart part;

            public State(string input, QuestionPart p)
            {
                part = p;
                var lines = Util.Split(input);

                Height = lines.Length;
                Width = lines[0].Length;

                for (var y = 0; y < lines.Length; ++y)
                {
                    for (var x = 0; x < lines[y].Length; ++x)
                    {
                        var c = lines[y][x];
                        if (c != '.') Seats.Add(x + (y << 16));
                        if (c == '#') Occupied.Add(x + (y << 16));
                    }
                }
            }

            public readonly HashSet<int> Occupied = new();
            public readonly HashSet<int> Seats = new();

            int MaxOccupancy { get => part.One() ? 4 : 5; }

            public bool CheckDirection(int intpos, int dir)
            {
                if (part.One())
                {
                    return Occupied.Contains(intpos + dir);
                }
                else
                {
                    while (((intpos & 0xffff) >= 0) && ((intpos & 0xffff) < Width) && ((intpos >> 16) >= 0) && ((intpos >> 16) < Height))
                    {
                        intpos += dir;
                        if (Seats.Contains(intpos)) return Occupied.Contains(intpos);
                    }
                    return false;
                }
            }

            static readonly int[] directions = new int[]
            {
               (  0+  ( -1 << 16)), // N
               (  1+  ( -1 << 16)), // NE
               (  1+  (        0)), // E
               (  1+  (  1 << 16)), // SE
               (  0+  (  1 << 16)), // S 
               ( -1+  (  1 << 16)), // SW
               ( -1+  (        0)), // W
               ( -1+  ( -1 << 16))  // NW
            };

            public int Neighbours(int intpos) =>
                directions.Count(d => CheckDirection(intpos,d)==true);

            public bool Tick(State oldState, int intpos)
            {
                int neighbours = oldState.Neighbours(intpos);
                var oldVal = oldState.Occupied.Contains(intpos);
                switch (oldVal)
                {
                    case false: // empty seat
                        if (neighbours == 0)
                        {
                            Occupied.Add(intpos);
                            return true;
                        }
                        break;

                    case true: // occupied seat
                        if (neighbours >= MaxOccupancy)
                        {
                            return true;
                        }
                        break;
                }
                if (oldVal) Occupied.Add(intpos);
                return false;
            }
        }


        public static bool Tick(State oldState, State newState)
        {
            newState.Occupied.Clear();
            bool changed = false;

            foreach (var key in oldState.Seats)
            {
                changed |= newState.Tick(oldState, key);
            }
            return changed;
        }

        public static int Run(string input, QuestionPart part)
        {
            State s1 = new (input, part), s2 = new (s1.Width, s1.Height, s1.Seats, part);

            bool changed = true;
            int steps = 0;
            while (changed)
            {
                changed = Tick(s1, s2);

                (s1, s2) = (s2, s1);
                steps++;
            }

            System.Console.WriteLine(steps);

            return s1.Occupied.Count;
        }

        public static int Part1(string input)
        {
            return Run(input, QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return Run(input, QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}