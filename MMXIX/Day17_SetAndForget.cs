using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXIX
{
    class Hoovamatic : NPSA.ICPUInterrupt
    {
        NPSA.IntCPU cpu;
        NPSA.ASCIITerminal terminal = new NPSA.ASCIITerminal();

        public Hoovamatic(string input)
        {
            cpu = new NPSA.IntCPU(input);
            cpu.Interrupt = this;
        }

        public void Activate()
        {
            cpu.Poke(0, 2);
        }

        public int Run()
        {
            cpu.Run();
            return 0;
        }

        public IEnumerable<ManhattanVector2> FindIntersections()
        {
            for (int y=1; y<terminal.Max.Y; ++y)
            {
                for (int x=1; x<terminal.Max.X; ++x)
                {
                    if ((terminal.GetAt(x,y)=='#') &&
                        (terminal.GetAt(x-1,y)=='#') &&
                        (terminal.GetAt(x+1,y)=='#') &&
                        (terminal.GetAt(x,y-1)=='#') &&
                        (terminal.GetAt(x,y+1)=='#'))
                    {
                        yield return new ManhattanVector2(x,y);
                    }
                }
            }
        }

        public void HasPutOutput()
        {
            char c = (char)cpu.Output.Dequeue();

            terminal.Write(c);
        }

        public void WillReadInput()
        {          
            throw new NotImplementedException();
        }
    }

    public class Day17 : IPuzzle
    {
        public string Name { get { return "2019-17";} }
 
        public static int Part1(string input)
        {
            var robot = new Hoovamatic(input);

            robot.Run();

            var intersections = robot.FindIntersections();

            return intersections.Select(v => v.X*v.Y).Sum();
        }

        public static int Part2(string input)
        {
            var robot = new Hoovamatic(input);
            robot.Activate();
            //robot.Run();

            return 0;
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}