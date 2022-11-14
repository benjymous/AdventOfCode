using AoC.Utils;
using AoC.Utils.Pathfinding;
using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day22 : IPuzzle
    {
        public string Name => "2016-22";

        class Node
        {
            public ManhattanVector2 position;
            public int Size;
            public int Used;
            public int Free;

            [Regex(@"\/dev\/grid\/node-x(\S+)-y(\S+)\s+(\S+)T\s+(\S+)T\s+(\S+)T\s+\S+%")]
            public Node(int x, int y, int size, int used, int free)
            {
                position = new ManhattanVector2(x, y);
                Size = size;
                Used = used;
                Free = free;
            }

            public override string ToString() => $"{position} : U{Used}:F{Free} [{Size}]";
        }

        static IEnumerable<Node> Parse(string input) => Util.RegexParse<Node>(Util.Split(input).Where(line => line.StartsWith("/dev/")));

        public static int Part1(string input)
        {
            var nodes = Parse(input);

            var pairs = nodes.Pairs().Where(pair => pair.Item1.Used > 0 && pair.Item1.Used <= pair.Item2.Free);

            return pairs.Count();
        }

        class Walkable : IIsWalkable<Node>
        {
            public Walkable(int max)
            {
                maxMovable = max;
            }

            readonly int maxMovable = 0;
            public bool IsWalkable(Node cell)
            {
                return cell.Used < maxMovable;
            }
        }

        public static int Part2(string input)
        {
            var nodes = Parse(input);

            var grid = nodes.ToDictionary(el => el.position.ToString(), el => el);
            var sourceX = nodes.Where(n => n.position.Y == 0).Select(n => n.position.X).Max();

            var empty = nodes.Where(n => n.Used == 0).First();

            GridMap<Node> map = new(new Walkable(empty.Free), grid);

            // move empty square to the left of the payload (avoiding the unmovable squares)
            var steps = AStar<ManhattanVector2>.FindPath(map, empty.position, new ManhattanVector2(sourceX, 0)).Count() - 1;

            steps++; // move payload left
            steps += 5 * (sourceX - 1); // repeated cycles of moving empty cell to left of payload, and moving payload left again

            return steps;
        }

        public void Run(string input, ILogger logger)
        {

            //string test = "Filesystem            Size  Used  Avail  Use%\n/dev/grid/node-x0-y0   10T    8T     2T   80%\n/dev/grid/node-x0-y1   11T    6T     5T   54%\n/dev/grid/node-x0-y2   32T   28T     4T   87%\n/dev/grid/node-x1-y0    9T    7T     2T   77%\n/dev/grid/node-x1-y1    8T    0T     8T    0%\n/dev/grid/node-x1-y2   11T    7T     4T   63%\n/dev/grid/node-x2-y0   10T    6T     4T   60%\n/dev/grid/node-x2-y1    9T    8T     1T   88%\n/dev/grid/node-x2-y2    9T    6T     3T   66%";

            //Util.Test(Part2(test), 7);

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}