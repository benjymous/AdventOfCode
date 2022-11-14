using System.Linq;

namespace AoC.Advent2020
{
    public class Day02 : IPuzzle
    {
        public string Name => "2020-02";

        public class Record
        {
            [Regex(@"(\d+)-(\d+) (.): (.+)")]
            public Record(int low, int high, char testChar, string password)
            {
                (LowCount, HighCount) = (low, high);
                TestChar = testChar;
                Password = password;
            }

            readonly int LowCount, HighCount;
            readonly char TestChar;
            readonly string Password;

            public bool ValidPt1
            {
                get
                {
                    var test = Password.Count(c => c == TestChar);
                    return test >= LowCount && test <= HighCount;
                }
            }

            public bool ValidPt2
            {
                get
                {
                    var is1 = Password[LowCount - 1] == TestChar;
                    var is2 = Password[HighCount - 1] == TestChar;

                    return is1 ^ is2; // Exclusive or
                }
            }
        }

        public static int Part1(string input)
        {
            return Util.RegexParse<Record>(input).Count(r => r.ValidPt1);
        }

        public static int Part2(string input)
        {
            return Util.RegexParse<Record>(input).Count(r => r.ValidPt2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}