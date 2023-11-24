namespace AoC.Advent2020;
public class Day13 : IPuzzle
{
    static ulong NextMultiple(ulong n, ulong mult) => (n + (mult - 1)) / mult * mult;

    public static ulong Part1(string input)
    {
        var lines = Util.Split(input, "\n");
        var timestamp = ulong.Parse(lines[0]);
        var times = Util.Split(lines[1]);
        var smallest = ulong.MaxValue;
        ulong bestbus = 0;
        foreach (var t in times)
        {
            if (t == "x") continue;
            var bus = ulong.Parse(t);
            var nexttime = NextMultiple(timestamp, bus);
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

        var nums = new List<(ulong, ulong)>();
        ulong i = 0;
        foreach (var t in times)
        {
            if (t != "x") nums.Add((ulong.Parse(t), i));
            i++;
        }
        ulong res = nums[0].Item1;
        var inc = res;

        foreach (var n in nums.Skip(1))
        {
            var mod = n.Item1 - (n.Item2 % n.Item1);
            while (res % n.Item1 != mod) res += inc;
            inc = Util.LCM(inc, n.Item1);
        }

        return res;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}