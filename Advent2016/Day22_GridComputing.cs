using AoC.Utils;
using AoC.Utils.Pathfinding;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day22 : IPuzzle
    {
        public string Name => "2016-22";

        class Node
        {
            public readonly (int x, int y) Position;
            public readonly int Size, Used, Free;

            [Regex(@"\/dev\/grid\/node-x(\S+)-y(\S+)\s+(\S+)T\s+(\S+)T\s+(\S+)T\s+\S+%")]
            public Node(int x, int y, int size, int used, int free) => (Position, Size, Used, Free) = ((x, y), size, used, free);
        }

        static Node[] Parse(string input) => Util.RegexParse<Node>(Util.Split(input).Where(line => line.StartsWith("/dev/"))).ToArray();

        record struct Walkable(int MaxMovable) : IIsWalkable<Node>
        {
            public bool IsWalkable(Node cell) => cell.Used <= MaxMovable;
        }

        public static int Part1(string input)
        {
            var nodes = Parse(input).ToArray();
            var it1 = nodes.Where(n => n.Used > 0).ToArray();
            return nodes.Sum(node => it1.Count(node2 => node2.Used <= node.Free));
        }

        public static int Part2(string input)
        {
            var nodes = Parse(input).ToArray();

            var grid = nodes.ToDictionary(el => el.Position);
            var sourceX = nodes.Where(n => n.Position.y == 0).Max(n => n.Position.x);

            var empty = nodes.Where(n => n.Used == 0).First();

            GridMap<Node> map = new(new Walkable(empty.Free), grid);

            // move empty square to the left of the payload (avoiding the unmovable squares)
            var steps = map.FindPath(empty.Position, (sourceX, 0)).Length - 1;

            steps++; // move payload left
            steps += 5 * (sourceX - 1); // repeated cycles of moving empty cell to left of payload, and moving payload left again

            return steps;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}