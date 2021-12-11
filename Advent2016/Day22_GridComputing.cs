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
            public int size;
            public int used;
            public int free;

            public Node(string line)
            {
                var bits = Util.ExtractNumbers(line.Select(c => (c >= '0' && c <= '9') ? c : ' '));
                position = new ManhattanVector2(bits[0], bits[1]);
                size = bits[2];
                used = bits[3];
                free = bits[4];
            }

            public override string ToString() => $"{position} : U{used}:F{free} [{size}]";
        }

        static IEnumerable<Node> Parse(string input) => Util.Parse<Node>(Util.Split(input).Where(line => line.StartsWith("/dev/")));

        public static int Part1(string input)
        {
            var nodes = Parse(input);

            var pairs = nodes.Pairs().Where(pair => pair.Item1.used > 0 && pair.Item1.used <= pair.Item2.free);

            return pairs.Count();
        }

        class Walkable : IIsWalkable<Node>
        {
            public Walkable(int max)
            {
                maxMovable = max;
            }
            int maxMovable = 0;
            public bool IsWalkable(Node cell)
            {
                return cell.used < maxMovable;
            }
        }

        public static int Part2(string input)
        {
            var nodes = Parse(input);

            var grid = nodes.ToDictionary(el => el.position.ToString(), el => el);
            var sourceX = nodes.Where(n => n.position.Y == 0).Select(n => n.position.X).Max();

            var empty = nodes.Where(n => n.used == 0).First();

            GridMap<Node> map = new GridMap<Node>(new Walkable(empty.free));
            map.data = grid;

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