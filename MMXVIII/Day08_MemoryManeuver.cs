using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day08 : IPuzzle
    {
        public string Name { get { return "2018-08";} }

        static int metaTotal = 0;
 
        static Node BuildTree(string input)
        {
            var data = new Queue<int>(Util.Parse(input, ' '));

            metaTotal = 0;
            return Read(data);
        }

        class Node
        {
            public List<Node> children = new List<Node>();
            public List<int> metaData = new List<int>();
        }

        static Node Read(Queue<int> data)
        {

            var childCount = data.Dequeue();
            var metaCount = data.Dequeue();

            var node = new Node();

            for (var i=0;i<childCount;++i) 
            {
                var child = Read(data);
                node.children.Add(child);
            }

            for (var i=0; i<metaCount; ++i) {
                var meta =  data.Dequeue();
                node.metaData.Add(meta);
                metaTotal += meta;
            }
            return node;

        }

        static int GetScore(Node node)
        {
            var count = 0;
            if (node.children.Count() == 0) 
            {
                count += node.metaData.Sum();
            }
            else 
            {
                foreach (var m in node.metaData)
                {
                    var childIdx = m-1;

                    if (childIdx >=0 && childIdx < node.children.Count()) {
                        count += GetScore(node.children[childIdx]);
                    }
                }
            }

            return count;
        }

        public static int Part1(string input)
        {
            var tree = BuildTree(input);
            return metaTotal;
        }

        public static int Part2(string input)
        {
            var tree = BuildTree(input);
            return GetScore(tree);
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}