using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day25 : IPuzzle
    {
        public string Name => "2021-25";

        public static int Part1(string input)
        {
            var state = Util.ParseSparseMatrix<char>(input).Where(kvp => kvp.Value != '.').ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            bool moved = false;
            int moves = 0;

            var maxx = state.Keys.Max(k => k.x) + 1;
            var maxy = state.Keys.Max(k => k.y) + 1;

            Dictionary<(int x, int y), char> next;

            do
            {
                moved = false;

                for (int phase = 0; phase < 2; ++phase)
                {
                    var (moving, waiting) = phase == 0 ? ('>', 'v') : ('v', '>');

                    next = state.Where(kvp => kvp.Value == waiting).ToDictionary();

                    foreach (var cell in state.Where(kvp => kvp.Value == moving).Select(kvp => kvp.Key))
                    {
                        var dest = phase == 0 ? ((cell.x + 1) % maxx, cell.y) : (cell.x, (cell.y + 1) % maxy);
                        if (state.ContainsKey(dest)) dest = cell;
                        else moved = true;
                        next[dest] = moving;
                    }

                    state = next;
                }

                moves++;

            } while (moved);

            return moves;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}