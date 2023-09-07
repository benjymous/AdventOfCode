using AoC.Utils;
using System.Collections.Generic;

namespace AoC.Advent2020
{
    public class Day15 : IPuzzle
    {
        public string Name => "2020-15";

        class Entry
        {
            public Entry(int v) => Set(v);
            public void Set(int v) => (v1, v2) = (v2, v);

            public bool IsFirst => v1 == -1;
            public int Get => v1;

            int v1 = -1;
            int v2 = -1;
        }

        class Storage
        {
            public Storage(string input)
            {
                Util.ParseNumbers<int>(input).ForEach(AddNumber);
            }

            public void Next() => AddNumber(LastEntry.IsFirst ? 0 : Count - LastEntry.Get);

            void AddNumber(int val)
            {
                LastNum = val;
                if (values.TryGetValue(LastNum, out Entry entry))
                {
                    LastEntry = entry;
                    entry.Set(++Count);
                }
                else
                {
                    LastEntry = new Entry(++Count);
                    values[val] = LastEntry;
                }
            }

            public int Count { get; private set; } = 0;
            public int LastNum { get; private set; } = 0;

            readonly Dictionary<int, Entry> values = new();
            Entry LastEntry = null;
        }


        public static int Solve(string input, int count)
        {
            var data = new Storage(input);

            while (data.Count < count)
            {
                data.Next();
            }
            return data.LastNum;
        }

        public static int Part1(string input)
        {
            return Solve(input, 2020);
        }

        public static int Part2(string input)
        {
            return Solve(input, 30000000);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}