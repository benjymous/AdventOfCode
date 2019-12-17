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

        public ASCIITerminal()
        {

        }

        public void Write(char c)
        {
            switch(c)
            {
                case '\n':
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
    }
}