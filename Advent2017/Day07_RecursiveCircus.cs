using AoC.Utils.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day07 : IPuzzle
    {
        public string Name => "2017-07";

        static (string, int) ExtractParent(string data) => (data.Split(" ")[0], Util.ExtractNumbers(data).First());

        public static Tree<string, int> ParseTree(string input)
        {
            var tree = new Tree<string, int>();
            var data = Util.Split(input, '\n');
            foreach (var line in data)
            {
                if (string.IsNullOrEmpty(line)) continue;

                if (line.Contains("->"))
                {
                    // parent and children
                    var bits = line.Split("->");

                    var (parentKey, parentValue) = ExtractParent(bits[0]);

                    tree.AddNode(parentKey, parentValue);

                    var children = Util.Split(bits[1], ',');
                    foreach (var child in children)
                    {
                        tree.AddPair(parentKey, child.Trim());
                    }
                }
                else
                {
                    // parent only
                    var (parentKey, parentValue) = ExtractParent(line);
                    tree.AddNode(parentKey, parentValue);
                }
            }
            return tree;
        }

        static int GetChildScore(TreeNode<string, int> node) => node.Value + node.Children.Sum(GetChildScore);

        public static string Part1(string input)
        {
            return ParseTree(input).GetRootKey();
        }

        public static int Part2(string input)
        {
            var tree = ParseTree(input);

            var currentParents = tree.Where(node => node.Children.Count == 0)
                                     .Select(node => node.Parent)
                                     .ToHashSet();

            // Find any leaves that have a missmatched weight
            while (currentParents.Count != 0)
            {
                var newParents = new HashSet<TreeNode<string, int>>();
                foreach (var node in currentParents)
                {
                    var childWeights = node.Children.Select(child => (score: GetChildScore(child), child)).GroupBy(x => x.score).OrderBy(g => g.Count());

                    if (childWeights.Count() > 1)
                    {
                        // children weights are mismatched

                        int wrongScore = childWeights.First().First().score;
                        var wrongNode = childWeights.First().First().child;
                        int rightScore = childWeights.Last().First().score;

                        int scoreChange = rightScore - wrongScore;

                        return wrongNode.Value + scoreChange;
                    }
                    else
                    {
                        newParents.Add(node.Parent);
                    }
                }
                currentParents = newParents;
            }

            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}