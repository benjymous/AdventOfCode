using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.Utils;


namespace Advent.MMXX
{
    public class Day11 : IPuzzle
    {
        public string Name { get { return "2020-11";} }
 
      public class State
        {
            public State(int w, int h)
            {
                width = w;
                height = h;
                cells = new char[width, height];
            }

            public int height;
            public int width;

            public State(string input)
            {
                var lines = Util.Split(input);

                height = lines.Length;
                width = lines[0].Length;

                cells = new char[width, height];

                for (var y = 0; y < lines.Length; ++y)
                {
                    for (var x = 0; x < lines[y].Length; ++x)
                    {
                        Set(x, y, lines[y][x]);
                    }
                }
            }

            public char[,] cells;

            public void Set(int x, int y, char c) => cells[x, y] = c;

            public char Get(int x, int y = 0) => cells[x, y];

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
                                    count += (Get(x, y) == '#' ? 1 : 0);
                                }
                            }
                        }
                    }
                }
                return count;
            }

            public bool Tick(State oldState, int x, int y)
            {
                bool changed = false;           
                int neighbours = oldState.Neighbours(x, y);
                char old = oldState.Get(x, y);
                char newVal = old;
                if (old == 'L') // empty
                {
                    if (neighbours == 0)
                    {
                        newVal = '#';
                        changed = true;
                    }
                }
                else if (old == '#')
                {
                    if (neighbours >= 4)
                    {
                        newVal = 'L';
                        changed = true;
                    }
                }
                cells[x, y] = newVal;
                return changed;
            }

            public void Display()
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        var v = cells[x, y];
                        Console.Write(v);
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }


        public static bool Tick(State oldState, State newState)
        {
            bool changed = false;
            for (var y = 0; y < oldState.height; ++y)
            {
                for (var x = 0; x < oldState.width; ++x)
                {
                    changed |= newState.Tick(oldState, x, y);
                }
            }
            return changed;
        }

        public static int Run(string input)
        {
            Queue<State> states = new Queue<State>();
            states.Enqueue(new State(input));
            states.Enqueue(new State(states.First().width, states.First().height));

            bool changed = true;
            while (changed)
            {
                var oldState = states.Dequeue();
                var newState = states.Dequeue();

                changed = Tick(oldState, newState);

                newState.Display();

                states.Enqueue(newState);
                states.Enqueue(oldState);
            }

            var end = states.Dequeue();
            var count = end.cells.Values().Where(v => v == '#').Count();

            return count;
        }

        public static int Part1(string input)
        {
            return Run(input);
            return 0;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}