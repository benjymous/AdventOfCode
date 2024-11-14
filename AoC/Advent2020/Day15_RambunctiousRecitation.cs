namespace AoC.Advent2020;
public class Day15 : IPuzzle
{
    class Entry(int v1 = -1, int v2 = -1)
    {
        public int Push(int v)
        {
            (v1, v2) = (v2, v);
            return v1;
        }
    }

    class Storage
    {
        public Storage(string input, int reserve)
        {
            values = new Entry[reserve];
            Util.ParseNumbers<int>(input).ForEach(i => AddNumber(i));
        }

        (int, int) AddNumber(int val)
        {
            var e = values[val] ??= new();
            return (val, e.Push(++Count));
        }

        public int Populate(int required)
        {
            int last = 0, v = -1;
            while (Count < required)
            {
                (last, v) = AddNumber(v == -1 ? 0 : Count - v);
            }
            return last;
        }

        public int Count { get; private set; } = 0;

        readonly Entry[] values;
    }

    public static int Solve(string input, int count)
    {
        var data = new Storage(input, count);
        return data.Populate(count);
    }

    public static int Part1(string input) => Solve(input, 2020);

    public static int Part2(string input) => Solve(input, 30000000);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}