using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXV
{
    public class Day11 : IPuzzle
    {
        public string Name { get { return "2015-11";} }

        static char[] bads = new char[]
        {
            'i', 'o', 'l', 
        };

        public static bool IsBad(char c)
        {
            return bads.Contains(c);
        }

        public static string Increment(string pwd)
        {
            var newPwd = pwd.ToArray();
            int i = pwd.Length-1;
            while (true)
            {
                newPwd[i]++;
                if (newPwd[i]<='z')
                {
                    if (!IsBad(newPwd[i]))
                    {
                        return String.Join("",newPwd);
                    }
                }
                else
                {
                    newPwd[i]='a';
                    i--;
                }
                if (i<0) throw new Exception("Password roll under!");
            }
        }

        public static bool HasStraight(string line)
        {
            for (int i=0; i<line.Length-2; ++i)
            {
                if (line[i]==line[i+1]-1 && line[i]==line[i+2]-2) return true;
            }
            return false;
        }


        public static bool NoBads(string line)
        {
            foreach (var bad in bads)
            {
                if (line.Contains(bad)) return false;
            }

            return true;
        }

        public static bool HasTwoNonOverlappingPairs(string line)
        {
            int pairs = 0;
            for (int i=0; i<line.Length-1; ++i)
            {
                if (line[i]==line[i+1])
                {
                    pairs++;
                    i++;
                }
            }
            return pairs>1;
        }

        public static bool IsValid1(string line)
        {
            return HasStraight(line) && HasTwoNonOverlappingPairs(line) && NoBads(line);
        }

        public static string FindNextValid(string input)
        {          
            do
            {
                input = Increment(input);
            } while (!IsValid1(input));

            return input;
        }
 
        public static string Part1(string input)
        {
            return FindNextValid(input.Trim());
        }

        public static string Part2(string input)
        {
            return FindNextValid(FindNextValid(input.Trim()));
        }

        public void Run(string input, ILogger logger)
        {
            //logger.WriteLine(Increment("aaaah"));
            //logger.WriteLine(Increment("aaahz"));

            //logger.WriteLine(IsValid1("hijklmmn"));
            //logger.WriteLine(IsValid1("abbceffg"));
            //logger.WriteLine(IsValid1("abbcegjk"));
            //logger.WriteLine(IsValid1("abcdefgh"));
            //logger.WriteLine(IsValid1("abcdffaa"));
            //logger.WriteLine(IsValid1("abcdfffa"));
            //logger.WriteLine(IsValid1("ghijklmn"));
            //logger.WriteLine(IsValid1("ghjaabcc"));

            //logger.WriteLine(FindNextValid("abcdefgh"));
            //logger.WriteLine(FindNextValid("ghijklmn"));

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}