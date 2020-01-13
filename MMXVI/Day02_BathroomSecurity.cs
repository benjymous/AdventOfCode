using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Advent.Utils.Vectors;

namespace Advent.MMXVI
{
    public class Day02 : IPuzzle
    {
        public string Name { get { return "2016-02";} }

        static string keypad1 = "123  \n"+
                                "456  \n"+
                                "789";

        static string keypad2 = "  1   \n"+
                                " 234  \n"+
                                "56789 \n"+
                                " ABC  \n"+
                                "  D   \n";

        static Dictionary<string,string> ParseKeypad(string kp)
        {
            var result = new Dictionary<string,string>();
            var lines = Util.Split(kp);
            int y=0;

            foreach (var line in lines)
            {
                int x=0;
                foreach (var c in line)
                {                   
                    if (c != ' ')
                    {
                        result[$"{x},{y}"]=c.ToString();
                    }
                    x++;
                }
                y++;
            }
            return result;
        }

        public static string SimulateKeypad(string input, string keypadLayout)
        {
            Dictionary<string,string> keypad = ParseKeypad(keypadLayout);

            ManhattanVector2 position = null;

            foreach (var kvp in keypad)
            {
                if (kvp.Value == "5")
                {
                    position = new ManhattanVector2(kvp.Key);
                }
            }

            var lines = Util.Split(input);
            StringBuilder code = new StringBuilder();

            ManhattanVector2 newPos = new ManhattanVector2(0,0);

            foreach (var line in lines)
            {         
                foreach (var c in line)
                {
                    newPos.Set(position.X, position.Y);
                    switch(c)
                    {
                        case 'L': newPos.Offset(-1,0); break;
                        case 'R': newPos.Offset(1,0); break;
                        case 'U': newPos.Offset(0,-1); break;
                        case 'D': newPos.Offset(0,1); break;
                    }
                    if (keypad.ContainsKey(newPos.ToString()))
                    {
                        position.Set(newPos.X, newPos.Y);
                    }
                }
                code.Append(keypad[position.ToString()]);
            }

            return code.ToString();
        }

        public static string Part1(string input)
        {
           return SimulateKeypad(input, keypad1);
        }

        public static string Part2(string input)
        {
            return SimulateKeypad(input, keypad2);
        }

        public void Run(string input, ILogger logger)
        {
          
            //logger.WriteLine(Part1("ULL\nRRDDD\nLURDL\nUUUUD"));
            logger.WriteLine(Part2("ULL\nRRDDD\nLURDL\nUUUUD"));

            logger.WriteLine("- Pt1 - "+Part1(input));
            logger.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}