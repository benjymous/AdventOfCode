namespace AoC.Advent2021;
public class Day08 : IPuzzle
{
    public class DataRow
    {
        [Regex(@"(.+) \| (.+)")]
        public DataRow([Split(" ")] List<string> diag, [Split(" ")] List<string> outp) => (diagnostic, output) = (diag.Select(Convert).ToArray(), outp.Select(Convert).ToArray());

        static uint Convert(string chars) => (uint)chars.Sum(c => 1 << (c - 'a'));

        readonly uint[] diagnostic, output;

        public int CountOutputMatches(int size) => output.Count(v => v.CountBits() == size);

        public int Decode()
        {
            /*    0:      1:      2:      3:      4:           2 segs = 1
                 aaaa    ....    aaaa    aaaa    ....          3 segs = 7
                b    c  .    c  .    c  .    c  b    c         4 segs = 4
                b    c  .    c  .    c  .    c  b    c         5 segs = 2, 3, 5
                 ....    ....    dddd    dddd    dddd          6 segs = 0, 6, 9
                e    f  .    f  e    .  .    f  .    f         7 segs = 8
                e    f  .    f  e    .  .    f  .    f
                 gggg    ....    gggg    gggg    ....

                  5:      6:      7:      8:      9:
                 aaaa    aaaa    aaaa    aaaa    aaaa
                b    .  b    .  .    c  b    c  b    c
                b    .  b    .  .    c  b    c  b    c
                 dddd    dddd    ....    dddd    dddd
                .    f  e    f  .    f  e    f  .    f
                .    f  e    f  .    f  e    f  .    f
                 gggg    gggg    ....    gggg    gggg   */

            var segments = diagnostic.GroupBy(v => v.CountBits()).ToDictionary(kvp => kvp.Key, kvp => kvp.ToHashSet());

            uint zero, one, two, three, four, five, six, seven, eight, nine;

            // 1, 4, 7, 8 are unique in their segment groups
            one = segments[2].Single();
            four = segments[4].Single();
            seven = segments[3].Single();
            eight = segments[7].Single();

            // three is the only 5 segmenter with both segments from 1
            segments[5].Remove(three = segments[5].Single(v => (v & one) == one));

            // six is the only 6 seg which doesn't contain both segs from 1
            segments[6].Remove(six = segments[6].Single(v => (v & one) != one));

            // five shares 5 of 6's segs
            segments[5].Remove(five = segments[5].Single(v => (v & six) == v));

            // two is the remaining 5 seg
            two = segments[5].Single();

            // nine shares all of 5's segs
            segments[6].Remove(nine = segments[6].Single(v => (v & five) == five));

            // zero is the remaining 6 seg
            zero = segments[6].Single();

            var decoder = new Dictionary<uint, int>
            { { zero, 0 }, { one, 1 }, { two, 2 }, { three, 3 }, { four, 4 }, { five, 5 }, { six, 6 }, { seven, 7 }, { eight, 8 }, { nine, 9 } };

            return output.Aggregate(0, (prev, curr) => (prev * 10) + decoder[curr]);
        }
    }

    public static int Part1(Parser.AutoArray<DataRow> data)
        => data.Select(line => Util.Values(2, 3, 4, 7).Sum(segCount => line.CountOutputMatches(segCount))).Sum();

    public static int Part2(Parser.AutoArray<DataRow> data)
        => data.Sum(line => line.Decode());

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}