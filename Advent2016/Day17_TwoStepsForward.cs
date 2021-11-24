using AoC.Utils;
using AoC.Utils.Vectors;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2016
{
    public class Day17 : IPuzzle
    {
        public string Name { get { return "2016-17"; } }

        const string opens = "BCDEF";
        const string direction = "UDLR";

        static Direction2[] offsets = new Direction2[]
        {
            new Direction2(0,-1),
            new Direction2(0,1),
            new Direction2(-1,0),
            new Direction2(1,0)
        };

        struct AvailableDoors
        {
            public AvailableDoors(bool[] d)
            {
                directions = d;
            }
            public bool[] directions;
        }

        static AvailableDoors[,] availableDoors = GetAvailableDoors();

        static AvailableDoors[,] GetAvailableDoors()
        {
            AvailableDoors[,] result = new AvailableDoors[4, 4];
            for (int x = 0; x < 4; ++x)
            {
                for (int y = 0; y < 4; ++y)
                {
                    bool up = y > 0;
                    bool down = y < 3;
                    bool left = x > 0;
                    bool right = x < 3;
                    result[x, y] = new AvailableDoors(new bool[] { up, down, left, right });
                }
            }
            return result;
        }

        public static IEnumerable<int> Exits((string path, ManhattanVector2 position) state, string passcode)
        {
            var key = $"{passcode}{state.path}";
            var possible = availableDoors[state.position.X, state.position.Y].directions;
            var unlocked = key.GetMD5Chars().Take(4).Select(c => opens.Contains(c)).ToArray();
            for (int i = 0; i < 4; ++i)
            {
                if (possible[i] && unlocked[i]) yield return i;
            }
        }

        static string Key((string path, ManhattanVector2 position) state, IEnumerable<int> exits) => $"{state.position}-{string.Join(",", exits)}";

        public static string Part1(string input)
        {
            var passcode = input.Trim();

            var jobqueue = new Queue<(string path, ManhattanVector2 position)>();

            jobqueue.Enqueue(("", new ManhattanVector2(0, 0)));
            var cache = new Dictionary<string, int>();

            int best = int.MaxValue;
            string path = "";

            var dest = new ManhattanVector2(3, 3);

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();

                if (entry.position == dest)
                {
                    if (entry.path.Length < best)
                    {
                        best = entry.path.Length;
                        path = entry.path;
                    }
                    continue; // don't continue if the path has reached the vault
                }

                var exits = Exits(entry, passcode);

                var key = Key(entry, exits);

                if (cache.TryGetValue(key, out var prev))
                {
                    if (prev < entry.path.Length) continue;
                }

                cache[key] = entry.path.Length;

                foreach (var i in exits)
                {
                    jobqueue.Enqueue((entry.path + direction[i], new ManhattanVector2(entry.position.X + offsets[i].DX, entry.position.Y + offsets[i].DY)));
                }
            }

            return path;
        }

        public static int Part2(string input)
        {
            var passcode = input.Trim();

            var jobqueue = new Queue<(string path, ManhattanVector2 position)>();

            jobqueue.Enqueue(("", new ManhattanVector2(0, 0)));

            int best = int.MinValue;
            string path = "";

            var dest = new ManhattanVector2(3, 3);

            while (jobqueue.Any())
            {
                var entry = jobqueue.Dequeue();

                if (entry.position == dest)
                {
                    if (entry.path.Length > best)
                    {
                        best = entry.path.Length;
                        path = entry.path;
                    }
                    continue;  // don't continue if the path has reached the vault
                }

                var exits = Exits(entry, passcode);
                foreach (var i in exits)
                {
                    jobqueue.Enqueue((entry.path + direction[i], new ManhattanVector2(entry.position.X + offsets[i].DX, entry.position.Y + offsets[i].DY)));
                }
            }

            return best;
        }

        public void Run(string input, ILogger logger)
        {
            // var dirs = Exits("", "hijkl");
            // var dirs1 = Exits("D", "hijkl");
            // var dirs2 = Exits("DR", "hijkl");

            //logger.WriteLine("??"+Part2("ihgpwlah"));

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}