namespace AoC.Advent2020;
public static class MapRowExtension
{
    const char Tree = '#';
    public static bool IsTree(this string[] map, (int x, int y) pos) => map[pos.y][pos.x % map[pos.y].Length] == Tree;
}

public class Day03 : IPuzzle
{
    private static int CountTrees(string[] map, (int dx, int dy) direction)
    {
        int treeCount = 0;
        for ((int x, int y) pos = (0, 0); pos.y < map.Length; pos = pos.OffsetBy(direction))
        {
            if (map.IsTree(pos)) treeCount++;
        }
        return treeCount;
    }

    public static long Part1(string input)
    {
        var map = Util.Split(input).ToArray();

        return CountTrees(map, (3, 1));
    }

    public static long Part2(string input)
    {
        var map = Util.Split(input).ToArray();

        return new List<(int x, int y)>{
            (1, 1),   //Right 1, down 1.
            (3, 1),   //Right 3, down 1. (This is the slope you already checked.)
            (5, 1),   //Right 5, down 1.
            (7, 1),   //Right 7, down 1.
            (1, 2)    //Right 1, down 2.
        }.Product(dir => CountTrees(map, dir));
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}