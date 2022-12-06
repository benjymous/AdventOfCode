using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day24 : IPuzzle
    {
        public string Name => "2017-24";

        public struct Component
        {
            [Regex(@"(.+)\/(.+)")]
            public Component(int x, int y)
            {
                val1 = Math.Min(x, y);
                val2 = Math.Max(x, y);
            }
            public int val1;
            public int val2;

            public bool IsDifferent(Component other)
            {
                return other.val1 != val1 || other.val2 != val2;
            }

            public bool Has(int val)
            {
                return val1 == val || val2 == val;
            }

            public int Other(int a)
            {
                if (val1 == a) return val2; else return val1;
            }

            public int Strength()
            {
                return val1 + val2;
            }
        }

        public static IEnumerable<IEnumerable<Component>> GetChains(int currentPort, IEnumerable<Component> inputs)
        {
            var first = inputs.Where(c => c.Has(currentPort));

            foreach (var component in first)
            {
                yield return new Component[] { component };
                var rest = inputs.Where(c => c.IsDifferent(component)).ToArray();

                var childResults = GetChains(component.Other(currentPort), rest);
                foreach (var item in childResults)
                {
                    yield return item.Prepend(component).ToArray();
                }
            }
        }

        public static int Part1(string input)
        {
            var data = Util.RegexParse<Component>(input);
            var chains = GetChains(0, data).ToArray();

            return Part1(chains);
        }

        public static int Part1(IEnumerable<IEnumerable<Component>> chains)
        {
            return chains.Max(chain => chain.Sum(comp => comp.Strength()));
        }

        public static int Part2(string input)
        {
            var data = Util.RegexParse<Component>(input);
            var chains = GetChains(0, data).ToArray();

            return Part2(chains);
        }

        public static int Part2(IEnumerable<IEnumerable<Component>> chains)
        {
            var groups = chains.GroupBy(chain => chain.Count());

            var longest = groups.Max(x => x.Key);

            var longestGroup = groups.Where(x => x.Key == longest).SelectMany(x => x);

            var longestStrongest = longestGroup.Max(chain => chain.Sum(component => component.Strength()));

            return longestStrongest;
        }

        public void Run(string input, ILogger logger)
        {
            //Console.WriteLine(Part1("0/2\n2/2\n2/3\n3/4\n3/5\n0/1\n10/1\n9/10"));
            //Console.WriteLine(Part2("0/2\n2/2\n2/3\n3/4\n3/5\n0/1\n10/1\n9/10"));

            var data = Util.Parse<Component>(input);
            var chains = GetChains(0, data).ToList();

            logger.WriteLine("- Pt1 - " + Part1(chains));
            logger.WriteLine("- Pt2 - " + Part2(chains));
        }
    }
}