using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXV
{
    public class Day18 : IPuzzle
    {
        public string Name { get { return "2015-18";} }

        public class State
        {
            public State(int w, int h)
            {
                width = w;
                height = h;
            }

            public int height;
            public int width;


            public State(string input)
            {
                var lines = Util.Split(input);

                height = lines.Length;
                width = lines[0].Length;

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

            public void Clear()
            {
                cells.Clear();
            }

            public Dictionary<string,int> cells = new Dictionary<string, int>();

            public static int Bit(int x, int y) => 1 << ((y * 5) + x);

            public void Set(int x, int y)
            {
                cells.PutStrKey($"{x},{y}",1);
            }

            public int Get(int x, int y = 0)
            {
                if (x < 0 || y < 0 || x >= width || y >= height)
                {
                    return 0;
                }
                else return cells.GetStrKey($"{x},{y}");
            }

            IEnumerable<(int x, int y)> GetNeighbours(int xs, int ys)
            {
                for (int x=xs-1; x<=xs+1; x++)
                {
                    for (int y=ys-1; y<=ys+1; y++)
                    {
                        if (x!=xs || y!=ys)
                            yield return (x,y);
                    }
                }
            }

            public int Neighbours(int x, int y) => GetNeighbours(x, y).Select(n => Get(n.x, n.y)).Sum();

            public void Tick(State oldState, int x, int y)
            {
                int neighbours = oldState.Neighbours(x, y);
                if (oldState.Get(x, y) == 1)
                {
                    if (neighbours == 2 || neighbours == 3)
                    {
                        Set(x, y);
                    }
                }
                else
                {
                    if (neighbours == 3)
                    {
                        Set(x, y);
                    }
                }
            }

            public void Display()
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int x = 0; x < width; ++x)
                    {
                        var v = cells.GetStrKey($"{x},{y}");
                        Console.Write(v == 0 ? '.' : '#');
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            public void StuckCorners()
            {
                Set(0,0);
                Set(0, height-1);
                Set(width-1, height-1);
                Set(width-1, 0);
            }
        }


        public static void Tick(State oldState, State newState)
        {
            newState.Clear();
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

            for (int i=0; i<steps; ++i)
            {
                var oldState = states.Dequeue();
                var newState = states.Dequeue();

                oldState.StuckCorners();

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
            var count = end.cells.Where(kvp => kvp.Value==1).Count();

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

        public void Run(string input, System.IO.TextWriter console)
        {
            
            //console.WriteLine(Run(".#.#.#\n...##.\n#....#\n..#...\n#.#..#\n####..", 4));


            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}