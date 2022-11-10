using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2021
{
    public class Day13 : IPuzzle
    {
        public string Name => "2021-13";

        class Fold
        {
            [Regex(@"fold along ([x|y])=(\d+)")]
            public Fold(char dir, int pos)
            {
                foldX = dir == 'x';
                foldLine = pos;
            }

            readonly bool foldX;
            readonly int foldLine;

            public IEnumerable<(int x, int y)> Perform(IEnumerable<(int x, int y)> dots)
            {
                return from dot in dots
                       select foldX
                           ? dot.x < foldLine ? dot : (foldLine - (dot.x - foldLine), dot.y)
                           : dot.y < foldLine ? dot : (dot.x, foldLine - (dot.y - foldLine));
            }
        }

        static string Display(HashSet<(int x, int y)> dots)
        {
            StringBuilder sb = new();
            sb.AppendLine();

            var maxX = dots.Max(v => v.x);
            var maxY = dots.Max(v => v.y);

            for (int y = 0; y <= maxY; ++y)
            {
                for (int x = 0; x <= maxX; ++x)
                {
                    sb.Append(dots.Contains((x, y)) ? "#" : ".");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        static (IEnumerable<(int x, int y)>, IEnumerable<Fold>) Parse(string input)
        {
            var bits = input.Split("\n\n");
            return 
            (
                Util.Parse<ManhattanVector2>(bits[0]).Select(v => ((int, int))v),
                Util.RegexParse<Fold>(bits[1])
            );
        }

        public static int Part1(string input)
        {
            (var dots, var folds) = Parse(input);

            var data = folds.First().Perform(dots).ToHashSet();

            return data.Count;
        }

        public static string Part2(string input, ILogger logger=null)
        {
            (var dots, var folds) = Parse(input);

            var data = folds.Aggregate(dots, (folded, fold) => fold.Perform(folded)).ToHashSet();

            var display = Display(data);
            if (logger != null) logger.WriteLine(display);

            return display.GetMD5String(false);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input, logger));
        }
    }
}