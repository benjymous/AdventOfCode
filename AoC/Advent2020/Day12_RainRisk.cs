namespace AoC.Advent2020;
public class Day12 : IPuzzle
{
    public class FactoryPt1
    {
        [Regex(@"N(\d+)")] public static Action<ManhattanVector2, Direction2> ActionN(int val) => (ManhattanVector2 v, Direction2 d) => v.Offset(Direction2.North, val);
        [Regex(@"S(\d+)")] public static Action<ManhattanVector2, Direction2> ActionS(int val) => (ManhattanVector2 v, Direction2 d) => v.Offset(Direction2.South, val);
        [Regex(@"E(\d+)")] public static Action<ManhattanVector2, Direction2> ActionE(int val) => (ManhattanVector2 v, Direction2 d) => v.Offset(Direction2.East, val);
        [Regex(@"W(\d+)")] public static Action<ManhattanVector2, Direction2> ActionW(int val) => (ManhattanVector2 v, Direction2 d) => v.Offset(Direction2.West, val);

        [Regex(@"L(\d+)")] public static Action<ManhattanVector2, Direction2> ActionL(int val) => (ManhattanVector2 v, Direction2 d) => d.TurnLeftByDegrees(val);
        [Regex(@"R(\d+)")] public static Action<ManhattanVector2, Direction2> ActionR(int val) => (ManhattanVector2 v, Direction2 d) => d.TurnRightByDegrees(val);

        [Regex(@"F(\d+)")] public static Action<ManhattanVector2, Direction2> ActionF(int val) => (ManhattanVector2 v, Direction2 d) => v.Offset(d, val);
    }

    public class FactoryPt2
    {
        [Regex(@"N(\d+)")] public static Action<ManhattanVector2, ManhattanVector2> ActionN(int val) => (ManhattanVector2 ship, ManhattanVector2 wp) => wp.Offset(Direction2.North, val);
        [Regex(@"S(\d+)")] public static Action<ManhattanVector2, ManhattanVector2> ActionS(int val) => (ManhattanVector2 ship, ManhattanVector2 wp) => wp.Offset(Direction2.South, val);
        [Regex(@"E(\d+)")] public static Action<ManhattanVector2, ManhattanVector2> ActionE(int val) => (ManhattanVector2 ship, ManhattanVector2 wp) => wp.Offset(Direction2.East, val);
        [Regex(@"W(\d+)")] public static Action<ManhattanVector2, ManhattanVector2> ActionW(int val) => (ManhattanVector2 ship, ManhattanVector2 wp) => wp.Offset(Direction2.West, val);

        [Regex(@"L(\d+)")] public static Action<ManhattanVector2, ManhattanVector2> ActionL(int val) => (ManhattanVector2 ship, ManhattanVector2 wp) => wp.TurnLeftBy(val);
        [Regex(@"R(\d+)")] public static Action<ManhattanVector2, ManhattanVector2> ActionR(int val) => (ManhattanVector2 ship, ManhattanVector2 wp) => wp.TurnRightBy(val);

        [Regex(@"F(\d+)")] public static Action<ManhattanVector2, ManhattanVector2> ActionF(int val) => (ManhattanVector2 ship, ManhattanVector2 wp) => ship.Offset(wp, val);
    }

    public static int Part1(Util.AutoParse<Action<ManhattanVector2, Direction2>, FactoryPt1> input)
    {
        var position = new ManhattanVector2(0, 0);
        var direction = new Direction2(1, 0);

        input.ForEach(i => i(position, direction));

        return position.Distance(ManhattanVector2.Zero);
    }

    public static int Part2(Util.AutoParse<Action<ManhattanVector2, ManhattanVector2>, FactoryPt2> input)
    {
        var position = new ManhattanVector2(0, 0);
        var waypoint = new ManhattanVector2(10, -1);

        input.ForEach(i => i(position, waypoint));

        return position.Distance(ManhattanVector2.Zero);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}