using System;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day04 : IPuzzle
    {
        public string Name => "2017-04";

        public static bool ValidationRule1(string passphrase)
        {
            var words = passphrase.Split(" ");
            return !words.GroupBy(w => w)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key).Any();
        }

        public static bool ValidationRule2(string passphrase)
        {
            var words = passphrase.Split(" ").Select(x => String.Join("", x.ToCharArray().OrderBy(y => y)));
            return !words.GroupBy(w => w)
                        .Where(group => group.Count() > 1)
                        .Select(group => group.Key).Any();
        }

        // public static bool ValidationRule2(string passphrase)
        // {
        //     var words = passphrase.Split(" ");

        //     foreach (var word1 in words)
        //     {
        //         var perms1 = Permutations.Get(word1.ToCharArray()).Select(x => string.Join("",x));
        //         foreach (var word2 in words)
        //         {
        //             if (word1 != word2)
        //             {
        //                 var perms2 = Permutations.Get(word2.ToCharArray()).Select(x => string.Join("",x));

        //                 if (perms1.Intersect<string>(perms2).Any()) return false; // match found
        //             }
        //         }
        //     }

        //     return true;
        // }

        public static int Part1(string input)
        {
            var lines = Util.Split(input, '\n');

            return lines.Where(line => ValidationRule1(line))
                        .Count();
        }

        public static int Part2(string input)
        {
            var lines = Util.Split(input, '\n');

            return lines.Where(line => ValidationRule1(line))
                        .Where(line => ValidationRule2(line))
                        .Count();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}