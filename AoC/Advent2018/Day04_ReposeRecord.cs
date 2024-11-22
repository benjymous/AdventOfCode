namespace AoC.Advent2018;
public class Day04 : IPuzzle
{
    enum EntryType
    {
        begins_shift,
        falls_asleep,
        wakes_up
    }

    [method: Regex(@"\[....-..-.. (..:..)\] (?:Guard #(\d+) )?(falls asleep|wakes up|begins shift)")]
    class Entry(string timecode, int guardId, EntryType type)
    {
        public readonly int Timestamp = int.Parse(timecode.Replace(":", ""));
        public readonly EntryType EntryType = type;
        public readonly int GuardId = guardId;
    }

    static (Dictionary<int, Dictionary<int, int>>, Dictionary<int, int> durations) Parse(string input)
    {
        int id = -1;
        int sleepStart = 0;

        Dictionary<int, Dictionary<int, int>> guards = [];
        Dictionary<int, int> durations = [];

        foreach (var r in Util.RegexParse<Entry>(Util.Split(input).Order()))
        {
            switch (r.EntryType)
            {
                case EntryType.begins_shift:
                    {
                        id = r.GuardId;
                        if (!guards.ContainsKey(id))
                        {
                            guards[id] = [];
                            durations[id] = 0;
                        }
                    }
                    break;

                case EntryType.falls_asleep:
                    sleepStart = r.Timestamp;
                    break;

                case EntryType.wakes_up:
                    {
                        int sleepEnd = r.Timestamp;
                        durations[id] += sleepEnd - sleepStart;

                        for (var m = sleepStart; m < sleepEnd; ++m)
                            guards[id].IncrementAtIndex(m);
                    }
                    break;
            }
        }

        return (guards, durations);
    }

    public static int Part1(string input)
    {
        var (guards, durations) = Parse(input);

        var sleepiest = durations.MaxBy(kvp => kvp.Value).Key;
        var m = guards[sleepiest].MaxBy(kvp => kvp.Value).Key;

        return sleepiest * m;
    }

    public static int Part2(string input)
    {
        var (guards, durations) = Parse(input);

        var (guardId, min, count) = guards.SelectMany(kvp => kvp.Value.Select(kvp2 => (guardId: kvp.Key, min: kvp2.Key, count: kvp2.Value))).MaxBy(entry => entry.count);
        return guardId * min;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}