using AoC.Utils.Collections;
using System.Linq;

namespace AoC.Advent2019
{
    public class Day06 : IPuzzle
    {
        public string Name => "2019-06";

        public static Tree<string> ParseTree(string input)
        {
            var tree = new Tree<string>();
            var data = input.Split();
            foreach (var line in data)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var bits = line.Split(')');
                tree.AddPair(bits[0], bits[1]);
            }
            return tree;
        }

        public static int Part1(string input)
        {
            var tree = ParseTree(input);
            return tree.GetNodes().Sum(n => n.GetDescendantCount());
        }

        public static int Part2(string input)
        {
            var tree = ParseTree(input);
            var youUp = tree.TraverseToRoot("YOU").ToArray();
            var santaUp = tree.TraverseToRoot("SAN").ToArray();

            return Util.Matrix(youUp.Length, santaUp.Length)
                       .Where(val => youUp[val.x] == santaUp[val.y])
                       .Select(val => val.x + val.y)
                       .Min();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
