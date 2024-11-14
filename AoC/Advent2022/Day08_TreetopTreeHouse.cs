namespace AoC.Advent2022;
public class Day08 : IPuzzle
{
    public class Data
    {
        public Data(string input)
        {
            grid = Util.ParseMatrix<char>(input);
            rows = grid.Rows().Select(c => c.ToArray()).ToArray();
            cols = grid.Columns().Select(c => c.ToArray()).ToArray();
        }
        public char[,] grid;
        public char[][] rows;
        public char[][] cols;

        public IEnumerable<(int x, int y)> All => grid.Keys();

        public bool VisibleFromEdge((int x, int y) pos)
            => VisibleFromEdge(rows[pos.y], pos.x) || VisibleFromEdge(cols[pos.x], pos.y);

        static bool VisibleFromEdge(char[] values, int index)
        {
            if (index == 0 || index == values.Length - 1) return true;
            var tree = values[index];
            if (tree == 0) return false;

            var visibleLeft = values.Take(index).All(v => v < tree);
            var visibleRight = values.Skip(index + 1).All(v => v < tree);
            return visibleLeft || visibleRight;
        }

        static int CountVisible(IEnumerable<char> values, int compare) => values.WithIndex(1).Where(v => v.Value >= compare).Select(v => v.Index).FirstOrDefault(values.Count());

        public int ScenicScore((int x, int y) pos)
        {
            var tree = grid[pos.x, pos.y];
            var (row, col) = (rows[pos.y], cols[pos.x]);

            return CountVisible(row.Take(pos.x).Reverse(), tree)
                 * CountVisible(row.Skip(pos.x + 1), tree)
                 * CountVisible(col.Take(pos.y).Reverse(), tree)
                 * CountVisible(col.Skip(pos.y + 1), tree);
        }

        public static implicit operator Data(string data) => new(data);
    }

    public static int Part1(Data data) => data.All.Count(data.VisibleFromEdge);

    public static int Part2(Data data) => data.All.Max(data.ScenicScore);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}