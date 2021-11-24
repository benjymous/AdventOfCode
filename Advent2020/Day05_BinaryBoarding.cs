using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2020
{
    public class Day05 : IPuzzle
    {
        public string Name { get { return "2020-05";} }
 
        public class BinSearch
        {
            public BinSearch(int min, int max)
            {
                Min=min;
                Max=max;
            }

            public int Min;
            public int Max;

            int Range { get { return Max-Min+1;} }

            public void Lower() 
            {
                var split = Math.Max(1,Range/2);
                Max -= split;
            }

            public void Upper() 
            {
                var split = Math.Max(1,Range/2);
                Min += split;
            }
        }
        public class BoardingPass
        {
            public BoardingPass(string id)
            {
                var row = new BinSearch(0,127);
                var col = new BinSearch(0,7);
                foreach(char c in id)
                {
                    switch(c)
                    {
                        case 'F':
                            row.Lower();
                            break;
                        case 'B':
                            row.Upper();
                            break;

                        case 'L':
                            col.Lower();
                            break;

                        case 'R':
                            col.Upper();
                            break;
                    }

                    //Console.WriteLine($"{row.Min} {row.Max} {col.Min} {col.Max}");
                }

                Row=row.Min;
                Col=col.Min;
                ID=(Row*8)+Col;
            }

            int Row;
            int Col;
            public int ID;
        }

        public static int Part1(string input)
        {
            var passes = Util.Parse<BoardingPass>(input);
            return passes.Select(p => p.ID).Max();
        }

        public static int Part2(string input)
        {
            var passes = Util.Parse<BoardingPass>(input);
            var ids = passes.Select(p => p.ID).OrderBy(p => p).ToArray();

            for (int i=0; i<ids.Length-1; ++i)
            {
                if (ids[i+1]-2 == ids[i]) {
                    return ids[i]+1;
                }
            }

            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}