using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.Utils.Collections;

//using PlanetTree = Tree<string, Advent.MMXIX.Day06.none>;
//using Planet = TreeNode<string, Advent.MMXIX.Day06.none>;

namespace Advent.MMXIX
{
    public class Day06 : IPuzzle
    {
        public string Name { get { return "2019-06";} }
 
        public struct none {};
        
        public static Tree<string, none> ParseTree(string input)
        {
            var tree = new Tree<string, none>();
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
            return tree.GetNodes().Select(n => n.GetDescendantCount()).Sum();
        }

        public static int Part2(string input)
        {
            var tree = ParseTree(input);
            var youUp = tree.TraverseToRoot("YOU");
            var santaUp = tree.TraverseToRoot("SAN");

            return Util.Matrix(youUp.Count, santaUp.Count)
                       .Where(val => youUp[val.Item1] == santaUp[val.Item2])
                       .Select(val => val.Item1+val.Item2)
                       .Min();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
