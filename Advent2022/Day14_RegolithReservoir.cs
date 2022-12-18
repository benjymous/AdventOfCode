﻿using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day14 : IPuzzle
    {
        public string Name => "2022-14";

        private static IEnumerable<(int x, int y)> BuildMap(string input)
        {
            var pairs = Util.Split(input, '\n')
                .Distinct()
                .Select(line =>
                    Util.Parse<ManhattanVector2>(line.Split(" -> "))
                    .OverlappingPairs())
                .SelectMany(x => x);

            foreach (var (first, second) in pairs)
            {
                var delta = (Math.Sign(second.X - first.X), Math.Sign(second.Y - first.Y));

                for (var pos = first; pos != second; pos += delta)
                {
                    yield return pos;
                }
                yield return second;
            }
        }

        static readonly (int dx, int dy)[] moves = new[] { (0, 1), (-1, 1), (1, 1) };
        static (bool blocked, (int x, int y) pos) FindStep(HashSet<(int x, int y)> map, (int x, int y) pos, int floor)
        {
            if (pos.y + 1 < floor)
            {
                foreach (var (dx, dy) in moves)
                {
                    var next = (pos.x + dx, pos.y + dy);
                    if (!map.Contains(next)) return (false, next);
                }
            }
            return (true, pos);
        }

        private static int Simulate(string input, QuestionPart part)
        {
            var map = BuildMap(input).ToHashSet();

            int maxY = map.Max(kvp => kvp.y) + 2;
            int floor = part.Two() ? maxY : int.MaxValue;

            int count = 0;
            (int x, int y) source = (500, 0), sandPos = source;

            while (sandPos.y < maxY)
            {
                (var blocked, sandPos) = FindStep(map, sandPos, floor);
                if (blocked)
                {
                    map.Add(sandPos);
                    count++;
                    if (sandPos == source) break;
                    sandPos = source;
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