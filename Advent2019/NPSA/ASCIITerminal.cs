using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2019.NPSA
{

    abstract class ASCIITerminal : ICPUInterrupt
    {
        protected IntCPU cpu;

        protected ASCIIBuffer buffer = new();

        protected long finalOutput;

        public bool Interactive { get; set; } = false;

        public ASCIITerminal(string program, int reserve = 0)
        {
            cpu = new IntCPU(program)
            {
                Interrupt = this
            };
            if (reserve > 0) cpu.Reserve(reserve);
        }

        public void SetDisplay(bool on)
        {
            buffer.DisplayLive = on;
        }

        public void OutputReady()
        {
            long v = cpu.Output.Dequeue();

            if (v <= 255)
            {
                buffer.Write((char)v);
            }
            else
            {
                finalOutput = v;
            }
        }

        public abstract IEnumerable<string> AutomaticInput();

        public void RequestInput()
        {
            var inputs = AutomaticInput().ToArray();

            if (inputs.Length != 0)
            {
                foreach (var line in inputs)
                {
                    if (Interactive)
                    {
                        Console.WriteLine(line);
                    }
                    foreach (var c in line)
                    {
                        cpu.AddInput(c);
                    }
                    cpu.AddInput('\n');
                }
            }
            else
            {
                if (Interactive)
                {
                    Console.Write("?> ");
                    var input = Console.ReadLine();
                    cpu.AddInput(input.Select(c => (long)c).ToArray());
                    cpu.AddInput('\n');
                }
            }
        }
    }

    class InteractiveTerminal : ASCIITerminal
    {
        public InteractiveTerminal(string program) : base(program)
        {
            Interactive = true;
            SetDisplay(true);
        }

        public void Run() => cpu.Run();

        public override IEnumerable<string> AutomaticInput()
        {
            throw new NotImplementedException();
        }
    }

    public class ASCIIBuffer
    {
        readonly Dictionary<int, char> screenBuffer = new();

        public ManhattanVector2 Cursor { get; } = new ManhattanVector2(0, 0);

        public ManhattanVector2 Max { get; } = new ManhattanVector2(0, 0);

        public bool DisplayLive { get; set; } = false;

        public List<string> Lines { get; set; } = new List<string>();
        private readonly List<char> sb = new();

        public IEnumerable<string> Pop()
        {
            var lines = new List<string>(Lines);
            Lines.Clear();
            return lines;
        }

        public void Write(char c)
        {
            if (DisplayLive) Console.Write(c);

            if (c == '\n')
            {
                Lines.Add(sb.AsString());
                sb.Clear();

                if (Cursor.X == 0)
                {
                    Cursor.Y = 0;
                    Cursor.X = 0;
                    if (DisplayLive) Console.WriteLine();
                }

                Cursor.X = 0;
                Cursor.Y++;
            }
            else
            { 
                sb.Add(c);
                screenBuffer[Cursor.X + (Cursor.Y << 16)] = c;
                Cursor.X++;
            }

            Max.X = Math.Max(Max.X, Cursor.X);
            Max.Y = Math.Max(Max.Y, Cursor.Y);
        }

        public char GetAt(int x, int y) => screenBuffer.GetOrDefault(x + (y << 16));

        public int FindCharacter(char c)
        {
            var res = FindAll(c);
            if (res.Count != 0)
            {
                return res.First();
            }

            throw new Exception("Not found");
        }

        public HashSet<int> FindAll(char c)
        {
            return screenBuffer.Where(kvp => kvp.Value == c).Select(kvp => kvp.Key).ToHashSet();
        }

        static public (int x, int y) DecodePos(int i) => (i & 0xffff, i >> 16);
        static public int EncodePos(int x, int y) => x + (y << 16);
        static public int EncodePos((int x, int y) p) => p.x + (p.y << 16);
    }
}