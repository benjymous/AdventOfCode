namespace AoC.Advent2017;
public class Day17 : IPuzzle
{
    public static Circle<int> Spinlock(int step, int totalSize)
    {
        var current = new Circle<int>(0);

        for (int i = 1; i < totalSize; ++i)
        {
            current = current.Forward(step).InsertNext(i);
        }

        return current;
    }

    public static int Part1(string input)
    {
        int steps = int.Parse(input);
        return Spinlock(steps, 2018).Next().Value;
    }

    public static int Part2(string input)
    {
        int step = int.Parse(input);
        int totalSize = 50000000;

        int val = -1;

        int current = 0;
        // we only need to keep track of anything that falls into the 1th slot

        for (int i = 1; i < totalSize; ++i)
        {
            current = ((current + step) % i) + 1;
            if (current == 1)
            {
                val = i;
            }
        }

        return val;
    }

    public void Run(string input, ILogger logger)
    {
        //Util.Test(Part1("3"), 638);

        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}