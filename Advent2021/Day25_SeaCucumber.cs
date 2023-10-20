using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day25 : IPuzzle
    {
        public string Name => "2021-25";

        static int ToKey((int x, int y) v) => v.x + (v.y * 1000);
        static (int x, int y) FromKey(int v) => (v % 1000, v / 1000);

        public static int Part1(string input)
        {
            var stateRaw = Util.ParseSparseMatrix<char>(input).Where(kvp => kvp.Value != '.');
            var state = stateRaw.ToDictionary(kvp => ToKey(kvp.Key), kvp => kvp.Value);

            bool moved = false;
            int moves = 0;

            var maxx = stateRaw.Max(k => k.Key.x) + 1;
            var maxy = stateRaw.Max(k => k.Key.y) + 1;

            Dictionary<int, char> next;

            do
            {
                moved = false;

                for (int phase = 0; phase < 2; ++phase)
                {
                    var (moving, waiting) = phase == 0 ? ('>', 'v') : ('v', '>');

                    next = state.Where(kvp => kvp.Value == waiting).ToDictionary();

                    foreach (var (key, cell) in state.Where(kvp => kvp.Value == moving).Select(kvp => (kvp.Key, FromKey(kvp.Key))))
                    {
                        var dest = ToKey(phase == 0 ? ((cell.x + 1) % maxx, cell.y) : (cell.x, (cell.y + 1) % maxy));
                        if (state.ContainsKey(dest)) dest = key;
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