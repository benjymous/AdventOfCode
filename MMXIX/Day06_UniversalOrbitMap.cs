using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day06 : IPuzzle
    {
        public string Name { get { return "2019-06";} }
 
        public class Node<TKeyType, TDataType>
        {
            public TKeyType Key {get;set;}

            public Node<TKeyType, TDataType> Parent = null;

            public List<Node<TKeyType, TDataType>> Children {get;set;} = new List<Node<TKeyType, TDataType>>();

            public int GetDescendantCount() => Children.Count + Children.Select(c => c.GetDescendantCount()).Sum();
        }

        public class Tree<TKeyType, TDataType>
        {
            Dictionary<TKeyType, Node<TKeyType, TDataType>> index = new Dictionary<TKeyType, Node<TKeyType, TDataType>>();

            public IEnumerable<TKeyType> GetIndex() => index.Keys;  
            public IEnumerable<Node<TKeyType, TDataType>> GetNodes() => index.Values;      

            public Node<TKeyType, TDataType> GetNode(TKeyType key)
            {
                if (!index.ContainsKey(key))
                {
                    index[key] = new Node<TKeyType, TDataType>{Key=key};
                }
                return index[key];
            }

            public void AddPair(TKeyType parent, TKeyType child)
            {
                var p = GetNode(parent);
                var c = GetNode(child);
                p.Children.Add(c);
                c.Parent = p;
            }

            public List<Node<TKeyType, TDataType>> TraverseToRoot(TKeyType key)
            {
                var output = new List<Node<TKeyType, TDataType>>();
                var node = GetNode(key);

                while (node.Parent != null)
                {
                    output.Add(node.Parent);
                    node = node.Parent;
                }

                return output;
            }
        }

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
