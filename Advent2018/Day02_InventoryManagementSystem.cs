using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day02 : IPuzzle
    {
        public string Name => "2018-02";

        public static int Part1(string input)
        {
            var keys = Util.Split(input);

            int doubles = 0;
            int triples = 0;

            foreach(var id in keys)
            {
                var grp = id.GroupBy(c => c);
                doubles += grp.Any(g => g.Count() == 2) ? 1 : 0;
                triples += grp.Any(g => g.Count() == 3) ? 1 : 0;
            }

            return doubles * triples;
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

                    for (int i = 0; i < s1.Length; ++i)
                    {
                        if (s1[i] != s2[i])
                        {
                            diff++;
                        }
                        else
                        {
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

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
