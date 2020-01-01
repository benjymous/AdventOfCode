using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXV
{
    public class Day15 : IPuzzle
    {
        public string Name { get { return "2015-15";} }

        class Ingredient
        {
            public string Name {get; private set;}
            public Dictionary<string,int> Qualities {get; private set;} = new Dictionary<string, int>();
            public Ingredient(string line)
            {
                var bits = line.Split(":");
                Name = bits[0];

                var parts = Util.Split(bits[1]);
                foreach (var part in parts)
                {
                   var bits2 = part.Trim().Split(" ");
                   Qualities[bits2[0]] = int.Parse(bits2[1]); 
                }
            }

            public override string ToString() => $"{Name}: {string.Join(", ",Qualities.Select(kvp => $"{kvp.Key} {kvp.Value}"))}";
        }

        static IEnumerable<string> Qualities()
        {
            yield return "capacity";
            yield return "durability";
            yield return "flavor";
            yield return "texture";
        }

        static int CalcScore(int[] weights, List<Ingredient> ingredients)
        {
            int score=1;

            foreach (var q in Qualities())
            {
                int qualScore = 0;
                for (int i=0; i<weights.Length; ++i)
                {
                    qualScore += ingredients[i].Qualities[q] * weights[i];
                }
                qualScore = Math.Max(0, qualScore);
                score *= qualScore;
            }

            return score;
        }

        static int CalcCalories(int[] weights, List<Ingredient> ingredients)
        {
            int calories = 0;

            for (int i=0; i<weights.Length; ++i)
            {
                calories += ingredients[i].Qualities["calories"] * weights[i];
            }

            return calories;
        }

        static IEnumerable<int[]> Neighbours(int[] weights)
        {
            if (weights[0]>0)
            {
                yield return new int[]{weights[0]-1, weights[1]+1, weights[2], weights[3]};
                yield return new int[]{weights[0]-1, weights[1], weights[2]+1, weights[3]};
                yield return new int[]{weights[0]-1, weights[1], weights[2], weights[3]+1};
            }

            if (weights[1]>0)
            {
                yield return new int[]{weights[0]+1, weights[1]-1, weights[2], weights[3]};
                yield return new int[]{weights[0], weights[1]-1, weights[2]+1, weights[3]};
                yield return new int[]{weights[0], weights[1]-1, weights[2], weights[3]+1};
            }

            
            if (weights[2]>0)
            {
                yield return new int[]{weights[0]+1, weights[1], weights[2]-1, weights[3]};
                yield return new int[]{weights[0], weights[1]+1, weights[2]-1, weights[3]};
                yield return new int[]{weights[0], weights[1], weights[2]-1, weights[3]+1};
            }

            
            if (weights[3]>0)
            {
                yield return new int[]{weights[0]+1, weights[1], weights[2], weights[3]-1};
                yield return new int[]{weights[0], weights[1]+1, weights[2], weights[3]-1};
                yield return new int[]{weights[0], weights[1], weights[2]+1, weights[3]-1};
            }
        }
 
        public static int Solve(string input, bool countCalories)
        {
            var ingredients = Util.Parse<Ingredient>(input);
            
            var jobqueue = new Queue<Tuple<int[], int>>();
            var initial = new int[4]{25,25,25,25};
            var initialCalories = countCalories ? CalcCalories(initial, ingredients) : 500;
            var initialScore = initialCalories == 500 ? CalcScore(initial, ingredients) : 0;
            
            jobqueue.Enqueue(Tuple.Create(initial, initialScore));
            var cache = new Dictionary<string, int>();

            int best = initialScore;

            cache[string.Join(",",initial)] = initialScore;

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();

                if (entry.Item2 > best) best = entry.Item2;

                var neighbours = Neighbours(entry.Item1);

                foreach (var neighbour in neighbours)
                {
                    var key = string.Join(",", neighbour);

                    if (!cache.ContainsKey(key))
                    {
                        int calories = countCalories ? CalcCalories(neighbour, ingredients) : 500;
                        int newScore = calories == 500 ? CalcScore(neighbour, ingredients) : 0;
                    
                        cache[key] = newScore;
                        jobqueue.Enqueue(Tuple.Create(neighbour, newScore));
                       
                    }
                }
               
            }

            return best;
        }

        public static int Part1(string input)
        {
            return Solve(input, false);
        }

        public static int Part2(string input)
        {
            return Solve(input, true);
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}