using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXX
{
    public class Day02 : IPuzzle
    {
        public string Name { get { return "2020-02"; } }

        public class Record
        {
            public Record (string row)
            {
                var parts = row.Split(' ');
                Password = parts[2].Trim();
                TestChar = parts[1][0];
                Values = Util.Parse32(parts[0], '-');
                if (Values.Length != 2) throw new InvalidProgramException($"Invalid record {row}");
            }

            public string Password;
            public char TestChar;
            public int[] Values;

            public bool ValidPt1 
            {  
                get
                {
                    var test = Password.Count(c => c==TestChar);
                    return test >= Values[0] && test <= Values[1];
                } 
            }

            public bool ValidPt2
            {
                get
                {
                    var is1 = Password[Values[0] - 1] == TestChar;
                    var is2 = Password[Values[1] - 1] == TestChar;

                    return is1 ^ is2; // Exclusive or
                }
            }
        }

        public static int Part1(string input)
        {
            var records = Util.Parse<Record>(input);
            return records.Count(r => r.ValidPt1);
        }

        public static int Part2(string input)
        {
            var records = Util.Parse<Record>(input);
            return records.Count(r => r.ValidPt2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}