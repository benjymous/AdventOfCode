using System;

namespace IntCpuCmd
{
    class Program
    {

        class Runner : Advent.MMXIX.NPSA.ICPUInterrupt
        {
            Advent.MMXIX.NPSA.IntCPU cpu;
            public Runner(string program)
            {
                cpu = new Advent.MMXIX.NPSA.IntCPU(program);
                cpu.Interrupt = this;
            }

            public void Run() => cpu.Run();

            public void HasPutOutput()
            {
                Console.WriteLine($"- {cpu.Output.Dequeue()}");
            }

            public void WillReadInput()
            {
                Console.Write("?> ");
                var input = Console.ReadLine();
                cpu.Input.Enqueue(Int64.Parse(input));
            }
        }

        static void Main(string[] args)
        {
            var program = string.Join("", args);
            var cpu = new Runner(program);
            cpu.Run();
        }
    }
}
