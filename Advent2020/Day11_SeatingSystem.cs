using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day11 : IPuzzle
    {
        public string Name { get { return "2020-11"; } }

        public class State
        {
            public State(int w, int h, Util.QuestionPart p)
            {
                Width = w;
                Height = h;
                part = p;
                Cells = new char[Width, Height];
            }

            public int Height { get; private set; }
            public int Width { get; private set; }
            Util.QuestionPart part;

            public State(string input, Util.QuestionPart p)
            {
                part = p;
                var lines = Util.Split(input);

                Height = lines.Length;
                Width = lines[0].Length;

                Cells = new char[Width, Height];

                for (var y = 0; y < lines.Length; ++y)
                {
                    for (var x = 0; x < lines[y].Length; ++x)
                    {
                        Set(x, y, lines[y][x]);
                    }
                }
            }

            public char[,] Cells { get; private set; }

            int MaxOccupancy { get => part == Util.QuestionPart.Part1 ? 4 : 5; }

            public void Set(int x, int y, char c) => Cells[x, y] = c;

            public char Get(int x, int y = 0) =>
                (x < 0 || x >= Width || y < 0 || y >= Height) ? ' ' : Cells[x, y];

            public char CheckDirection(int x, int y, int dx, int dy)
            {
                if (part == Util.QuestionPart.Part1)
                {
                    return Get(x + dx, y + dy);     
                }
                else
                {
                    while (true)
                    {
                        x += dx;
                        y += dy;
                        char c = Get(x, y);
                        if (c != '.') return c;
                    }
                }
            }

            static (int dx, int dy)[] directions = new (int, int)[]
            {
                (0, 1),   // N
                (1, 1),   // NE
                (1, 0),   // E
                (1, -1),  // SE
                (0, -1),  // S 
                (-1, -1), // SW
                (-1, 0),  // W
                (-1, 1),  // NW
            };

            public int Neighbours(int xs, int ys) =>
                directions.Select(d => CheckDirection(xs, ys, d.dx, d.dy)).Where(d => d == '#').Count();

            public bool Tick(State oldState, int x, int y)
            {
                int neighbours = oldState.Neighbours(x, y);
                var oldVal = oldState.Get(x, y);
                switch (oldVal)
                {
                    case 'L': // empty seat
                        if (neighbours == 0)
                        {
                            Cells[x, y] = '#';
                            return true;
                        }
                        break;

                    case '#': // occupied seat
                        if (neighbours >= MaxOccupancy)
                        {
                            Cells[x, y] = 'L';
                            return true;
                        }
                        break;
                }
                Cells[x, y] = oldVal;
                return false;
            }

            public void Display()
            {
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {
                        Console.Write(Cells[x, y]);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }


        public static bool Tick(State oldState, State newState)
        {
            bool changed = false;
            for (var y = 0; y < oldState.Height; ++y)
            {
                for (var x = 0; x < oldState.Width; ++x)
                {
                    changed |= newState.Tick(oldState, x, y);
                }
            }
            return changed;
        }

        public static int Run(string input, Util.QuestionPart part)
        {
            State[] states = new State[2];
            states[0] = new State(input, part);
            states[1] = new State(states[0].Width, states[0].Height, part);

            bool changed = true;
            int current = 0;
            while (changed)
            {
                var oldState = states[current];
                var newState = states[(current + 1) % 2];

                changed = Tick(oldState, newState);

                //newState.Display();

                current = 1 - current;
            }

            return states[current].Cells.Values().Where(v => v == '#').Count();
        }

        public static int TestNeighbours(string input, int tx, int ty)
        {
            var state = new State(input, Util.QuestionPart.Part2);
            if (state.Get(tx, ty) != 'L') return -1;
            return state.Neighbours(tx, ty);
        }

        public static int Part1(string input)
        {
            return Run(input, Util.QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return Run(input, Util.QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}