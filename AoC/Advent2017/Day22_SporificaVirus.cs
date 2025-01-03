﻿namespace AoC.Advent2017;
public class Day22 : IPuzzle
{
    public static int Part1(string input)
    {
        var data = Util.ParseSparseMatrix<PackedPos32, bool>(input);

        PackedPos32 pos = (x: data.Width / 2, y: data.Height / 2);
        var dir = new Direction2(Direction2.North);

        var grid = data.KeysWithValue(true).ToHashSet();

        int infections = 0;

        for (int i = 0; i < 10000; ++i)
        {
            var infected = grid.Contains(pos);
            if (!infected)
            {
                dir.TurnLeft();
                grid.Add(pos);
                infections++;
            }
            else
            {
                dir.TurnRight();
                grid.Remove(pos);
            }
            pos += (dir.DX, dir.DY);
        }

        return infections;
    }

    const char Clean = '.';
    const char Infected = '#';
    const char Weakened = 'W';
    const char Flagged = 'F';

    public static int Part2(string input)
    {
        var data = Util.ParseSparseMatrix<PackedPos32, char>(input);

        PackedPos32 pos = (data.Width / 2, data.Height / 2);
        var dir = new Direction2(Direction2.North);

        var grid = data.Where(kvp => kvp.Value == Infected).ToDictionary();

        int infections = 0;

        for (int i = 0; i < 10000000; ++i)
        {
            if (!grid.TryGetValue(pos, out var cell)) cell = Clean;

            switch (cell)
            {
                case Clean:
                    dir.TurnLeft();
                    grid[pos] = Weakened;
                    break;

                case Infected:
                    dir.TurnRight();
                    grid[pos] = Flagged;
                    break;

                case Weakened:
                    grid[pos] = Infected;
                    infections++;
                    break;

                case Flagged:
                    dir.Turn180();
                    grid.Remove(pos);
                    break;
            }

            pos += (dir.DX, dir.DY);
        }

        return infections;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}