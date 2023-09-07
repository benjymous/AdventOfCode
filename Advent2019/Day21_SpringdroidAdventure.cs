using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019
{
    class SpringDroid : NPSA.ASCIITerminal
    {
        public long DamageReport => finalOutput;

        public SpringDroid(string program) : base(program)
        {
        }

        IEnumerable<string> commandBuffer;

        public void Run(IEnumerable<string> program)
        {
            commandBuffer = program;
            cpu.Reserve(3000);
            cpu.Run();
            Console.WriteLine(cpu.Speed());
        }

        public override IEnumerable<string> AutomaticInput() => commandBuffer;

    }
    public class Day21 : IPuzzle
    {
        public string Name => "2019-21";

        public static long SurveyHull(string input, IEnumerable<string> commandBuffer)
        {
            var droid = new SpringDroid(input);
            droid.SetDisplay(false);
            droid.Interactive = false;
            droid.Run(commandBuffer);
            return droid.DamageReport;
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

            var commandBuffer = new[]
            {
                "NOT A J",
                "NOT B T",
                "OR T J",
                "NOT C T",
                "OR T J",
                "AND D J",
                "WALK"
            };

            return SurveyHull(input, commandBuffer);
        }

        public static long Part2(string input)
        {
            // (!A|!B|!C) & D & (E|H))

            var commandBuffer = new[]
            {

            //////////////////////////////////
                "NOT A J",
                "NOT B T",
                "OR T J",   // (!A) | (!B) | (!C)
                "NOT C T",  // there's a hole in one of the next three spaces
                "OR T J",
            //////////////////////////////////
                "AND D J",  // & D
                            // theres a solid space four away

            //////////////////////////////////

                "NOT D T", // Clear T (since we know D must be true)

            //////////////////////////////////

                "OR E T",  // (E) | (H)
                "OR H T",  // theres a solid space five or eight spaces away

            //////////////////////////////////

                "AND T J", // combine it all together

                "RUN"
            };

            return SurveyHull(input, commandBuffer);
        }

        private static void Interactive(string input)
        {

            Console.WriteLine();
            Console.WriteLine("..@..........");
            Console.WriteLine("###...#######");
            Console.WriteLine("   ABCD");
            Console.WriteLine();

            var droid = new SpringDroid(input);
            droid.SetDisplay(true);
            droid.Interactive = true;
            droid.Run(Enumerable.Empty<string>());
        }

        public void Run(string input, ILogger logger)
        {
            //Interactive(input);

            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }


    }
}