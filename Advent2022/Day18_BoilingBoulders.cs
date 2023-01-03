using AoC.Utils;
using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day18 : IPuzzle
    {
        public string Name => "2022-18";

        public class Node
        {
            public Node(string line) => Pos = new ManhattanVector3(line).AsSimple();
            public Node((int, int, int) p) => Pos = p;

            public readonly (int x, int y, int z) Pos;

            public ((int, int, int), (int, int, int))[] Edges() => new[]
            {
                ((Pos.x-1, Pos.y, Pos.z), Pos),
                (Pos, (Pos.x+1, Pos.y, Pos.z)),
                ((Pos.x, Pos.y-1, Pos.z), Pos),
                (Pos, (Pos.x, Pos.y+1, Pos.z)),
                ((Pos.x, Pos.y, Pos.z-1), Pos),
                (Pos, (Pos.x, Pos.y, Pos.z+1)),
            };
        }

        private static int CountOuterEdges(IEnumerable<Node> cells) 
            => cells.SelectMany(cell => cell.Edges()).GetUniqueItems().Count();

        static (int dx, int dy, int dz)[] Neighbours = new[] { (-1, 0, 0), (1, 0, 0), (0, -1, 0), (0, 1, 0), (0, 0, -1), (0, 0, 1) };
        static void FloodFill((int x, int y, int z) pos, HashSet<(int x, int y, int z)> matrix)
        {
            if (matrix.Contains(pos))
            {
                matrix.Remove(pos);
                foreach (var (dx, dy, dz) in Neighbours)
                    FloodFill((pos.x + dx, pos.y + dy, pos.z + dz), matrix);
            }
        }

        static (int minZ, int maxZ, int minY, int maxY, int minX, int maxX) GetRange(IEnumerable<Node> input)
            => (input.Min(k => k.Pos.z), input.Max(k => k.Pos.z),
                input.Min(k => k.Pos.y), input.Max(k => k.Pos.y),
                input.Min(k => k.Pos.x), input.Max(k => k.Pos.x));

        public static int Part1(string input)
        {
            return CountOuterEdges(Util.Parse<Node>(input));
        }

        public static int Part2(string input)
        {
            var cells = Util.Parse<Node>(input).ToArray();

            var range = GetRange(cells);
            var airPositions = Util.Range3DInclusive(range).Except(cells.Select(c => c.Pos)).ToHashSet();
            var boundaries = airPositions.Where(c => c.x == range.minX || c.x == range.maxX || c.y == range.minY || c.y == range.maxY || c.z == range.minZ || c.z == range.maxZ);
            
            while (boundaries.Any()) FloodFill(boundaries.First(), airPositions);
            
            return CountOuterEdges(cells.Union(airPositions.Select(p => new Node(p))));
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}