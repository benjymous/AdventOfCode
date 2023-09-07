﻿using AoC.Utils;
using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day14 : IPuzzle
    {
        public string Name => "2022-14";

        private static IEnumerable<int> BuildMap(string input)
        {
            var pairs = Util.Split(input, '\n')
                .Distinct()
                .SelectMany(line =>
                    Util.Parse<ManhattanVector2>(line.Split(" -> "))
                    .OverlappingPairs());

            foreach (var (first, second) in pairs)
            {
                var delta = second.Delta(first);
                for (var pos = first; pos != second; pos += delta) yield return pos.X + (pos.Y<<16);
                yield return second.X + (second.Y <<16);
            }
        }

        static readonly int[] moves = new[] { 0, -1, 1 };
        static (bool blocked, int pos) FindStep(HashSet<int> map, int pos, int floor, Stack<int> lastBranch)
        {
            if (pos < floor)
            {
                foreach (var delta in moves)
                {
                    var next = pos + delta + (1<<16);
                    if (!map.Contains(next))
                    {
                        if (delta != 1) lastBranch.Push(pos);
                        return (false, next);
                    }
                }
            }
            return (true, pos);
        }

        private static int Simulate(string input, QuestionPart part)
        {
            var map = BuildMap(input).ToHashSet();

            int maxY = (map.Max(kvp => kvp >> 16) + 2) << 16;
            int floor = part.Two() ? (maxY - (1<<16)) : int.MaxValue;

            int count = 0, source = 500, sandPos = source;
            Stack<int> lastBranch = new();

            while (sandPos < maxY)
            {
                (var blocked, sandPos) = FindStep(map, sandPos, floor, lastBranch);
                if (blocked)
                {
                    map.Add(sandPos);
                    count++;
                    if (sandPos >> 16 == 0) break;
                    if (!lastBranch.TryPop(out sandPos)) sandPos = source;
                }
            }

            return count;
        }

        public static int Part1(string input)
        {
            return Simulate(input, QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return Simulate(input, QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}