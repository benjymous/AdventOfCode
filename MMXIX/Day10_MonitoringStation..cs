using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    public class Day10 : IPuzzle
    {
        public string Name { get { return "2019-10";} }
 
        static List<ManhattanVector2> Parse(string input)
        {
            var lines = Util.Split(input);

            var result = new List<ManhattanVector2>();

            for (var y=0; y<lines.Count(); ++y)
            {
                var line = lines[y];
                for (var x=0; x < line.Length; ++x)
                {
                    if (line[x]=='#')
                    {
                        result.Add(new ManhattanVector2(x,y));
                    }
                }
            }

            return result;
        }

        // public static double AngleBetween(ManhattanVector2 vector1, ManhattanVector2 vector2)
        // {
        //     double sin = vector1.X * vector2.Y - vector2.X * vector1.Y;  
        //     double cos = vector1.X * vector2.X + vector1.Y * vector2.Y;

        //     return Math.Atan2(sin, cos) * (180 / Math.PI);            
        // }

        static int GCD(int[] numbers)
        {
            return numbers.Aggregate(GCD);
        }

        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        public static ManhattanVector2 Normalize(ManhattanVector2 input)
        {
            var gcd = GCD(Math.Abs(input.X), Math.Abs(input.Y));

            if (gcd == 0) return input;

            return new ManhattanVector2(input.X / gcd, input.Y / gcd);
        }

        public static ManhattanVector2 AngleBetween(ManhattanVector2 vector1, ManhattanVector2 vector2)
        {
            var t = new ManhattanVector2((vector1.X-vector2.X),(vector1.Y-vector2.Y));

            var n = Normalize(t);

            return n;
        }

        public static int Part1(string input)
        {
            var data = Parse(input);

            var grouped = data.Select(a1 => data.Select(a2 => new Tuple<ManhattanVector2, ManhattanVector2>(AngleBetween(a1,a2), a2)).GroupBy(tup => tup.Item1, tup => tup.Item2));

            var counts = grouped.Select(g1 => g1.Count()).Max();

            // foreach (var group in grouped)
            // {
            //     int i = grouped.IndexOf(group);
            //     var a1 = input[i];

            //     foreach (var grouping in group)
            //     {
            //         foreach (var a2 in grouping)
            //         {

            //         }
            //     }
            // }

            return counts-1;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, System.IO.TextWriter console)
        {           

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

            console.WriteLine("- Pt1 - "+Part1(input));
            //console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}