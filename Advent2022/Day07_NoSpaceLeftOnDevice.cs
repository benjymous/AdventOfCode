using AoC.Utils;
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

            public int Size => FileSize + ChildDirs.Sum(child => child.Size);

            public DirNode CreateChild(List<DirNode> index)
            {
                DirNode child = new() { Parent = this };
                ChildDirs.Add(child);
                index.Add(child);
                return child;
            }
        }

        private static int[] BuildTree(string input)
        {
            DirNode current = new();
            List<DirNode> index = new() { current };

            foreach (var tokens in Util.Split(input).Select(line => line.Split(' ')))
            {
                if (tokens[0] == "$" && tokens[1] == "cd")
                {
                    current = tokens[2] switch
                    {
                        "/" => index.First(),
                        ".." => current.Parent,
                        _ => current.CreateChild(index)
                    };
                }
                else if (int.TryParse(tokens[0], out var fileSize))
                {
                    current.FileSize += fileSize;
                }
            }
            return index.Select(d => d.Size).ToArray();
        }

        public static int Part1(string input)
        {
            return BuildTree(input).Where(size => size <= 100000).Sum();
        }

        public static int Part2(string input)
        {
            var sizes = BuildTree(input);

            const int driveSize = 70000000;
            const int updateSize = 30000000;
            int rootSize = sizes.First();
            int availableSpace = driveSize - rootSize;
            int requiredSpace = updateSize - availableSpace;

            return sizes.Where(size => size >= requiredSpace).Min();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}