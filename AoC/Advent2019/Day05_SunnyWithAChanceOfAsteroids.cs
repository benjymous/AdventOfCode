﻿namespace AoC.Advent2019;
public class Day05 : IPuzzle
{
    public static string RunProgram(string program, int input)
    {
        var cpu = new NPSA.IntCPU(program);
        cpu.AddInput(input);
        cpu.Run();
        return string.Join(",", cpu.Output);
    }

    public static string Part1(string input) => RunProgram(input, 1);

    public static string Part2(string input) => RunProgram(input, 5);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}
