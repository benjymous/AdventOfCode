using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVII
{
    public class Day24 : IPuzzle
    {
        public string Name { get { return "2017-24";} }

        public struct Component
        {
            public Component(string input)
            {
                var bits = input.Split('/');
                int x = int.Parse(bits[0]);
                int y = int.Parse(bits[1]);

                val1 = Math.Min(x,y);
                val2 = Math.Max(x,y);
            }
            public int val1;
            public int val2;

            public bool isDifferent(Component other)
            {
                return other.val1 != val1 || other.val2 != val2;
            }

            public bool Has(int val)
            {
                return val1==val || val2==val;
            }

            public int Other(int a)
            {
                if (val1==a) return val2; else return val1;
            }

            public int Strength()
            {
                return val1+val2;
            }
        }

        public static IEnumerable<IEnumerable<Component>> GetChains(int currentPort, IEnumerable<Component> inputs)
        {
            var first = inputs.Where(c => c.Has(currentPort));

            foreach (var component in first)
            {
                yield return new List<Component>{component};
                var rest = inputs.Where(c => c.isDifferent(component));

                var childResults = GetChains(component.Other(currentPort), rest); 
                foreach (var item in childResults)
                {
                    List<Component> newList = new List<Component>();
                    newList.Add(component);
                    newList.AddRange(item);
                    yield return newList;
                } 
            }
        }

        public static int Part1(string input)
        {
            var data = Util.Parse<Component>(input);
            var chains = GetChains(0, data);

            return Part1(chains);
        }
 
        public static int Part1(IEnumerable<IEnumerable<Component>> chains)
        {
            return chains.AsParallel().Select(chain => chain.Select(comp => comp.Strength()).Sum()).Max();
        }

        public static int Part2(string input)
        {
            var data = Util.Parse<Component>(input);
            var chains = GetChains(0, data);

            return Part2(chains);
        }

        public static int Part2(IEnumerable<IEnumerable<Component>> chains)
        {
            var groups = chains.GroupBy(chain => chain.Count());

            var longest = groups.Select(x => x.Key).Max();

            var longestGroup = groups.Where(x => x.Key == longest).SelectMany(x=>x);

            var longestStrongest = longestGroup.Select(chain => chain.Select(component => component.Strength()).Sum()).Max();

            return longestStrongest;
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            //Console.WriteLine(Part1("0/2\n2/2\n2/3\n3/4\n3/5\n0/1\n10/1\n9/10"));
            //Console.WriteLine(Part2("0/2\n2/2\n2/3\n3/4\n3/5\n0/1\n10/1\n9/10"));

            var data = Util.Parse<Component>(input);
            var chains = GetChains(0, data).ToList();

            console.WriteLine("- Pt1 - "+Part1(chains));
            console.WriteLine("- Pt2 - "+Part2(chains));
        }
    }
}