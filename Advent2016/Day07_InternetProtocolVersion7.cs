using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day07 : IPuzzle
    {
        public string Name => "2016-07";

        static bool HasAbba(string address)
        {
            bool hasAbba = false;
            int bracketCount = 0;
            for (int i = 0; i < address.Length - 3; ++i)
            {
                if (address[i] == '[')
                {
                    bracketCount++;
                }
                else if (address[i] == ']')
                {
                    bracketCount--;
                }
                else if (address[i] == address[i + 3] && address[i + 1] == address[i + 2] && address[i] != address[i + 1])
                {
                    if (bracketCount > 0)
                    {
                        return false;
                    }
                    else
                    {
                        hasAbba = true;
                    }
                }
            }

            return hasAbba;
        }

        static bool HasAbaBab(string address)
        {
            int bracketCount = 0;

            HashSet<string> abas = new();
            HashSet<string> babs = new();

            for (int i = 0; i < address.Length - 2; ++i)
            {
                if (address[i] == '[')
                {
                    bracketCount++;
                }
                else if (address[i] == ']')
                {
                    bracketCount--;
                }
                else if (address[i] == address[i + 2] && address[i] != address[i + 1])
                {
                    var tla = $"{address[i]}{address[i + 1]}{address[i + 2]}";
                    if (bracketCount > 0)
                    {
                        babs.Add(tla);
                    }
                    else
                    {
                        abas.Add(tla);
                    }
                }
            }

            foreach (var aba in abas)
            {
                if (babs.Contains($"{aba[1]}{aba[0]}{aba[1]}")) return true;
            }

            return false;
        }

        public static int Part1(string input)
        {
            return Util.Split(input).Where(i => HasAbba(i)).Count();
        }

        public static int Part2(string input)
        {
            return Util.Split(input).Where(i => HasAbaBab(i)).Count();
        }

        public void Run(string input, ILogger logger)
        {
            // logger.WriteLine(HasAbba("--abba--"));
            // logger.WriteLine(HasAbba("abba--"));
            // logger.WriteLine(HasAbba("--abba"));
            // logger.WriteLine(HasAbba("--abab--"));
            // logger.WriteLine(HasAbba("--aba--"));

            // logger.WriteLine(HasAbba("abba[mnop]qrst"));  // true
            // logger.WriteLine(HasAbba("abcd[bddb]xyyx"));  // false
            // logger.WriteLine(HasAbba("aaaa[qwer]tyui"));  // false
            // logger.WriteLine(HasAbba("ioxxoj[asdfgh]zxcvbn"));  // true

            //logger.WriteLine(HasAbaBab("aba[bab]xyz")); // true
            //logger.WriteLine(HasAbaBab("xyx[xyx]xyx")); // false
            //logger.WriteLine(HasAbaBab("aaa[kek]eke")); // true
            //logger.WriteLine(HasAbaBab("zazbz[bzb]cdb")); // true

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}