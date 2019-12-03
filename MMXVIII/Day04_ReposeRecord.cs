using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVIII
{
    public class Day04 : IPuzzle
    {
        public string Name { get { return "2018-04";} }
 
        struct Data
        {
            public Dictionary<string, Dictionary<int,int>> guards;
            public Dictionary<string, int> durations;
        }

        static Data Parse(string input)
        {
            string id = null;
            string sleep = null;

            Data d = new Data();
            d.guards = new Dictionary<string, Dictionary<int,int>>();
            d.durations = new Dictionary<string, int>();

            var lines = Util.Split(input).OrderBy(x => x);

            foreach (var l in lines)
            {
                var line = l.Replace("[","").Replace("]","").Replace(":","");
                var bits = line.Split(" ");
                if (line.Contains("Guard")) 
                {
                    id = bits[3];

                    if (!d.guards.ContainsKey(id)) 
                    {
                        d.guards[id] = new Dictionary<int, int>();
                        d.durations[id] = 0;
                    }
                }
                else if (line.Contains("asleep")) 
                {
                    sleep = bits[1];
                }
                else if (line.Contains("wakes")) 
                {      
                    var duration = int.Parse(bits[1])-int.Parse(sleep); 
                    d.durations[id] += duration; 

                    for (var m=int.Parse(sleep); m<int.Parse(bits[1]); ++m) 
                    {
                        d.guards[id].IncrementAtIndex(m);
                    }
                }
            }
            return d;
        }

        public static int Part1(string input)
        {
            var data = Parse(input);

            string sleepiest = null;
            var v = 0;

            foreach (var id in data.durations.Keys) 
            {
                if (data.durations[id] > v) 
                {
                    v = data.durations[id];
                    sleepiest = id;
                }
            }

            var m = -1;
            var mv = -1;

            foreach (var min in data.guards[sleepiest].Keys)
            {
                if (data.guards[sleepiest][min] > mv) {
                    mv = data.guards[sleepiest][min];
                    m = min;
                }
            }

            return int.Parse(sleepiest.Replace("#","")) * m;
        }

        public static int Part2(string input)
        {
            var data = Parse(input);

            var m = -1;
            var mv = -1;
            string sleepiest = null;

            foreach (var id in data.guards.Keys)
            {
                foreach (var min in data.guards[id].Keys)
                {
                    if (data.guards[id][min] > mv) 
                    {
                        mv = data.guards[id][min];
                        m = min;
                        sleepiest = id;
                    }
                }
            }

            return int.Parse(sleepiest.Replace("#","")) * m;
        }

        public void Run(string input)
        {
            Console.WriteLine("- Pt1 - "+Part1(input));
            Console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}
