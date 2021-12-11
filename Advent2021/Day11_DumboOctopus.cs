using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day11 : IPuzzle
    {
        public string Name => "2021-11";

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
                var toFlash = Cells.Entries().Where(entry => entry.value > 9);
                while (toFlash.Any())
                {
                    foreach (var cell in toFlash)
                    {
                        Cells[cell.key.x, cell.key.y] = -100; // enough that it can't flash again this step
                        foreach (var neighbour in Neighbours(cell.key)) Cells.TryIncrement(neighbour);
                    }
                }

                // reset flashed
                var flashed = Cells.Entries().Where(entry => entry.value < 0).ToList();
                flashed.ForEach(entry => Cells[entry.key.x, entry.key.y] = 0);

                return flashed.Count(); 
            }
        }

        public static int Part1(string input)
        {
            var state = new State(input);

            return Enumerable.Range(1, 100).Select(_ => state.Step()).Sum();
        }

        public static int Part2(string input)
        {
            var state = new State(input);
            int cellCount = state.Cells.Width() * state.Cells.Height();

            return Util.Forever(1).Where(_ => state.Step() == cellCount).First();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}