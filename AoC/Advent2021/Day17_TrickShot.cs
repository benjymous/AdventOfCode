namespace AoC.Advent2021;
public class Day17 : IPuzzle
{
    [Regex(@"target area: x=(\d+)..(\d+), y=(-?\d+)..(-?\d+)")]
    public record struct TargetRect(int X1, int X2, int Y1, int Y2)
    {
        public static implicit operator TargetRect(string data) => Parser.FromString<TargetRect>(data);

        public readonly bool Contains((int X, int Y) point, bool ignoreX) => ignoreX ? point.Y >= Y1 && point.Y <= Y2 : point.X >= X1 && point.Y >= Y1 && point.X <= X2 && point.Y <= Y2;

        public readonly bool Missed((int X, int Y) point, bool ignoreX) => ignoreX ? point.Y < Y1 : point.X > X2 || point.Y < Y1;
    }

    public static (bool hit, int maxY) TestShot(TargetRect rect, (int DX, int DY) vel, bool ignoreX = false)
    {
        (int X, int Y) pos = (0, 0);
        int maxY = 0;

        while (true)
        {
            pos = (pos.X + vel.DX, pos.Y + vel.DY);
            vel = (vel.DX - Math.Sign(vel.DX), vel.DY - 1);

            maxY = Math.Max(maxY, pos.Y);

            if (rect.Contains(pos, ignoreX)) return (true, maxY);
            if (rect.Missed(pos, ignoreX)) return (false, 0);
        }
    }

    public static int Part1(TargetRect target)
    {
        return Util.RangeBetween(0, 150)
                         .Select(dy => TestShot(target, (0, dy), true))
                         .Where(res => res.hit)
                         .Max(res => res.maxY);
    }

    public static int Part2(TargetRect target)
    {
        return Util.Matrix(Util.RangeBetween(1, target.X2 + 1), Util.RangeBetween(target.Y1, 150))
                    .Select(pos => TestShot(target, pos, false))
                    .Count(res => res.hit);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}