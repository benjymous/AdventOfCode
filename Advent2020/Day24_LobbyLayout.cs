using AoC.Utils;
using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2020
{
    public class Day24 : IPuzzle
    {
        public string Name => "2020-24";

        static IEnumerable<string> SplitCommands(string line)
        {
            for (int i = 0; i < line.Length; ++i)
            {
                if (line[i] == 'w' || line[i] == 'e')
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

        static HexVector FollowPath(string path)
        {
            HexVector pos = new(0, 0, 0);

            var steps = SplitCommands(path);
            foreach (var step in steps)
            {
                pos.TranslateHex(step);
            }
            return pos;
        }

        public class State
        {
            public State() {}

            public State(HashSet<HexVector> initialState)
            {
                foreach (var v in initialState) Set(v);
            }

            public void Reset()
            {
                Cells.Clear();
                XRange.Reset();
                YRange.Reset();
            }

            readonly Accumulator XRange = new();
            readonly Accumulator YRange = new();
            public HashSet<HexVector> Cells { get; private set; } = new HashSet<HexVector>();

            public IEnumerable<HexVector> Range()
            {
                foreach (var y in YRange.RangeBuffered(1))
                {
                    foreach (var x in XRange.RangeBuffered(1))
                    {
                        yield return new HexVector((int)x, (int)y, (int)(-x - y));
                    }
                }
            }

            public void Set(HexVector pos)
            {
                XRange.Add(pos.X);
                YRange.Add(pos.Y);
                Cells.Add(pos);
            }

            public bool Get(HexVector pos)
                => Cells.Contains(pos);

            public bool CheckDirection(HexVector pos, (int x, int y, int z) dir) =>
                Get(new HexVector(pos.X + dir.x, pos.Y + dir.y, pos.Z + dir.z));

            public int Neighbours(HexVector pos) =>
                pos.Directions.Values.Where(d => CheckDirection(pos, d)).Count();

            public void Tick(State oldState, HexVector pos)
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

                if (cellstate) Set(pos);
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

        public static int Run(HashSet<HexVector> initialState, int cycles)
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

            return states[current].Cells.Count;
        }

        static HashSet<HexVector> GetInitialState(string input)
        {
            var data = input.Trim().Split("\n");

            var counts = new Dictionary<HexVector, int>();

            foreach (var line in data)
            {
                counts.IncrementAtIndex(FollowPath(line));
            }
            return new HashSet<HexVector>(counts.Where(kvp => (kvp.Value % 2 == 1)).Select(kvp => kvp.Key));
        }


        public static int Part1(string input)
        {
            var state = GetInitialState(input);
            return state.Count;
        }

        public static int Part2(string input)
        {
            var state = GetInitialState(input);
            return Run(state, 100);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}