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
       
            public Dictionary<string, DirNode> ChildDirs = new();
            public Dictionary<string, int> ChildFiles = new();

            public void Tree(int indent = 0)
            {
                if (indent == 0) Console.WriteLine("- / (dir)");
                foreach (var dir in ChildDirs)
                {
                    for (int i = 0; i < indent; ++i) Console.Write(" ");
                    Console.WriteLine($"- {dir.Key} (dir)");
                    dir.Value.Tree(indent + 2);
                }
                foreach (var file in ChildFiles)
                {
                    for (int i = 0; i < indent; ++i) Console.Write(" ");
                    Console.WriteLine($"- {file.Key} (file size={file.Value})");
                }
            }

        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input);
            Dictionary<string, DirNode> index = new();
            DirNode current = new();
            string path = "";
            index.Add(path, current);

            foreach (var line in lines)
            {
                var tokens = line.Split(" ");
                if (tokens[0] == "$")
                {
                    var command = tokens[1];
                    if (command == "cd")
                    {
                        var newPath = tokens[2];
                        if (newPath == "/")
                        {
                            path = "";
                            current = index[path];
                        }
                        else if (newPath == "..")
                        {
                            path = path.Substring(0, path.LastIndexOf("/"));
                            current = index[path];
                        }
                        else
                        {
                            path += "/" + newPath;
                            current.ChildDirs[newPath] = new();
                            index.Add(path, current);
                        }
                    }
                }
                else if (tokens[0][0].IsDigit())
                {
                    var filesize = int.Parse(tokens[0]);
                    var filename = tokens[1];
                    if (!current.ChildFiles.ContainsKey(filename))
                    {
                        current.ChildFiles.Add(filename, filesize);
                    }
                    else
                    {
                        Console.WriteLine("Duplicate file!");
                    }
                }
                else if (tokens[0] == "dir")
                {
                    var dirname = tokens[1];
                    if (!current.ChildDirs.ContainsKey(dirname))
                    {
                        current.ChildDirs.Add(dirname, new());
                    }
                    else
                    {
                        Console.WriteLine("Duplicate dir!");
                    }
                }
                else
                {
                    Console.WriteLine("Unexpected command!");
                }
            }

            index[""].Tree();

            return 0;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            string test = @"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k".Replace("\r", "");

            Part1(test);


            Console.WriteLine("- Pt1 - " + Part1(input));
            Console.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}