using AoC.Utils;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2021
{
    public class Day08 : IPuzzle
    {
        public string Name => "2021-08";

        class DataRow
        {
            public DataRow(string input)
            {
                var bits = input.Split(" | ");
                diagnostic = bits[0].Split(" ").Select(v => v.Order().AsString());
                output = bits[1].Split(" ").Select(v => v.Order().AsString());
            }

            readonly IEnumerable<string> diagnostic, output;

            public int CountOutputMatches(int size) => output.Count(v => v.Length == size);

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

                var segments = diagnostic.GroupBy(v => v.Length).ToDictionary(kvp => kvp.Key, kvp => kvp.ToList());

                string zero, one, two, three, four, five, six, seven, eight, nine;

                // 1, 4, 7, 8 are unique in their segment groups
                segments[2].Remove(one = segments[2].First());
                segments[4].Remove(four = segments[4].First());
                segments[3].Remove(seven = segments[3].First());
                segments[7].Remove(eight = segments[7].First());

                // three is the only 5 segmenter with both segments from 1
                segments[5].Remove(three = segments[5].First(v => v.Intersect(one).Count() == one.Length));

                // six is the only 6 seg which doesn't contain both segs from 1
                segments[6].Remove(six = segments[6].First(v => v.Intersect(one).Count() != one.Length));

                // five shares 5 of 6's segs
                segments[5].Remove(five = segments[5].First(v => v.Intersect(six).Count() == 5));

                // two is the remaining 5 seg
                segments[5].Remove(two = segments[5].First());

                // nine shares all of 5's segs
                segments[6].Remove(nine = segments[6].First(v => v.Intersect(five).Count() == five.Length));

                // zero is the remaining 6 seg
                segments[6].Remove(zero = segments[6].First());

                var decoder = new Dictionary<string, char>
                { { zero, '0' }, { one, '1' }, { two, '2' }, { three, '3' }, { four, '4' }, { five, '5' }, { six, '6' }, { seven, '7' }, { eight, '8' }, { nine, '9' } };

                return int.Parse(output.Select(v => decoder[v]).AsString());
            }
        }

        public static int Part1(string input)
        {
            var data = Util.Parse<DataRow>(input);

            return data.Select(line => Util.Values(2, 3, 4, 7).Select(segCount => line.CountOutputMatches(segCount)).Sum()).Sum();
        }

        public static int Part2(string input)
        {
            var data = Util.Parse<DataRow>(input);

            return data.Sum(line => line.Decode());
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}