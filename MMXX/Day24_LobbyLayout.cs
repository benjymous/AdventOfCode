using System;
using System.Collections.Generic;
using System.Linq;
using Advent.Utils;
using System.Text;

namespace Advent.MMXX
{
    public class Day24 : IPuzzle
    {
        public string Name { get { return "2020-24";} }

        static Dictionary<string, (int x, int y, int z)> directions = new Dictionary<string, (int x, int y, int z)>()
        {
            { "ne", (+1,-1, 0) },
            { "e",  (+1, 0, -1) },
            { "se", (0, +1, -1) },
            { "sw", (-1, +1, 0) },
            { "w",  (-1, 0, +1) },
            { "nw", (0, -1, +1) }
        };

        static (int x, int y, int z) Translate((int x, int y, int z) pos, (int x, int y, int z) dir) =>
            (pos.x + dir.x, pos.y + dir.y, pos.z + dir.z);

        static (int x, int y, int z) TranslateHex((int x, int y, int z) pos, string dir)
            => Translate(pos, directions[dir]);

        static IEnumerable<string> SplitCommands(string line)
        {
            for (int i=0; i<line.Length; ++i)
            {
                if (line[i] == 'w' || line[i]=='e')
                {
                    yield return line.Substring(i, 1);
                }
                else
                {
                    yield return line.Substring(i, 2);
                    i++;
                }
            }
        }

        static (int x, int y, int z) FollowPath(string path)
        {
            (int x, int y, int z) pos = (0,0,0);

            var steps = SplitCommands(path);
            foreach (var step in steps)
            {
                pos = TranslateHex(pos, step);
            }
            return pos;
        }

        public class State
        {
            public State()
            {
            }

            public State(HashSet<(int x, int y, int z)> initialState)
            {

                foreach (var v in initialState) Set(v, true);
            }

            public void Reset()
            {
                Cells.Clear();
                XRange.Reset();
                YRange.Reset();
            }

            Accumulator XRange = new Accumulator();
            Accumulator YRange = new Accumulator();
            public HashSet<(int x, int y, int z)> Cells { get; private set; } = new HashSet<(int x, int y, int z)>();

            public IEnumerable<(int x, int y, int z)> Range()
            {
                foreach (var y in YRange.RangeBuffered(1))
                {
                    foreach (var x in XRange.RangeBuffered(1))
                    {
                        yield return ((int)x, (int)y, (int)(-x-y));
                    }
                }
            }

            public void Set((int x, int y, int z) pos, bool v)
            {
                if (v)
                {
                    XRange.Add(pos.x);
                    YRange.Add(pos.y);
                    Cells.Add(pos);
                }
            }

            public bool Get((int x, int y, int z) pos)
                => Cells.Contains(pos);

            public bool CheckDirection((int x, int y, int z) pos, (int x, int y, int z) dir) =>
                Get((pos.x + dir.x, pos.y + dir.y, pos.z + dir.z));

            public int Neighbours((int x, int y, int z) pos) =>
                directions.Values.Where(d => CheckDirection(pos, d)).Count();

            public void Tick(State oldState, (int x, int y, int z) pos)
            {
                int neighbours = oldState.Neighbours(pos);
                var cellstate = oldState.Get(pos);
    
                if (cellstate)
                {
                    // Any black tile with zero or more than 2 black tiles immediately adjacent to it is flipped to white.
                    if (neighbours == 0 || neighbours > 2) cellstate = false;
                }
                else
                {
                    //Any white tile with exactly 2 black tiles immediately adjacent to it is flipped to black.
                    if (neighbours == 2) cellstate = true;
                }

                Set(pos, cellstate);
            }
        }


        public static void Tick(State oldState, State newState)
        {
            newState.Reset();
            foreach (var pos in oldState.Range())
            {
                newState.Tick(oldState, pos);
            }
        }

        public static int Run(HashSet<(int x, int y, int z)> initialState, int cycles)
        {
            State[] states = new State[2];
            states[0] = new State(initialState);
            states[1] = new State();

            int current = 0;
            while (cycles-- > 0)
            {
                var oldState = states[current];
                var newState = states[(current + 1) % 2];

                Tick(oldState, newState);

                current = 1 - current;
            }

            return states[current].Cells.Count();
        }

        static HashSet<(int x, int y, int z)> GetInitialState(string input)
        {
            var data = input.Trim().Split("\n");

            var counts = new Dictionary<(int x, int y, int z), int>();

            foreach (var line in data)
            {
                counts.IncrementAtIndex(FollowPath(line));
            }
            return new HashSet<(int x, int y, int z)>(counts.Where(kvp => (kvp.Value % 2 == 1)).Select(kvp => kvp.Key));
        }


        public static int Part1(string input)
        {
            var state = GetInitialState(input);
            return state.Count();
        }

        public static int Part2(string input)
        {
            var state = GetInitialState(input);
            return Run(state, 100);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}