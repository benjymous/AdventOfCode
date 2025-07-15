namespace AoC.Advent2017;
public class Day18 : IPuzzle
{
    private class Part1Port : NorthCloud.IOutputPort
    {
        public long Value { get; private set; }

        public void Write(long value) => Value = value;
    }

    private class Part2Port : NorthCloud.IInputPort, NorthCloud.IOutputPort
    {
        public int SendCount { get; internal set; } = 0;

        private readonly Queue<long> values = [];

        public void Write(long value)
        {
            SendCount++;
            values.Enqueue(value);
        }

        public bool HasData() => values.Count != 0;

        public long Read() => values.Dequeue();
    }

    public static long Part1(string input)
    {
        var cpu = new NorthCloud.Coprocessor(input, "Common,Day18,Day18Part1");
        var reciever = new Part1Port();
        cpu.Bus.Output = reciever;
        cpu.Run();
        return reciever.Value;
    }

    public static int Part2(string input)
    {
        var port01 = new Part2Port();
        var port10 = new Part2Port();

        var cpu0 = new NorthCloud.Coprocessor(input, "Common,Day18,Day18Part2");
        var cpu1 = new NorthCloud.Coprocessor(input, "Common,Day18,Day18Part2");

        cpu0.Set('p', 0);
        cpu0.Bus.Output = port01;
        cpu0.Bus.Input = port10;

        cpu0.Set('p', 1);
        cpu1.Bus.Output = port10;
        cpu1.Bus.Input = port01;

        while (cpu0.Bus.Waiting == false || cpu1.Bus.Waiting == false)
        {
            if (!cpu0.Step()) break;
            if (!cpu1.Step()) break;
        }

        return port01.SendCount;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}