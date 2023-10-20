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
            public State(string input, QuestionPart p)
            {
                part = p;
                var data = Util.ParseSparseMatrix<char>(input, new Util.Convertomatic.SkipSpaces('.'));
                (Width, Height) = (data.Keys.Max(pos => pos.x), data.Keys.Max(pos => pos.y));
                Seats = data.Keys.Select(pos => pos.x + (pos.y << 16)).ToHashSet();
                Occupied = data.Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key.x + (kvp.Key.y << 16)).ToHashSet();
            }

            public State(State other) => (Width, Height, part, Seats) = (other.Width, other.Height, other.part, other.Seats);

            readonly int Height, Width;
            readonly QuestionPart part;
            public readonly HashSet<int> Occupied = new(), Seats = new();

            int MaxOccupancy { get => part.One() ? 4 : 5; }

            public int Neighbours(int intpos) => directions.Count(d => CheckDirection(intpos, d) == true);
            bool Inside(int intpos) => ((intpos & 0xffff) >= 0) && ((intpos & 0xffff) <= Width) && ((intpos >> 16) >= 0) && ((intpos >> 16) <= Height);

            public bool CheckDirection(int intpos, int dir)
            {
                if (part.One()) return Occupied.Contains(intpos + dir);
                else while (Inside(intpos += dir)) if (Seats.Contains(intpos)) return Occupied.Contains(intpos);
                return false;
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

            public bool Tick(State oldState, int intpos)
            {
                int neighbours = oldState.Neighbours(intpos);

                if (oldState.Occupied.Contains(intpos))
                {
                    if (neighbours >= MaxOccupancy) return true;
                    Occupied.Add(intpos);
                }
                else if (neighbours == 0)
                {
                    Occupied.Add(intpos);
                    return true;
                }

                return false;
            }
        }

        public static bool Tick(State oldState, State newState)
        {
            newState.Occupied.Clear();
            return oldState.Seats.Where(key => newState.Tick(oldState, key)).ToArray().Length != 0;
        }

        public static int Run(string input, QuestionPart part)
        {
            State s1 = new(input, part), s2 = new(s1);
            do (s1, s2) = (s2, s1); while (Tick(s1, s2));
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