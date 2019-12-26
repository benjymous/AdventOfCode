using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXV
{
    public class Day13 : IPuzzle
    {
        public string Name { get { return "2015-13";} }
 
        static int GetScore(char[] perm, Dictionary<int, int> scores)
        {
            int score = 0;
            for (int i=0; i<perm.Length; ++i)
            {
                int j = (i+1)%perm.Length;
                score += scores[GetKey(perm[i],perm[j])];
            }
            return score;
        }

        public static int GetKey(char p1, char p2) => (p1 << 16) + p2;

        public static int Solve(string input, bool includeYou = false)
        {
            var lines = Util.Split(input);

            HashSet<char> people = new HashSet<char>();

            Dictionary<int, int> scores = new Dictionary<int, int>();

            foreach (var line in lines)
            {
                var words = line.Split();
                var person1 = words.First()[0];
                var person2 = words.Last()[0];

                people.Add(person1);

                var factor = words[2];
                var score = int.Parse(words[3]);
                if (factor == "lose") score *= -1;

                scores.IncrementAtIndex(GetKey(person1, person2), score);
                scores.IncrementAtIndex(GetKey(person2, person1), score);
            }

            var starter = '?';
            if (includeYou) 
            {
                starter = 'Y';
                foreach (var other in people)
                {
                    scores[GetKey(starter,other)] = 0;
                    scores[GetKey(other,starter)] = 0;
                }          
            }
            else
            {
                starter = people.First();
                people.Remove(starter);  
            }

            var perms = people.Permutations();

            var additional = $"{starter}";
            return perms.AsParallel().Select(p => GetScore(additional.Union(p).ToArray(), scores)).Max();
        }

        public static int Part1(string input) => Solve(input);

        public static int Part2(string input) => Solve(input, true);

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}