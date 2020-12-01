using Advent.Utils;
using Advent.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX.NPSA
{

    abstract class ASCIITerminal : NPSA.ICPUInterrupt
    {
        protected NPSA.IntCPU cpu;

        protected NPSA.ASCIIBuffer buffer = new NPSA.ASCIIBuffer();

        protected Int64 finalOutput;

        public bool Interactive { get; set; } = false;

        public ASCIITerminal(string program)
        {
            cpu = new NPSA.IntCPU(program);
            cpu.Interrupt = this;
        }

        public void SetDisplay(bool on)
        {
            buffer.DisplayLive = on;
        }

        public void HasPutOutput()
        {
            Int64 v = cpu.Output.Dequeue();

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

        public void WillReadInput()
        {
            var inputs = AutomaticInput().ToArray();

            if (inputs.Any())
            {
                foreach (var line in inputs)
                {
                    if (Interactive)
                    {
                        Console.WriteLine(line);
                    }
                    foreach (var c in line)
                    {
                        cpu.Input.Enqueue(c);
                    }
                    cpu.Input.Enqueue('\n');
                }
            }
            else
            {
                if (Interactive)
                {
                    Console.Write("?> ");
                    var input = Console.ReadLine();
                    foreach (var c in input.ToArray())
                    {
                        cpu.Input.Enqueue(c);
                    }
                    cpu.Input.Enqueue('\n');
                }
            }
        }
    }

    public class ASCIIBuffer
    {
        Dictionary<string, char> screenBuffer = new Dictionary<string, char>();

        public ManhattanVector2 Cursor { get; } = new ManhattanVector2(0, 0);

        public ManhattanVector2 Max { get; } = new ManhattanVector2(0, 0);

        public bool DisplayLive { get; set; } = false;

        public List<string> Lines { get; set; } = new List<string>();
        private StringBuilder sb = new StringBuilder();

        public ASCIIBuffer()
        {
        }

        public void Write(char c)
        {
            if (DisplayLive)
            {
                Console.Write(c);
            }

            switch (c)
            {
                case '\n':

                    Lines.Add(sb.ToString());
                    sb = new StringBuilder();

                    if (Cursor.X == 0)
                    {
                        Cursor.Y = 0;
                        Cursor.X = 0;
                        if (DisplayLive)
                        {
                            Console.WriteLine();
                        }
                    }

                    Cursor.X = 0;
                    Cursor.Y++;
                    break;

                default:
                    sb.Append(c);
                    screenBuffer.PutObjKey(Cursor, c);
                    Cursor.X++;
                    break;
            }

            Max.X = Math.Max(Max.X, Cursor.X);
            Max.Y = Math.Max(Max.Y, Cursor.Y);
        }

        public char GetAt(int x, int y)
        {
            return screenBuffer.GetStrKey($"{x},{y}");
        }

        public ManhattanVector2 FindCharacter(char c)
        {
            var res = screenBuffer.Where(kvp => kvp.Value == c);
            if (res.Any())
            {
                return new ManhattanVector2(res.First().Key);
            }

            throw new Exception("Not found");
        }

        public void Clear()
        {
            Max.Set(0, 0);
            screenBuffer.Clear();
            Lines.Clear();
            sb = new StringBuilder();
        }

    }
}