using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.Utils.Vectors;

namespace Advent.MMXVIII
{
    public class Day06 : IPuzzle
    {
        public string Name { get { return "2018-06";} }
 
        static List<ManhattanVector2> ParseData(string input)
        {
            //return input.Split("\n").Where(x => !string.IsNullOrWhiteSpace(x)).Select(line => new ManhattanVector2(line)).ToList();           
            return Util.Parse<ManhattanVector2>(input);
        }

        public static int Part1(string input)
        {
            var data = ParseData(input);

            var width = data.AsParallel().Select(pos => pos.X).Max();
            var height = data.AsParallel().Select(pos => pos.Y).Max();

            
            var grid = new List<List<int>>();

            for (var y=0; y<height; ++y)
            {
                var row = new List<int>();

                for (var x=0; x<width; ++x)
                {                
                    var distances = new List<int>();
                    var smallest = width*height;
                    var smallestIdx = -1;

                    for (var i=0; i<data.Count; ++i)
                    {
                        var entry = data[i];

                        distances.Add( entry.Distance(x,y) );
                        if (distances[i] < smallest)
                        {
                            smallest = distances[i];
                            smallestIdx = i;
                        }
                    }

                    var smallestCount = 0;
                    for (var i=0; i<data.Count; ++i)
                    {
                        if (distances[i]==smallest)
                        {
                            smallestCount++;
                        }
                    }

                    if (smallestCount == 1)
                    {
                        row.Add(smallestIdx);
                    }
                    else
                    {
                        row.Add(-1);
                    }
                }

                grid.Add(row);
            }

            var infinite = new Dictionary<int,bool>();
            for (var i=0; i<data.Count; ++i)
            {
                infinite[i] = false;
            }

            for (var x=0; x<width; ++x)
            {
                infinite[grid[0][x]] = true;
                infinite[grid[height-1][x]] = true;
            }

            for (var y=0; y<height; ++y)
            {
                infinite[grid[y][0]] = true;
                infinite[grid[y][width-1]] = true;
            }


            
            var maxArea = 0;
            foreach (var i in infinite.Keys)
            {
                var count = 0;                

                if (!infinite[i])
                {
                    for (var y=0; y<height; ++y)
                    {
                        for (var x=0; x<width; ++x)
                        {
                            if (grid[y][x]==i)
                            {
                                count++;
                            }
                        }
                    }
                }

                maxArea = Math.Max(maxArea, count);
            }

            return maxArea;
        }

        public static int Part2(string input, int safeDistance)
        {
            var data = ParseData(input);

            var width = data.AsParallel().Select(pos => pos.X).Max();
            var height = data.AsParallel().Select(pos => pos.Y).Max();

            return ParallelEnumerable.Range(0,height).Select( 
                y => ParallelEnumerable.Range(0,width).Where(
                    x => data.Select(e => e.Distance(x,y)).Sum() < safeDistance
                ).Count()
            ).Sum();
        }

        public static int Part2(string input) => Part2(input, 10000);

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}