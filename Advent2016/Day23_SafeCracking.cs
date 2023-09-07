namespace AoC.Advent2016
{
    public class Day23 : IPuzzle
    {
        public string Name => "2016-23";

        public static int Crack(string program, int input)
        {
            var cpu = new BunniTek.BunnyCPU(program);
            cpu.Set(BunniTek.RegisterId.a, input);
            cpu.Run();
            System.Console.WriteLine(cpu.Speed());
            return cpu.Get(BunniTek.RegisterId.a);
        }

        public static int Part1(string input)
        {
            return Crack(input, 7);
        }

        public static int Part2(string input)
        {
            return Crack(input, 12);
        }

        public void Run(string input, ILogger logger)
        {
            // var prog = "cpy 2 a\ntgl a\ntgl a\ntgl a\ncpy 1 a\ndec a\ndec a";

            // var cpu = new BunniTek.BunnyCPU(prog);
            // cpu.Run();
            // Console.WriteLine(cpu.Get(BunniTek.RegisterId.a));



            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}