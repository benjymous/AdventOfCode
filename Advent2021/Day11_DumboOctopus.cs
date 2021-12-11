using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day11 : IPuzzle
    {
        public string Name { get { return "2021-11"; } }

        class State
        {
            public State(string input) => Cells = Util.ParseMatrix<int>(input);

            public int[,] Cells { get; private set; }

            static (int dx, int dy)[] directions = new (int, int)[] { (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1), (-1, 0), (-1, 1) };

            static IEnumerable<(int x, int y)> Neighbours((int x, int y) pos) => from dir in directions select (pos.x + dir.dx, pos.y + dir.dy);
            public int Step()
            {
                // inc all cells
                foreach (var pos in Cells.Keys()) Cells[pos.x, pos.y]++;

                // flash reaction
                int flashes = 0;
                bool flashed;
                do
                {
                    flashed = false;
                    foreach (var pos in Cells.Keys())
                    {
                        if (Cells[pos.x, pos.y] > 9)
                        {
                            Cells[pos.x, pos.y] = -100; // enough that it can't flash again this step
                            flashed = true;
                            flashes++;

                            foreach (var pos2 in Neighbours(pos)) Cells.TryIncrement(pos2);
                        }
                    }
                } while (flashed);

                // reset flashed
                foreach (var pos in Cells.Keys()) if (Cells[pos.x, pos.y] < 0) Cells[pos.x, pos.y] = 0;

                return flashes;
            }
        }

        public static int Part1(string input)
        {
            var state = new State(input);

            int count = 0;
            for (int i = 0; i < 100; i++)
            {
                count += state.Step();
            }

            return count;
        }

        public static int Part2(string input)
        {
            var state = new State(input);

            int step = 0;
            int count;

            int cellCount = state.Cells.Width() * state.Cells.Height();

            do
            {
                count = state.Step();
                step++;
            } while (count < cellCount);

            return step;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}