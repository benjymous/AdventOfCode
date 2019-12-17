using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.MMXIX.NPSA
{
    public class ASCIITerminal
    {
        Dictionary<string, char> screenBuffer = new Dictionary<string, char>();

        public ManhattanVector2 Cursor {get;} = new ManhattanVector2(0,0);

        public ManhattanVector2 Max {get;} = new ManhattanVector2(0,0);

        public bool DisplayLive {get;set;} = false;            

        public ASCIITerminal()
        {

        }

        public void Write(char c)
        {
            if (DisplayLive)
            {
                Console.Write(c);
            }

            switch(c)
            {
                case '\n':

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
    }
}