namespace AoC.Advent2020;
public class Day13 : IPuzzle
{
    public static ulong Part1(string input)
    {
        var lines = Util.Split(input, "\n");
        var timestamp = ulong.Parse(lines[0]);
        var times = Util.Split(lines[1]);
        var smallest = ulong.MaxValue;
        ulong bestbus = 0;
        foreach (var t in times.Where(t => t != "x"))
        {
            var bus = ulong.Parse(t);
            var nexttime = Util.RoundUpToNextMultiple(timestamp, bus);
            if (nexttime < smallest)
            {
                smallest = nexttime;
                bestbus = bus;
            }
        }
        return (smallest - timestamp) * bestbus;
    }

    public static ulong Part2(string input)
    {
        var lines = Util.Split(input, "\n");
        var times = Util.Split(lines[1]);

        var nums = times.WithIndex().Where(e => e.Value != "x").Select(e => (Value: ulong.Parse(e.Value), Index: (ulong)e.Index)).ToList();

        ulong res = nums[0].Value;
        var inc = res;

        foreach (var n in nums.Skip(1))
        {
            var mod = n.Value - (n.Index % n.Value);
            while (res % n.Value != mod) res += inc;
            inc = Util.LCM(inc, n.Value);
        }

        return res;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}