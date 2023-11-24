namespace AoC.Advent2020;
public class Day02 : IPuzzle
{

    [Regex(@"(\d+)-(\d+) (.): (.+)")]
    public record class Record(int LowCount, int HighCount, char TestChar, string Password)
    {
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

    public static int Part1(string input) => Util.RegexParse<Record>(input).Count(r => r.ValidPt1);

    public static int Part2(string input) => Util.RegexParse<Record>(input).Count(r => r.ValidPt2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}