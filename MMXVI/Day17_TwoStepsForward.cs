using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVI
{
    public class Day17 : IPuzzle
    {
        public string Name { get { return "2016-17";} }
 
        const string opens = "BCDEF";
        const string direction = "UDLR";

        static Direction2[] offsets = new Direction2[]
        {
            new Direction2(0,-1),
            new Direction2(0,1),
            new Direction2(-1,0),
            new Direction2(1,0)
        };

        public static IEnumerable<bool> Exits((string path, ManhattanVector2 position) state, string passcode)
        {
            var key = $"{passcode}{state.path}";
            bool left = state.position.X > 0;
            bool up = state.position.Y > 0;
            bool down = state.position.Y < 3;
            bool right = state.position.X < 3;
            var possible = new bool[] {up, down, left, right};
            var unlocked = key.GetMD5Chars().Take(4).Select(c => opens.Contains(c)).ToArray();
            for (int i=0; i<4; ++i)
            {
                yield return possible[i]&&unlocked[i];
            }
        }

        static string Key((string path, ManhattanVector2 position) state, IEnumerable<bool> exits)
        {
            return $"{state.position}-{string.Join(",",exits)}";
        }

        public static string Part1(string input)
        {
            var passcode = input.Trim();

            var jobqueue = new Queue<(string path, ManhattanVector2 position)>();
                   
            jobqueue.Enqueue(("", new ManhattanVector2(0,0)));
            var cache = new Dictionary<string, int>();

            int best = int.MaxValue;
            string path = "";

            var dest = new ManhattanVector2(3,3);

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();

                if (entry.position == dest)
                {
                    if (entry.path.Length < best)
                    {
                        best = Math.Min(best, entry.path.Length);
                        path = entry.path;
                    }
                }

                var exits = Exits(entry, passcode).ToArray();

                var key = Key(entry, exits);

                if (cache.TryGetValue(key, out var prev))
                {
                    if (prev < entry.path.Length) continue;
                }

                cache[key] = entry.path.Length;

                for (int i=0; i<4; ++i)
                {
                    if (exits[i])
                    {
                        jobqueue.Enqueue((entry.path+direction[i], new ManhattanVector2(entry.position.X + offsets[i].DX, entry.position.Y + offsets[i].DY)));
                    }
                }
               
            }

            return path;
        }

        public static int Part2(string input)
        {
            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            // var dirs = Exits("", "hijkl");
            // var dirs1 = Exits("D", "hijkl");
            // var dirs2 = Exits("DR", "hijkl");

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}