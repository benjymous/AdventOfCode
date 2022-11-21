using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day12 : IPuzzle
    {
        public string Name => "2018-12";

        private static string Step(Dictionary<string, char> rules, string current)
        {
            List<char> next = new() { '.', '.' };
            for (var i = 0; i < current.Length - 5; ++i)
            {
                var sub = current.Substring(i, 5);

                if (rules.TryGetValue(sub, out var rule))
                {
                    next.Add(rule);
                }
                else
                {
                    next.Add('.');
                }
            }
            next.AddRange("..");
            current = next.AsString();
            return current;
        }

        private static void ParseInput(string input, out string initialState, out Dictionary<string, char> rules)
        {
            var lines = Util.Split(input);
            initialState = lines[0].Split(": ")[1];
            rules = new();
            foreach (var line in lines.Skip(1))
            {
                var bits = line.Split(" => ");
                rules[bits[0]] = bits[1][0];
            }
        }

        public static int Part1(string input)
        {
            ParseInput(input, out var initialState, out var rules);

            var left = 0;

            for (var i = 0; i < 30; ++i)
            {
                initialState = "." + initialState + ".";
                left--;
            }

            var current = initialState;

            for (var gen = 0; gen < 20; ++gen)
            {
                current = Step(rules, current);
            }

            var sum = 0;
            for (var i = 0; i < current.Length; ++i)
            {
                if (current[i] == '#')
                {
                    sum += (i + left);
                }
            }

            return sum;
        }

        public static Int64 Part2(string input)
        {
            ParseInput(input, out var initialState, out var rules);

            var left = 0;

            var current = initialState;
            var previous = "";


            int gen = 0;
            int lastLeft;
            while (true)
            {
                // Keep the line padded with 5 .s either side
                lastLeft = left;
                for (var z = 0; z < 5; ++z)
                {
                    current = "." + current + ".";
                    left--;
                }

                var i = current.IndexOf('#');
                if (i > 5)
                {
                    current = current[(i - 5)..];
                    left += (i - 5);
                }
                var j = current.LastIndexOf("#");
                if ((current.Length - j - 1) > 5)
                {
                    current = current[..(j + 6)];
                }

                if (current == previous) break; // We've got a stable pattern


                previous = current;
                current = Step(rules, current) + ".....";
                gen++;

            }

            var turnStep = left - lastLeft; // we progress this many cells each turn

            // we'd progress this many cells over 50 billion turns
            Int64 finalLeft = left + ((50000000000 - gen) * turnStep);

            Int64 sum = 0;
            for (var i = 0; i < current.Length; ++i)
            {
                if (current[i] == '#')
                {
                    sum += (i + finalLeft);
                }
            }

            return sum;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}