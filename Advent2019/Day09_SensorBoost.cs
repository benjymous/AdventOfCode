namespace AoC.Advent2019
{
    public class Day09 : IPuzzle
    {
        public string Name => "2019-09";

        public static string Run(string program, int input)
        {
            var cpu = new NPSA.IntCPU(program);
            cpu.Reserve(8000);
            cpu.Input.Enqueue(input);
            cpu.Run();
            return string.Join(",", cpu.Output);
        }

        public static string Part1(string input) => Run(input, 1);

        public static string Part2(string input) => Run(input, 2);

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}