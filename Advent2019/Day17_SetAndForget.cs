using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC.Advent2019
{
    class Hoovamatic : NPSA.ASCIITerminal
    {
        public long DustCollected => finalOutput;

        public Hoovamatic(string input) : base(input)
        {
            cpu.Reserve(3500);
        }

        public void Activate() => cpu.Poke(0, 2);

        public void Run() => cpu.Run();

        public IEnumerable<(int x, int y)> FindIntersections()
        {
            for (int y = 1; y < buffer.Max.Y; ++y)
            {
                for (int x = 1; x < buffer.Max.X; ++x)
                {
                    if ((buffer.GetAt(x, y) == '#') &&
                        (buffer.GetAt(x - 1, y) == '#') &&
                        (buffer.GetAt(x + 1, y) == '#') &&
                        (buffer.GetAt(x, y - 1) == '#') &&
                        (buffer.GetAt(x, y + 1) == '#'))
                    {
                        yield return (x, y);
                    }
                }
            }
        }

        private static Dictionary<string, int> FindPatterns(string value)
        {
            List<string> patternToSearchList = new();
            for (int i = 0; i < value.Length; i++)
            {
                for (int j = 2; j <= value.Length / 2; j++)
                {
                    if (i + j <= value.Length)
                    {
                        patternToSearchList.Add(value.Substring(i, j));
                    }
                }
            }
            Dictionary<string, int> results = new();
            foreach (string pattern in patternToSearchList)
            {
                int occurence = Regex.Matches(value, pattern, RegexOptions.IgnoreCase).Count;
                var finalc = pattern.Reverse().Skip(1).First();
                var firstc = pattern.First();
                bool startsWithCommand = (firstc == 'L' || firstc == 'R');
                bool endsNumeric = finalc >= '0' && finalc <= '9';
                bool containsSubroutine = pattern.Contains('-');
                if (pattern.Length <= 21 && !containsSubroutine && startsWithCommand && pattern.EndsWith(",") && endsNumeric)
                {
                    results[pattern] = occurence;
                }
            }

            return results;
        }


        public static string Optimise(string unoptimised)
        {
            var shrunk = unoptimised.Replace(" ", "");

            if (!shrunk.EndsWith(",")) shrunk += ",";

            List<string> subs = new();

            var result = TrySubstitute(shrunk, subs);

            if (result == null)
            {
                return null;
            }

            return string.Join("\n", result);
        }

        static bool NeedsToShrink(string s) => s.Contains('L') || s.Contains('R');

        public static string Filter(string input)
        {
            input = input.Replace("-", "");

            if (input.EndsWith(","))
            {
                input = input[..^1];
            }

            return input;
        }

        public static IEnumerable<string> TestResult(string shrunk, List<string> used) => NeedsToShrink(shrunk) ? null : used.Prepend(Filter(shrunk));

        public static IEnumerable<string> TrySubstitute(string shrunk, List<string> used)
        {
            if (!NeedsToShrink(shrunk))
            {
                return TestResult(shrunk, used);
            }

            var patternDict = FindPatterns(shrunk);

            var patterns = patternDict.ToList().OrderBy(x => -(x.Key.Length + x.Value)).Select(kvp => kvp.Key).ToList();

            if (patterns.Count == 0)
            {
                return TestResult(shrunk, used);
            }

            foreach (var pattern in patterns)
            {
                var result = shrunk.Replace(pattern, $"-{(char)('A' + used.Count)}-,");
                var newUsed = new List<string>();
                newUsed.AddRange(used);
                newUsed.Add(Filter(pattern));

                var res = TestResult(result, newUsed);
                if (res != null)
                {
                    return res;
                }

                if (newUsed.Count < 3)
                {
                    var recurse = TrySubstitute(result, newUsed);
                    if (recurse != null)
                    {
                        return recurse;
                    }
                }
            }

            return null;
        }

        static IEnumerable<string> Compile(string program)
        {
            var shrunk = program.Replace(" ", "");

            var lines = shrunk.Split('\n');

            return lines;
        }

        int CountNeighbours(int x, int y)
        {
            int count = 0;
            if (buffer.GetAt(x - 1, y) == '#') count++;
            if (buffer.GetAt(x + 1, y) == '#') count++;
            if (buffer.GetAt(x, y - 1) == '#') count++;
            if (buffer.GetAt(x, y + 1) == '#') count++;
            return count;
        }

        string BuildPath()
        {
            var command = "";
            var position = buffer.FindCharacter('^');
            var direction = new Direction2(0, -1);
            while (true)
            {
                if (CountNeighbours(position.x, position.y) == 1 && command.Any()) break;

                int spins = 0;
                while (buffer.GetAt(position.x + direction.DX, position.y + direction.DY) != '#' || spins == 2)
                {
                    direction.TurnRight();
                    spins++;
                }

                if (spins == 1) command += "R,";
                else command += "L,";

                int distance = 0;
                while (buffer.GetAt(position.x + direction.DX, position.y + direction.DY) == '#')
                {
                    position = position.OffsetBy(direction);
                    distance++;
                }
                command += $"{distance}, ";

            }

            return command;
        }

        public override IEnumerable<string> AutomaticInput()
        {
            var route = BuildPath();

            var optimised = Optimise(route);

            string videoFeedFlag = buffer.DisplayLive ? "\ny\n" : "\nn\n";

            return Compile(optimised + videoFeedFlag);
        }
    }

    public class Day17 : IPuzzle
    {
        public string Name => "2019-17";

        public static int Part1(string input)
        {
            var robot = new Hoovamatic(input);

            robot.Run();

            var intersections = robot.FindIntersections();

            return intersections.Sum(v => v.x * v.y);
        }

        public static Int64 Part2(string input)
        {
            var robot = new Hoovamatic(input);
            robot.Activate();
            //robot.SetDisplay(true);
            robot.Run();

            return robot.DustCollected;
        }

        public void Run(string input, ILogger logger)
        {

            //Hoovamatic.Optimise("R,8,R,8,R,4,R,4,R,8,L,6,L,2,R,4,R,4,R,8,R,8,R,8,L,6,L,2");

            //var shrunk = Hoovamatic.Optimise("L,1, R,1, L,2, R,2, L,1, R,1, L,1, R,2, R,2");
            //var shrunk2 = Hoovamatic.Optimise(Hoovamatic.programUnoptimised);

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}