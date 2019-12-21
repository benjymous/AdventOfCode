using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Advent.MMXIX
{
    class Hoovamatic : NPSA.ASCIITerminal
    {
        public Int64 DustCollected => finalOutput;

        public Hoovamatic(string input) : base(input)
        {
        }

        public void Activate()
        {
            cpu.Poke(0, 2);
        }

        public void Run() => cpu.Run();

        public IEnumerable<ManhattanVector2> FindIntersections()
        {
            for (int y=1; y<buffer.Max.Y; ++y)
            {
                for (int x=1; x<buffer.Max.X; ++x)
                {
                    if ((buffer.GetAt(x,y)=='#') &&
                        (buffer.GetAt(x-1,y)=='#') &&
                        (buffer.GetAt(x+1,y)=='#') &&
                        (buffer.GetAt(x,y-1)=='#') &&
                        (buffer.GetAt(x,y+1)=='#'))
                    {
                        yield return new ManhattanVector2(x,y);
                    }
                }
            }
        }



      

        // ......................a#####b......................
        // ......................#.....#......................
        // ..................K#####L...#......................
        // ..................#...#.#...#......................
        // S#######T.........#...#.#...#......................
        // #.......#.........#...#.#...#......................
        // #.......#.Y###########Z.#...#......................
        // #.......#.#.......#.....#...#......................
        // #.......#.#.......#...######c......................
        // #.......#.#.......#.....#..........................
        // R#####Q.U#######V.J#######I........................
        // ......#...#.....#.......#.#........................
        // ......#...#.....#.......#.#........................
        // ......#...#.....#.......#.#........................
        // ......#...#.N###########M.#.........2#####3........
        // ......#...#.#...#.........#.........#.....#........
        // ......#...#.#...#.........H#######G.#.....#........
        // ......#...#.#...#.................#.#.....#........
        // ......#...X#####W.................#.#.....#........
        // ......#.....#.....................#.#.....#........
        // ......#.....#.....................#.#.....#........
        // ......#.....#.....................#.#.....#........
        // ......P#####O...........0###########1.C#######D....
        // ..................................#...#...#...#....
        // ..................................#...#...#...#....
        // ..................................#...#...#...#....
        // ..................................#...#...4#######5
        // ..................................#...#.......#...#
        // ..................................F###########E...#
        // ......................................#...........#
        // ................................A#####B...........#
        // ................................#.................#
        // ................................#.....7###########6
        // ................................#.....#............
        // ................................#.....#............
        // ................................#.....#............
        // ................................#.....#............
        // ................................#.....#............
        // ................................9#####8............

        public static string programUnoptimised = 
                "R,12, L,8, R,6,  "+
                "R,12, L,8, R,6,  " +
                "R,12, L,6, R,6, R,8, R,6, "+
                "L,8, R,8, R,6, R,12, " + // to F
                "R,12, L,8, R,6, " +
                "L,8, R,8, R,6, R,12, "+ //+ // to N
                "R,12, L,8, R,6, " +  
                "R,12, L,6, R,6, R,8, R,6, " + // to V
                "L,8, R,8, R,6, R,12, " + 
                "R,12, L,6, R,6, R,8, R,6, ";

        private static Dictionary<string, int> FindPatterns(string value)
        {
            List<string> patternToSearchList = new List<string>();
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
            Dictionary<string, int> results = new Dictionary<string, int>();
            foreach (string pattern in patternToSearchList)
            {
                int occurence = Regex.Matches(value, pattern, RegexOptions.IgnoreCase).Count;
                var finalc = pattern.Reverse().Skip(1).First();
                var firstc = pattern.First();
                bool startsWithCommand = (firstc == 'L' || firstc == 'R');
                bool endsNumeric = finalc >='0' && finalc <='9';
                bool containsSubroutine = pattern.Contains("-");                
                if (pattern.Length <= 21 && !containsSubroutine && startsWithCommand && pattern.EndsWith(",") && endsNumeric)
                {
                    results[pattern] = occurence;
                }
            }

            return results;
        }


        public static string Optimise(string unoptimised)
        {
            var shrunk = unoptimised.Replace(" ","");

            if (!shrunk.EndsWith(",")) shrunk = shrunk+",";

            List<string> subs = new List<string>();

            var result = TrySubstitute(shrunk, subs);

            if (result==null)
            {
                return null;
            }

            return string.Join("\n", result);
        }

        static bool NeedsToShrink(string s)
        {
            return s.Contains("L") || s.Contains("R");
        }

        public static string Filter(string input)
        {
            input = input.Replace("-", "");

            if (input.EndsWith(","))
            {
                input = input.Substring(0, input.Length-1);
            }

            return input;
        }

        public static List<string> TestResult(string shrunk, List<string> used)
        {
            if (NeedsToShrink(shrunk))
            {
                return null;
            }
            else
            {
                var result = new List<string>();
                result.Add(Filter(shrunk));
                result.AddRange(used);
                return result;
            }
        }

        public static List<string> TrySubstitute(string shrunk, List<string> used)
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

                var result = shrunk.Replace(pattern, $"-{(char)('A'+used.Count)}-,");
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

        IEnumerable<string> Compile(string program)
        {
            var shrunk = program.Replace(" ","");

            var lines = shrunk.Split('\n');

            return lines;
        }

        string BuildPath()
        {
            var position = buffer.FindCharacter('^');
            if (buffer.GetAt(position.X, position.Y-1)!='#')
            {
                
            }

            return "";
        }

        public override IEnumerable<string> AutomaticInput()
        {
            var prog = BuildPath();

            Console.WriteLine($"--{prog}--");

            var optimised = Optimise(programUnoptimised);

            string videoFeedFlag = buffer.DisplayLive ? "\ny\n" : "\nn\n";

            return Compile(optimised + videoFeedFlag);
        }
    }

    public class Day17 : IPuzzle
    {
        public string Name { get { return "2019-17";} }
 
        public static int Part1(string input)
        {
            var robot = new Hoovamatic(input);

            robot.Run();

            var intersections = robot.FindIntersections();

            return intersections.Select(v => v.X*v.Y).Sum();
        }

        public static Int64 Part2(string input)
        {
            var robot = new Hoovamatic(input);
            robot.Activate();
            //robot.SetDisplay(true);
            robot.Run();

            return robot.DustCollected;
        }

        public void Run(string input, System.IO.TextWriter console)
        {

            //Hoovamatic.Optimise("R,8,R,8,R,4,R,4,R,8,L,6,L,2,R,4,R,4,R,8,R,8,R,8,L,6,L,2");

            //var shrunk = Hoovamatic.Optimise("L,1, R,1, L,2, R,2, L,1, R,1, L,1, R,2, R,2");
            //var shrunk2 = Hoovamatic.Optimise(Hoovamatic.programUnoptimised);

            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}