using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXV
{
    public class Day19 : IPuzzle
    {
        public string Name { get { return "2015-19";} }
 
        public static int Part1(string input)
        {
            var lines = Util.Split(input);

            var rules = lines.Take(lines.Length-1).Select(x => x.Split(" => "));
            var molecule = lines.Last();

            HashSet<string> results = new HashSet<string>();

            foreach (var rule in rules)
            {
                var indices = molecule.AllIndexesOf(rule[0]);
                foreach (var i in indices)
                {
                    var newStr = molecule.ReplaceAtIndex(i, rule[0], rule[1]);
                    results.Add(newStr);
                }
            }

            return results.Count;
        }

        public static int Part2(string input)
        {
            var lines = Util.Split(input);

            var rules = lines.Take(lines.Length-1).Select(x => x.Split(" => "));
            var molecule = lines.Last();


            var jobqueue = new Queue<Tuple<string, int>>();
            jobqueue.Enqueue(Tuple.Create(molecule, 0));
            var cache = new Dictionary<string, int>();

            cache[molecule] = 0;

            int best = int.MaxValue;
            int shortest = int.MaxValue;

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();

                shortest = Math.Min(shortest, entry.Item1.Length);

                if (entry.Item1 == "e")
                {
                    best = Math.Min(best, entry.Item2);
                    continue;
                }

                foreach (var rule in rules.Where(r => entry.Item1.Contains(r[1])))
                {
                    int newScore = entry.Item2+1;

                    if (newScore > best) continue;


                    var indices = entry.Item1.AllIndexesOf(rule[1]);
                    foreach (var i in indices)
                    {
                        var newStr = entry.Item1.ReplaceAtIndex(i, rule[1], rule[0]);
                        
                        if (!cache.ContainsKey(newStr) || cache[newStr] > newScore)
                        {
                            cache[newStr] = newScore;
                            jobqueue.Enqueue(Tuple.Create(newStr, newScore));
                        }
                    }
                }
                jobqueue = new Queue<Tuple<string, int>>(jobqueue.OrderBy(x => x.Item1.Length));
                
            }

            return best;
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
            //console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}