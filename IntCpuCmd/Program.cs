using System;
using AoC.Advent2019.NPSA;

namespace IntCpuCmd
{
    class Program
    {

        class Runner : ICPUInterrupt
        {
            readonly IntCPU cpu;
            public Runner(string program)
            {
                cpu = new(program)
                {
                    Interrupt = this
                };
            }

            public void Run() => cpu.Run();

            public void OutputReady()
            {
                Console.WriteLine($"- {cpu.Output.Dequeue()}");
            }

            public void RequestInput()
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
