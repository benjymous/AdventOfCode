using AoC.Advent2017.NorthCloud;

namespace AoC.Advent2017
{
    public class Day23 : IPuzzle
    {
        public string Name => "2017-23";

        public static int Part1(string input)
        {
            int mulCount = 0;
            var cpu = new Coprocessor(input, "Common,Day23")
            {
                Debugger = (line) => { if (line.Name == "mul") mulCount++; return true; }
            };
            cpu.Run();
            return mulCount;
        }

        public static long Part2(string input)
        {
            var cpu = new Coprocessor(input, "Common,Day23")
            {
                Debugger = (line) => line.Index != 8
            };
            cpu.Set('a', 1);
            cpu.Run();

            var b = cpu.Get('b');
            int h = 0;

            // counting non primes between b and b+17000 (in 17 step increments)

            for (long i = b; i <= b + 17000; i += 17)
            {
                for (long j = 2; j < i; ++j)
                {
                    if (i % j == 0)
                    {
                        h++;
                        break;
                    }
                }
            }

            return h;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}