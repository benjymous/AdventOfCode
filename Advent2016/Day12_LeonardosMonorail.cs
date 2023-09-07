namespace AoC.Advent2016
{
    public class Day12 : IPuzzle
    {
        public string Name => "2016-12";

        public static int Part1(string input)
        {
            var cpu = new BunniTek.BunnyCPU(input);
            cpu.Run();
            return cpu.Get(BunniTek.RegisterId.a);
        }

        public static int Part2(string input)
        {
            var cpu = new BunniTek.BunnyCPU(input);
            cpu.Set(BunniTek.RegisterId.c, 2);
            cpu.Run();
            System.Console.WriteLine(cpu.Speed());
            return cpu.Get(BunniTek.RegisterId.a);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}