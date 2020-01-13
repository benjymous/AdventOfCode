using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.Utils.Vectors;

namespace Advent.MMXIX
{
    public class Day10 : IPuzzle
    {
        public string Name { get { return "2019-10";} }
 
        static List<ManhattanVector2> Parse32(string input)
        {
            var lines = Util.Split(input);

            var result = new List<ManhattanVector2>();

            for (var y=0; y<lines.Count(); ++y)
            {
                var line = lines[y];
                for (var x=0; x < line.Length; ++x)
                {
                    if (line[x]!='.')
                    {
                        result.Add(new ManhattanVector2(x,y));
                    }
                }
            }

            return result;
        }

        public static double AngleBetween(ManhattanVector2 vector1, ManhattanVector2 vector2)
        {
            var angle = Math.Atan2(vector2.X - vector1.X, vector1.Y - vector2.Y) * (180 / Math.PI);
            if (angle<0) angle += 360;
            return angle;            
        }

        // static int GCD(int[] numbers)
        // {
        //     return numbers.Aggregate(GCD);
        // }

        // static int GCD(int a, int b)
        // {
        //     return b == 0 ? a : GCD(b, a % b);
        // }

        // public static ManhattanVector2 IntNormalize(ManhattanVector2 input)
        // {
        //     var gcd = GCD(Math.Abs(input.X), Math.Abs(input.Y));

        //     if (gcd == 0) return input;

        //     return new ManhattanVector2(input.X / gcd, input.Y / gcd);
        // }

        // public static ManhattanVector2 ManhattanAngleBetween(ManhattanVector2 vector1, ManhattanVector2 vector2)
        // {
        //     var t = new ManhattanVector2((vector1.X-vector2.X),(vector1.Y-vector2.Y));

        //     var n = IntNormalize(t);

        //     return n;
        // }

        public static IEnumerable<IEnumerable<IGrouping<double, Tuple<ManhattanVector2,ManhattanVector2>>>> BuildGroups(string input)
        {
            var data = Parse32(input);

            var groups = data.Select(a1 => data.Select(a2 => Tuple.Create(AngleBetween(a1,a2), Tuple.Create(a1,a2))).GroupBy(tup => tup.Item1, tup => tup.Item2));

            return groups;
        }

        public static int Part1(string input)
        {
            var grouped = BuildGroups(input);

            var counts = grouped.Select(group => group.Count()).Max();

            return counts; 
        }

        public static int Part2(string input, int position)
        {
            var order = TargetOrder(input);
            var pos = order.ToList()[position-1];
            return pos.X*100 + pos.Y;
        }

        public static List<ManhattanVector2> TargetOrder(string input)
        {
            var data = Parse32(input);

            var grouped = BuildGroups(input);

            var counts = grouped.OrderBy(group => group.Count()).Reverse();

            var best = counts.First().OrderBy(item => item.Key);

            // first result will be the group with the most visible (i.e. the part1 answer)
            var result = counts.First();

            // Extract the position from this (ick!)
            var bestPosition = result.First().First().Item1;

            var targets = result.Select(x => x.Select(y => Tuple.Create(x.Key, y.Item2)));

            // now sort into order
            var order = new List<Tuple<double, int, ManhattanVector2>>();
            foreach (var x in targets)
            {
                foreach (var y in x)
                {
                    order.Add(Tuple.Create(y.Item1, y.Item2.Distance(bestPosition), y.Item2));
                }
            }

            var sorted = order.OrderBy(x => x.Item1).ThenBy(y => y.Item2)
                .Skip(1); // ignore first element, as the base can't target itself
                
            var queue = new Queue<Tuple<double, int, ManhattanVector2>>(sorted);

            double currentAngle = 0;

            List<ManhattanVector2> destroyed = new List<ManhattanVector2>();
            
            while (queue.Any())
            {
                var target = queue.Dequeue();
                if (target.Item1 >= currentAngle || !queue.Any())
                {
                    //Console.WriteLine($"[{++steps}] Destroyed {target.Item3} [{currentAngle}]");   
                    destroyed.Add(target.Item3);                          
                }
                else
                {
                    queue.Enqueue(target);
                }  
                currentAngle = target.Item1 + 0.00001;                
                if (currentAngle > 360) currentAngle -= 360;
            }

            return destroyed;
        }

        public void Run(string input, ILogger logger)
        {        

            //Console.WriteLine(AngleBetween(new ManhattanVector2(8,3), new ManhattanVector2(8,1)));   

            // Console.WriteLine(AngleBetween(new ManhattanVector2(5,5), new ManhattanVector2(10,5)));
            // Console.WriteLine(AngleBetween(new ManhattanVector2(5,5), new ManhattanVector2(0,5)));

            // Console.WriteLine(AngleBetween(new ManhattanVector2(5,5), new ManhattanVector2(5,0)));
            // Console.WriteLine(AngleBetween(new ManhattanVector2(5,5), new ManhattanVector2(10,0)));

            // Console.WriteLine(AngleBetween(new ManhattanVector2(0,0), new ManhattanVector2(1,1)));
            // Console.WriteLine(AngleBetween(new ManhattanVector2(0,0), new ManhattanVector2(2,2)));
            // Console.WriteLine(AngleBetween(new ManhattanVector2(0,0), new ManhattanVector2(3,3)));

            // Console.WriteLine(AngleBetween(new ManhattanVector2(1,1), new ManhattanVector2(0,0)));

            // Console.WriteLine(AngleBetween(new ManhattanVector2(0,1), new ManhattanVector2(1,0)));
            // Console.WriteLine(AngleBetween(new ManhattanVector2(1,0), new ManhattanVector2(0,1)));

            // Console.WriteLine("8? " + Part1(".#..#,.....,#####,....#,...##"));

            // Console.WriteLine("33? " + Part1("......#.#.,#..#.#....,..#######.,.#.#.###..,.#..#.....,..#....#.#,#..#....#.,.##.#..###,##...#..#.,.#....####"));

            // Console.WriteLine("35? " + Part1("#.#...#.#.,.###....#.,.#....#...,##.#.#.#.#,....#.#.#.,.##..###.#,..#...##..,..##....##,......#...,.####.###."));

            // Console.WriteLine("41? " + Part1(".#..#..###,####.###.#,....###.#.,..###.##.#,##.##.#.#.,....###..#,..#.#..#.#,#..#.#.###,,.##...##.#,.....#.#.."));

            // Console.WriteLine("210? " + Part1(".#..##.###...#######,##.############..##.,.#.######.########.#,.###.#######.####.#.,#####.##.#.##.###.##,..#####..#.#########,####################,#.####....###.#.#.##,##.#################,#####.##.###..####..,..######..##.#######,####.##.####...##..#,.#####..#.######.###,##...#.##########...,#.##########.#######,.####.#.###.###.#.##,....##.##.###..#####,.#.#.###########.###,#.#.#.#####.####.###,###.##.####.##.#..##"));


            //Console.WriteLine(Part2(".#..#,.....,#####,....#,...##"));
            //Console.WriteLine(Part2(".#....#####...#..,##...##.#####..##,##...#...#.#####.,..#.....X...###..,..#.#.....#....##"));

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input, 200));
        }
    }
}