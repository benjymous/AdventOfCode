using Advent.Utils.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Advent.MMXVII
{
    public class Day07 : IPuzzle
    {
        public string Name { get { return "2017-07"; } }

        static (string, int) ExtractParent(string data)
        {
            var key = data.Split(" ")[0];
            var value = Util.ExtractNumbers(data).First();
            return (key, value);
        }

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

                    tree.GetNode(parentKey).Value = parentValue;

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
                    tree.GetNode(parentKey).Value = parentValue;
                }


            }
            return tree;
        }

        public static string Part1(string input) => ParseTree(input).GetRoot().Key;

        static int GetChildScore(TreeNode<string, int> node)
        {
            int score = node.Value;
            foreach (var child in node.Children)
            {
                score += GetChildScore(child);
            }
            return score;
        }

        public static int Part2(string input)
        {
            var tree = ParseTree(input);

            var leaves = new HashSet<TreeNode<string, int>>();
            var currentParents = new HashSet<TreeNode<string, int>>();

            foreach (var node in tree.GetNodes())
            {
                if (node.Children.Count() == 0)
                {
                    leaves.Add(node);
                    currentParents.Add(node.Parent);
                }
            }

            // Find any leaves that have a missmatched weight

            while (currentParents.Any())
            {
                var newParents = new HashSet<TreeNode<string, int>>();
                foreach (var node in currentParents)
                {
                    var childWeights = node.Children.Select(child => (GetChildScore(child), child)).GroupBy(x => x.Item1).OrderBy(g => g.Count());

                    if (childWeights.Count() > 1)
                    {
                        // children weights are mismatched

                        int wrongScore = childWeights.First().First().Item1;
                        var wrongNode = childWeights.First().First().child;
                        int rightScore = childWeights.Last().First().Item1;

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

            //var testData = "pbga (66)\nxhth (57)\nebii (61)\nhavc (66)\nktlj (57)\nfwft (72) -> ktlj, cntj, xhth\nqoyq (66)\npadx (45) -> pbga, havc, qoyq\ntknk (41) -> ugml, padx, fwft\njptl (61)\nugml (68) -> gyxo, ebii, jptl\ngyxo (61)\ncntj (57)";
            //Util.Test(Part2(testData), 60);


            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}