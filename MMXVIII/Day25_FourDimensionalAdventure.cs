using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.Utils.Vectors;

namespace Advent.MMXVIII
{
    public class Day25 : IPuzzle
    {
        public string Name { get { return "2018-25";} }
 
        class Graph
        {
            Dictionary<string, Node> index = new Dictionary<string, Node>();

            public IEnumerable<string> GetIndex() => index.Keys;  
            public IEnumerable<Node> GetNodes() => index.Values;
             
            public Node GetNode(ManhattanVector4 p)
            {
                var key = p.ToString();
                if (!index.ContainsKey(key))
                {
                    index[key] = new Node
                    {
                        position = p
                    };
                }

                return index[key];
            }

            public void AddLink(ManhattanVector4 p1, ManhattanVector4 p2)
            {
                var node1 = GetNode(p1);
                var node2 = GetNode(p2);

                node1.links.Add(node2);
                node2.links.Add(node1);
            }

            public int CountGroups()
            {
                return GetNodes().
                       Select(
                            node => string.Join(":", 
                                node.FindAllLinks().
                                Select(n => n.position.ToString()).
                                OrderBy(a=>a)).
                            GetHashCode()
                       ).Distinct().Count();
            }
        }

        class Node
        {
            public ManhattanVector4 position;
            public HashSet<Node> links = new HashSet<Node>();

            public HashSet<Node> FindAllLinks(HashSet<Node> seen = null)
            {
                if (seen == null) seen = new HashSet<Node>();
                foreach (var node in links)
                {
                    if (!seen.Contains(node))
                    {
                        seen.Add(node);
                        seen = node.FindAllLinks(seen);
                    }
                }

                return seen;
            }
        }

        public static int Part1(string input)
        {
            var data = Util.Parse<ManhattanVector4>(input);

            var Graph = new Graph();

            foreach (var item1 in data)
            {
                Graph.GetNode(item1);
                foreach (var item2 in data)
                {
                    if (item1.Distance(item2) <= 3)
                    {
                        Graph.AddLink(item1, item2);
                    }
                }              
            }

            return Graph.CountGroups();
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {


            //Console.WriteLine(Part1("0,0,0,0\n3,0,0,0\n0,3,0,0\n0,0,3,0\n0,0,0,3\n0,0,0,6\n9,0,0,0\n12,0,0,0\n"));
            logger.WriteLine("- Pt1 - "+Part1(input));
            //logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
