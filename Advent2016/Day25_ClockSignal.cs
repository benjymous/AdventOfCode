using AoC.Advent2016.BunniTek;

namespace AoC.Advent2016
{
    public class Day25 : IPuzzle
    {
        public string Name => "2016-25";

        const int SAMPLE_SIZE = 10;

        static int Signal(Instruction[] program, int input)
        {
            int responseCount = 0;
            var cpu = new BunnyCPU(program)
            {
                Output = i => i == responseCount % 2 && ++responseCount < SAMPLE_SIZE
            };
            cpu.Set(RegisterId.a, input);
            cpu.Run();
            //Console.WriteLine($"{Convert.ToString(input, 2).PadLeft(8, '0')} : {input} : {responseCount}");
            return responseCount;
        }

        public static int Part1(string input)
        {
            var program = BunnyCPU.Compile(input);

            int testBit = 1;
            int result = 0;
            int best = 0;
            while (true)
            {
                int res = Signal(program, result | testBit);
                if (res == SAMPLE_SIZE) return result | testBit;
                if (res > best)
                {
                    best = res;
                    result |= testBit;
                    testBit = 1 << res;
                }
            }
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}