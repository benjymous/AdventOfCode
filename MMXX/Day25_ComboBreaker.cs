using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXX
{
    public class Day25 : IPuzzle
    {
        public string Name { get { return "2020-25";} }
 
        public static IEnumerable<(Int64 loop, Int64 val)> LoopVals(Int64 subject)
        {
            Int64 loop=1;
            Int64 val=1;

            while(true) 
            {
                val *= subject;
                val %= 20201227;
                yield return (loop++, val);
            }
        }

        public static Int64 Part1(string input)
        {
            var inputs = Util.Parse64(input);

            var loops = new Dictionary<Int64,Int64>();

            foreach (var dat in LoopVals(7))
            {
                //if (dat.loop%10000000 == 0) Console.WriteLine(dat.loop);
                foreach(var i in inputs)
                {
                    if (dat.val==i)
                    {
                        //Console.WriteLine($"{i} {dat.loop}");
                        //Console.WriteLine(loops.Count);
                        loops[i]=dat.loop;
                    }
                   
                }
                if (loops.Count==2) break;
            }

            var res = LoopVals(loops.First().Key).Where(v => v.loop == loops.Last().Value);

            return res.First().val;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - "+Part1(input));
        }
    }
}