using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day08 : IPuzzle
    {
        public string Name => "2018-08";

        static Node BuildTree(string input) => new(new Queue<int>(Util.ParseNumbers<int>(input, ' ')));

        class Node
        {
            public Node(Queue<int> data)
            {
                var childCount = data.Dequeue();
                var metaCount = data.Dequeue();

                for (var i = 0; i < childCount; ++i) children.Add(new Node(data));
                for (var i = 0; i < metaCount; ++i) metaData.Add(data.Dequeue());
            }

            public int MetaTotal => metaData.Sum() + children.Sum(c => c.MetaTotal);
            public int Score => children.Count == 0 ? metaData.Sum() : metaData.Select(v => v - 1).Where(i => i >= 0 && i < children.Count).Sum(i => children[i].Score);

            readonly List<Node> children = new();
            readonly List<int> metaData = new();
        }

        public static int Part1(string input)
        {
            return BuildTree(input).MetaTotal;
        }

        public static int Part2(string input)
        {
            return BuildTree(input).Score;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}