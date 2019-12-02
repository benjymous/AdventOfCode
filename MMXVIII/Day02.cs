using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day02 : IPuzzle
    {
        public string Name { get { return "2018-02";} }
 
        public void Part1(string[] keys)
        {
            int doubles = 0;
            int triples = 0;

            foreach(var id in keys)
            {
                var chars = new Dictionary<char, int>();
                for (var i=0; i<id.Length; ++i)
                {
                    var code = id[i];
                    if (chars.ContainsKey(code))
                    {
                        chars[code]++;
                    }
                    else
                    {
                        chars[code]=1;
                    } 
                }

                bool hasDouble = chars.Any(kvp => kvp.Value == 2);
                bool hasTriple = chars.Any(kvp => kvp.Value == 3);

                if (hasDouble) doubles++;
	            if (hasTriple) triples++;
            }


            Console.WriteLine("* Pt1 - Doubles: "+ doubles+ " Triples: "+ triples+ " Checksum:" + doubles*triples);
        }

        public void Part2(string[] keys)
        {
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
                        Console.WriteLine("* Pt2 - "+answer);
                        return;
                    }

                }
            }
        }

        public void Run(string input)
        {
            var keys = Util.Split(input);

            Part1(keys);
            Part2(keys);
        }
    }
}
