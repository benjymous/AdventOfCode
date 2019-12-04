using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day02 : IPuzzle
    {
        public string Name { get { return "2018-02";} }
 
        public static int Part1(string input)
        {
            var keys = Util.Split(input);

            int doubles = 0;
            int triples = 0;

            foreach(var id in keys)
            {
                var chars = new Dictionary<char, int>();
                for (var i=0; i<id.Length; ++i)
                {
                    chars.IncrementAtIndex(id[i]);
                }

                bool hasDouble = chars.Any(kvp => kvp.Value == 2);
                bool hasTriple = chars.Any(kvp => kvp.Value == 3);

                if (hasDouble) doubles++;
	            if (hasTriple) triples++;
            }

            return doubles*triples;
        }

        public static string Part2(string input)
        {
            var keys = Util.Split(input);
            foreach (var s1 in keys)
            {                
                foreach (var s2 in keys)
                {
                    var diff = 0;
                    var answer = "";

                    for (int i=0; i<s1.Length; ++i)
                    {
                        if (s1[i]!=s2[i])
                        {
                            diff++;
                        }
                        else {
                            answer += s1[i];
                        }
                    }

                    if (diff == 1) 
                    {
                        return answer;
                    }               
                }
            }
            return "FAIL";

        }

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - "+Part1(input));
            Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
