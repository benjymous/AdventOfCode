using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day18 : IPuzzle
    {
        public string Name { get { return "2015-18"; } }

        public class State
        {
            public State(int w, int h)
            {
                width = w;
                height = h;
                cells = new bool[width, height];
            }

            public int height;
            public int width;

            public State(string input)
            {
                var lines = Util.Split(input);

                height = lines.Length;
                width = lines[0].Length;

                cells = new bool[width, height];

                for (var y = 0; y < lines.Length; ++y)
                {
                    for (var x = 0; x < lines[y].Length; ++x)
                    {
                        if (lines[y][x] == '#')
                        {
                            Set(x, y);
                        }
                    }
                }
            }

            public bool[,] cells;

            public void Set(int x, int y) => cells[x, y] = true;

            public int Get(int x, int y = 0) => cells[x, y] ? 1 : 0;

            int Neighbours(int xs, int ys)
            {
                int count = 0;
                for (int x = xs - 1; x <= xs + 1; x++)
                {
                    if (x >= 0 && x < width)
                    {
                        for (int y = ys - 1; y <= ys + 1; y++)
                        {
                            if (y >= 0 && y < height)
                            {
                                if (x != xs || y != ys)
                                {
                                    count += Get(x, y);
                                }
                            }
                        }
                    }
                }
                return count;
            }

            public void Tick(State oldState, int x, int y)
            {
                int neighbours = oldState.Neighbours(x, y);
                bool newVal = false;
                if (oldState.Get(x, y) == 1)
                {
                    if (neighbours == 2 || neighbours == 3)
                    {
                        newVal = true;
                    }
                }
                else
                {
                    if (neighbours == 3)
                    {
                        newVal = true;
                    }
                }
                cells[x, y] = newVal;
            }

            public void Display()
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        var v = cells[x, y];
                        Console.Write(v ? '.' : '#');
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            public void StuckCorners()
            {
                Set(0, 0);
                Set(0, height - 1);
                Set(width - 1, height - 1);
                Set(width - 1, 0);
            }
        }


        public static void Tick(State oldState, State newState)
        {
            for (var y = 0; y < oldState.height; ++y)
            {
                for (var x = 0; x < oldState.width; ++x)
                {
                    newState.Tick(oldState, x, y);
                }
            }
        }

        public static int Run(string input, int steps, bool stuckCorners = false)
        {
            Queue<State> states = new Queue<State>();
            states.Enqueue(new State(input));
            states.Enqueue(new State(states.First().width, states.First().height));

            for (int i = 0; i < steps; ++i)
            {
                var oldState = states.Dequeue();
                var newState = states.Dequeue();

                if (stuckCorners)
                {
                    oldState.StuckCorners();
                }

                Tick(oldState, newState);

                if (stuckCorners)
                {
                    newState.StuckCorners();
                }

                //newState.Display();

                states.Enqueue(newState);
                states.Enqueue(oldState);
            }

            var end = states.Dequeue();
            var count = end.cells.Values().Where(v => v == true).Count();

            return count;
        }

        public static int Part1(string input)
        {
            return Run(input, 100);
        }
        public static int Part2(string input)
        {
            return Run(input, 100, true);
        }

        public void Run(string input, ILogger logger)
        {

            //logger.WriteLine(Run(".#.#.#\n...##.\n#....#\n..#...\n#.#..#\n####..", 4));


            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}