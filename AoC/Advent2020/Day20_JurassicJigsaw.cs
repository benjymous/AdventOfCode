namespace AoC.Advent2020;
public class Day20 : IPuzzle
{
    enum Edge { top = 0, right, bottom, left }

    public class Tile
    {
        public Tile(string input)
        {
            var lines = input.Split("\n");
            ID = int.Parse(lines[0].Substring(5, 4));
            Grid = lines.Skip(1).ToArray();
            Edges = [Grid.First(), (Grid.Select(row => row.Last()).AsString()), Grid.Last().Reversed(), Grid.Select(row => row.First()).AsString().Reversed()];
            (w, h) = (Grid.Length - 1, Grid[0].Length - 1);
        }

        public int ID, Orientation = 0, Borders = 0;
        public string[] Grid, Edges;
        private readonly int w, h;

        public void Twizzle()
        {
            Orientation = (Orientation + 1) % 8;
            Edges = [Edges[3], Edges[0], Edges[1], Edges[2]]; // rotate clockwise
            if (Orientation is 0 or 4) Edges = Edges.Select(e => e.Reversed()).Reverse().ToArray(); // flip over
        }

        public void RotateToFit(int edgePos, string matchingEdge) => Util.RepeatWhile(() => Edges[edgePos] != matchingEdge, Twizzle);

        readonly Func<int, int, int, int, (int x, int y)>[] Transforms = [(x, y, w, h) => (x, y), (x, y, w, h) => (y, w - x), (x, y, w, h) => (w - x, h - y), (x, y, w, h) => (h - y, x), (x, y, w, h) => (y, x), (x, y, w, h) => (w - x, y), (x, y, w, h) => (h - y, w - x), (x, y, w, h) => (x, h - y)];

        char GetCell((int x, int y) pos) => Grid[pos.y][pos.x];
        public char GetCellTransformed(int x, int y) => GetCell(Transforms[Orientation](x, y, w, h));
    }

    public class JigsawSolver
    {
        public JigsawSolver(string input)
        {
            Tiles = Util.Parse<Tile>(input.Trim().Split("\n\n"));
            EdgeMap = Tiles.SelectMany(t => t.Edges.Select(e => (t, e))).SelectMany(v => Util.Values((v.t, v.e), (v.t, v.e.Reversed()))).GroupBy(v => v.Item2).ToDictionary(g => g.Key, g => g.Select(x => x.t).ToArray());
            foreach (var tile in Tiles) tile.Borders = tile.Edges.Count(e => EdgeMap[e].Length == 1);
        }

        public readonly List<Tile> Tiles;
        public readonly Dictionary<string, Tile[]> EdgeMap;

        Tile FindNeighbour(Tile nextTo, Edge position)
        {
            var edge = nextTo.Edges[(int)position];

            var neighbourTile = EdgeMap[edge].SingleOrDefault(x => x != nextTo);
            neighbourTile?.RotateToFit(((int)position + 2) % 4, edge.Reversed());
            return neighbourTile;
        }

        public int Solve()
        {
            var nessieIdentifier = Util.ParseSparseMatrix<bool>("                  #\n#    ##    ##    ###\n #  #  #  #  #  #").Keys.ToHashSet();
            foreach (var corner in Corners)
            {
                while (!(EdgeMap[corner.Edges[(int)Edge.top]].Length == 1 && EdgeMap[corner.Edges[(int)Edge.left]].Length == 1)) corner.Twizzle();
                (int x, int y) pos = (0, 0);
                var solution = new Dictionary<(int x, int y), Tile> { [pos] = corner };

                for (var tile = corner; tile != null;)
                    if ((tile = FindNeighbour(tile, Edge.right)) != null) solution[pos = (pos.x + 1, pos.y)] = tile;
                    else if ((tile = FindNeighbour(solution[(0, pos.y)], Edge.bottom)) != null) solution[pos = (0, pos.y + 1)] = tile;

                HashSet<int> bitmap = solution.Select(kvp => (pos: kvp.Key, cell: kvp.Value)).SelectMany(v => Util.Range2DExclusive((0, 8, 0, 8)).Where(cellPos => v.cell.GetCellTransformed(cellPos.x + 1, cellPos.y + 1) == '#').Select(cellPos => (v.pos.x * 8) + cellPos.x + (((v.pos.y * 8) + cellPos.y) << 16))).ToHashSet();
                var nessies = Util.Range2DExclusive((0, ((pos.y + 1) * 8) - 2, 0, ((pos.x + 1) * 8) - 19)).Select(offset => nessieIdentifier.Select(e => offset.x + e.x + ((offset.y + e.y) << 16))).Where(keys => keys.All(k => bitmap.Contains(k))).SelectMany(v => v).Count();
                if (nessies != 0) return bitmap.Count - nessies;
            }
            return 0;
        }

        public IEnumerable<Tile> Corners => Tiles.Where(t => t.Borders == 2);
    }

    public static long Part1(string input) => new JigsawSolver(input).Corners.Product(t => t.ID);

    public static long Part2(string input) => new JigsawSolver(input).Solve();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}