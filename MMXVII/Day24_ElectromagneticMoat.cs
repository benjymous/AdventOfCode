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

            public bool Has(int val)
            {
                return val1==val || val2==val;
            }

            public bool CanLink(Component other)
            {
                return other.Has(val1) || other.Has(val2);
            }

            public int Strength()
            {
                return val1+val2;
            }
        }

        public static IEnumerable<Component> ReduceValid(IEnumerable<Component> data)
        {
            List<Component> output = new List<Component>();
            var input = data.ToList();

            if (!input[0].Has(0)) return null;

            output.Add(input[0]);

            for (int i=0; i<input.Count-1; ++i)
            {
                if (input[i].CanLink(input[i+1]))
                {
                    output.Add(input[i+1]);
                }
                else
                {
                    return output;
                }
            }

            return output;
        }
 
        public static int Part1(string input)
        {
            // var data = Util.Parse<Component>(input);

            // var perms = Permutations.Get(data).AsParallel()
            //             .Select(perm => ReduceValid(perm))
            //             .Where(perm => perm != null);

            // return perms.Count();
            return 0;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - "+Part1(input));
            Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}