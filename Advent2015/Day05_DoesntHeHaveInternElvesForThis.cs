using AoC.Utils;
using System;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day05 : IPuzzle
    {
        public string Name => "2015-05";

        public static bool HasVowels(string line, int count)
        {
            int vowelCount = line.Count(c => c.IsVowel());

            return vowelCount >= count;
        }

        public static bool HasRun(string line)
        {
            return line.OverlappingPairs().Where(pair => pair.first == pair.second).Any();
        }

        static readonly string[] bads = new string[]
        {
            "ab", "cd", "pq", "xy"
        };

        public static bool NoBads(string line)
        {
            return !bads.Any(line.Contains);
        }

        public static bool IsNice1(string line)
        {
            return HasVowels(line, 3) && HasRun(line) && NoBads(line);
        }

        public static bool HasNonOverlappingPair(string line)
        {
            for (int i = 0; i < line.Length - 1; ++i)
            {
                for (int j = 0; j < line.Length - 1; ++j)
                {
                    if (Math.Abs(i - j) >= 2)
                    {
                        if (line[i] == line[j] && line[i + 1] == line[j + 1])
                            return true;
                    }
                }
            }
            return false;
        }

        public static bool HasGapRepeat(string line)
        {
            for (int i = 0; i < line.Length - 2; ++i)
            {
                if (line[i] == line[i + 2]) return true;
            }
            return false;
        }


        public static bool IsNice2(string line)
        {
            return HasNonOverlappingPair(line) && HasGapRepeat(line);
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input);
            return lines.Count(IsNice1);
        }

        public static int Part2(string input)
        {
            var lines = Util.Split(input);
            return lines.Count(IsNice2);
        }

        public void Run(string input, ILogger logger)
        {

            // Console.WriteLine(HasVowels("aei", 3));  // true
            // Console.WriteLine(HasVowels("xazegov", 3)); //true
            // Console.WriteLine(HasVowels("aeiouaeiouaeiou", 3));  // true
            // Console.WriteLine(HasVowels("aaz", 3)); // false

            // Console.WriteLine(IsNice1("ugknbfddgicrmopn")); //true
            // Console.WriteLine(IsNice1("aaa")); //true
            // Console.WriteLine(IsNice1("jchzalrnumimnmhp")); //false
            // Console.WriteLine(IsNice1("haegwjzuvuyypxyu")); //false
            // Console.WriteLine(IsNice1("dvszwmarrgswjxmb")); //false

            //Console.WriteLine(HasNonOverlappingPair("xyxy")); // true
            //Console.WriteLine(HasNonOverlappingPair("aabcdefgaa")); // true
            //Console.WriteLine(HasNonOverlappingPair("aaa")); // false

            //Console.WriteLine(HasGapRepeat("xyx")); // true
            //Console.WriteLine(HasGapRepeat("efe")); // true
            //Console.WriteLine(HasGapRepeat("aaa")); // true
            //Console.WriteLine(HasGapRepeat("abba")); // false

            //Console.WriteLine(IsNice2("qjhvhtzxzqqjkmpb")); //true
            //Console.WriteLine(IsNice2("xxyxx")); //true
            //Console.WriteLine(IsNice2("uurcxstgmygtbstg")); //false
            //Console.WriteLine(IsNice2("ieodomkazucvgmuy")); //false

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}