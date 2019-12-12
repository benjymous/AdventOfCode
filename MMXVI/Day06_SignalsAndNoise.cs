﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVI
{
    public class Day06 : IPuzzle
    {
        public string Name { get { return "2016-06";} }
 
        static List<Dictionary<char,int>> BuildDataMaps(string input)
        {
            var lines = Util.Split(input);

            var first = lines.First();

            var storage = new List<Dictionary<char,int>>();
            foreach (var c in first)
            {
                storage.Add(new Dictionary<char, int>());
            }

            foreach (var line in lines)
            {
                for (int i=0; i<line.Length; ++i)
                {
                    storage[i].IncrementAtIndex(line[i]);
                }
            }
            return storage;
        }

        public static string Part1(string input)
        {
            var storage = BuildDataMaps(input);
            var processed = storage.Select(row => row.Select(kvp => Tuple.Create(kvp.Value, kvp.Key)).OrderBy(t => -t.Item1).First().Item2);

            return processed.AsString();
        }

        public static string Part2(string input)
        {
            var storage = BuildDataMaps(input);
            var processed = storage.Select(row => row.Select(kvp => Tuple.Create(kvp.Value, kvp.Key)).OrderBy(t => t.Item1).First().Item2);

            return processed.AsString();
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}