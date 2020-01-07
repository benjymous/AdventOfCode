using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVI
{
    public class Day03 : IPuzzle
    {
        public string Name { get { return "2016-03";} }
 
        public static bool TriangleValid(params int[] row)
        {
            if (row[0] + row[1] <= row[2] || row[0] + row[2] <= row[1] || row[1] + row[2] <= row[0])
            { 
                return false; 
            }
            else
            {
                return true;  
            }   
        }

        public static int Part1(string input)
        {
            var lines = Util.Split(input);
            var data = lines.Select(line => Util.Parse(line, ' ')); 

            var valid = data.Where(row => TriangleValid(row));

            return valid.Count();
        }

        public static int Part2(string input)
        {
            var numbers = Util.Parse(input.Replace("\n", " "), ' ');

            var triangles = new Queue<List<int>>();

            var count = 0;

            for (var i=0; i<3; ++i)
            {
                triangles.Enqueue(new List<int>());
            }

            foreach (var num in numbers)
            {
                var current = triangles.Dequeue();
                current.Add(num);

                if (current.Count == 3)
                {
                    count += TriangleValid(current.ToArray()) ? 1 : 0;
                    current.Clear();
                }
                triangles.Enqueue(current);
            }
            return count;
        }

        public void Run(string input, ILogger logger)
        {
            // logger.WriteLine(TriangleValid(5,5,5));
            // logger.WriteLine(TriangleValid(5,10,25));

            // logger.WriteLine(TriangleValid(7,10,5));
            // logger.WriteLine(TriangleValid(1,10,12));

            //logger.WriteLine(Part2("101 301 501\n102 302 502\n103 303 503\n201 401 601\n202 402 602\n203 403 603"));

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}