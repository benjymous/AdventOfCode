using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day07 : IPuzzle
    {
        public string Name => "2022-07";

        class DirNode
        {
            public DirNode Parent = null;
            public List<DirNode> ChildDirs = new();
            public int FileSize = 0;

            int? _size = null;
            public int Size => _size ??= FileSize + ChildDirs.Sum(child => child.Size);
        }

        private static IEnumerable<DirNode> BuildTree(string input)
        {
            DirNode current = new();
            List<DirNode> index = new() { current };
            
            foreach (var tokens in Util.Split(input).Select(line => line.Split(' ')))
            {
                if (tokens[0] == "$" && tokens[1] == "cd")
                {
                    var newPath = tokens[2];
                    if (newPath == "/")
                    {
                        current = index.First();
                    }
                    else if (newPath == "..")
                    {
                        current = current.Parent;
                    }
                    else
                    {
                        DirNode child = new() { Parent = current };
                        current.ChildDirs.Add(child);
                        index.Add(child);
                        current = child;
                    }
                }
                else if (int.TryParse(tokens[0], out var fileSize))
                {
                    current.FileSize += fileSize;
                }
            }
            return index;
        }

        public static int Part1(string input)
        {
            var index = BuildTree(input).Select(d => d.Size);

            return index.Where(size => size <= 100000).Sum();
        }

        public static int Part2(string input)
        {
            var index = BuildTree(input).Select(d => d.Size);

            const int driveSize = 70000000;
            const int updateSize = 30000000;
            int rootSize = index.First();
            int availableSpace = driveSize - rootSize;
            int requiredSpace = updateSize - availableSpace;

            return index.Where(size => size >= requiredSpace).Order().First();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}