using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day06 : IPuzzle
    {
        public string Name { get { return "2019-06";} }
 
        public class Node
        {
            public string Name {get;set;}

            public Node Parent = null;

            public List<Node> Children {get;set;} = new List<Node>();

            public int GetDescendantCount() => Children.Count + Children.Select(c => c.GetDescendantCount()).Sum();
        }

        public class Tree
        {
            Dictionary<string, Node> index = new Dictionary<string, Node>();

            public IEnumerable<string> GetIndex() => index.Keys;  
            public IEnumerable<Node> GetNodes() => index.Values;      

            public Node GetNode(string name)
            {
                if (!index.ContainsKey(name))
                {
                    index[name] = new Node{Name=name};
                }
                return index[name];
            }

            public void AddPair(string parent, string child)
            {
                var p = GetNode(parent);
                var c = GetNode(child);
                p.Children.Add(c);
                c.Parent = p;
            }
        }
        
        public static Tree ParseTree(string input)
        {
            var tree = new Tree();
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

        public static List<string> TraverseUp(Node node)
        {
            var output = new List<string>();

            while (node.Parent != null)
            {
                output.Add(node.Parent.Name);
                node = node.Parent;
            }

            return output;
        }

        public static int Part2(string input)
        {
            var tree = ParseTree(input);
            var youUp = TraverseUp(tree.GetNode("YOU"));
            var santaUp = TraverseUp(tree.GetNode("SAN"));

            return Enumerable.Range(0,youUp.Count).Select( 
                    you => Enumerable.Range(0,santaUp.Count).Select(
                        santa => youUp[you] == santaUp[santa] ? you+santa : int.MaxValue
                    ).Min()
                ).Min();
        }

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - "+Part1(input));
            Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
