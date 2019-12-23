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

            var rules = lines.Take(lines.Length-1).Select(x => x.Split(" => ")).OrderByDescending(x => x[1].Length);
            var molecule = lines.Last();


            var jobqueue = new LinkedList<Tuple<string, int>>();
            jobqueue.AddFirst(Tuple.Create(molecule, 0));
            var cache = new Dictionary<string, int>();

            cache[molecule] = 0;

            int best = int.MaxValue;
            int shortest = int.MaxValue;

            int step = 0;
            int skip = 0;

            while (jobqueue.Any())
            {
                var entry = jobqueue.First();  jobqueue.RemoveFirst();
                step++;

                if (step%1000 == 0)
                {
                    Console.WriteLine($"{step} - {shortest} - {jobqueue.Count} - {cache.Count} {skip}");
                }

                if (entry.Item1.Length < shortest)
                {
                    Console.WriteLine($"{entry.Item1} {entry.Item1.Length}");
                }
                shortest = Math.Min(shortest, entry.Item1.Length);

                if (entry.Item1 == "e")
                {
                    best = Math.Min(best, entry.Item2);
                    continue;
                }

                foreach (var rule in rules.Where(r => entry.Item1.Contains(r[1])))
                {
                    int newScore = entry.Item2+1;

                    if (newScore > best) 
                    {
                        skip++;
                        continue;
                    }


                    var indices = entry.Item1.AllIndexesOf(rule[1]);

                    var replacements = indices.AsParallel().Select(i => entry.Item1.ReplaceAtIndex(i, rule[1], rule[0]));

                    foreach (var newStr in replacements)
                    {
                        if (!cache.ContainsKey(newStr) || cache[newStr] > newScore)
                        {
                            cache[newStr] = newScore;
                            var newitem = Tuple.Create(newStr, newScore);
                            jobqueue.AddFirst(newitem);
                        }
                        else
                        {
                            skip++;
                        }
                    }

                    // foreach (var i in indices)
                    // {
                    //     var newStr = entry.Item1.ReplaceAtIndex(i, rule[1], rule[0]);
                        
                    //     if (!cache.ContainsKey(newStr) || cache[newStr] > newScore)
                    //     {
                    //         cache[newStr] = newScore;
                    //         var newitem = Tuple.Create(newStr, newScore);
                    //         jobqueue.AddFirst(newitem);
                    //     }
                    //     else
                    //     {
                    //         skip++;
                    //     }
                    // }
                }
                //jobqueue = new Queue<Tuple<string, int>>(jobqueue.OrderBy(x => x.Item1.Length));
                
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