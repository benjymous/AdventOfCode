﻿namespace AoC.Advent2019;
class SpringDroid(string program) : NPSA.ASCIITerminal(program, 3000)
{
    IEnumerable<string> commandBuffer;

    public long Run(IEnumerable<string> program)
    {
        commandBuffer = program;
        base.Run();
        return finalOutput;
    }

    public override IEnumerable<string> AutomaticInput() => commandBuffer;
}

public class Day21 : IPuzzle
{
    public static long SurveyHull(string input, IEnumerable<string> commandBuffer)
    {
        var droid = new SpringDroid(input);
        droid.SetDisplay(false);
        return droid.Run(commandBuffer);
    }

    // AND X Y sets Y to true if both X and Y are true; otherwise, it sets Y to false.
    // OR X Y sets Y to true if at least one of X or Y is true; otherwise, it sets Y to false.
    // NOT X Y sets Y to true if X is false; otherwise, it sets Y to false.

    public static long Part1(string input)
    {
        // (!A|!B|!C) & D

        // ..@..........
        // ###...#######
        //    ABCD

        return SurveyHull(input, [
            "NOT A J",
            "NOT B T",
            "OR T J",
            "NOT C T",
            "OR T J",
            "AND D J",
            "WALK"
        ]);
    }

    public static long Part2(string input)
    {
        // (!A|!B|!C) & D & (E|H))

        return SurveyHull(input, [
            "NOT A J",
            "NOT B T",
            "OR T J",   // (!A) | (!B) | (!C)
            "NOT C T",  // there's a hole in one of the next three spaces
            "OR T J",

            "AND D J",  // & D
                        // theres a solid space four away

            "NOT D T",  // Clear T (since we know D must be true)

            "OR E T",   // (E) | (H)
            "OR H T",   // theres a solid space five or eight spaces away

            "AND T J",  // combine it all together

            "RUN"
        ]);
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}