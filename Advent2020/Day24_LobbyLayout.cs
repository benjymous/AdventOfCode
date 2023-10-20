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
            for (int i = 0; i < line.Length; ++i) yield return line[i] == 'w' || line[i] == 'e' ? line.Substring(i, 1) : line.Substring(i++, 2);
        }

        static HexVector FollowPath(string path) => SplitCommands(path).Aggregate(new HexVector(0, 0, 0), (prev, step) => prev.TranslateHex(step));

        public class State
        {
            public State(int reserve) => Cells = new HashSet<int>(reserve);

            public State(HashSet<HexVector> initialState)
            {
                Cells = new HashSet<int>(initialState.Count);
                foreach (var v in initialState) Set(v.X, v.Y);
            }

            public void Reset()
            {
                Cells.Clear();
                Range.Reset();
            }

            public readonly Accumulator2D<int> Range = new();
            public HashSet<int> Cells { get; private set; }

            public void Set(int x, int y)
            {
                Range.Add(x,y);
                Cells.Add(x + (y << 16));
            }

            public bool Get(int x, int y) => Cells.Contains(x + (y << 16));

            public bool CheckDirection(int x, int y, (int x, int y, int z) dir) =>
                Get(x + dir.x, y + dir.y);

            public int Neighbours(int x, int y) =>
                HexVector.directions_pointy.Values.Count(d => CheckDirection(x, y, d));

            public void Tick(State oldState, int x, int y)
            {
                int neighbours = oldState.Neighbours(x, y);
                var cellstate = oldState.Get(x, y);

                if (cellstate && (neighbours == 0 || neighbours > 2)) cellstate = false;
                else if (!cellstate && neighbours == 2) cellstate = true;

                if (cellstate) Set(x, y);
            }
        }

        public static (State, State) Tick(State oldState, State newState)
        {
            newState.Reset();
            foreach (var (x, y) in oldState.Range.RangeBuffered(1)) newState.Tick(oldState, x, y);
            return (oldState, newState);
        }

        public static int Run(HashSet<HexVector> initialState, int cycles)
        {
            State s1 = new(initialState), s2 = new(initialState.Count);

            while (cycles-- > 0) (s2, s1) = Tick(s1, s2);

            return s1.Cells.Count;
        }

        static HashSet<HexVector> GetInitialState(string input) => input.Trim().Split("\n").Select(line => FollowPath(line)).GroupBy(v => v).Where(group => group.Count() % 2 == 1).Select(group => group.First()).ToHashSet();

        public static int Part1(string input)
        {
            return GetInitialState(input).Count;
        }

        public static int Part2(string input)
        {
            return Run(GetInitialState(input), 100);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}