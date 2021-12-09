using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2021
{
    public class Day08 : IPuzzle
    {
        public string Name { get { return "2021-08"; } }

        class DataRow
        {
            public DataRow(string input)
            {
                this.input = input;
                var bits = input.Split(" | ");
                test = bits[0].Split(" ").Select(v => v.OrderBy(c => (int)c).ToArray().AsString());
                output = bits[1].Split(" ").Select(v => v.OrderBy(c => (int)c).ToArray().AsString());
            }
            public string input;
            public IEnumerable<string> test;
            public IEnumerable<string> output;

            public int CountOutputMatches(int size)
            {
                return output.Count(v => v.Length == size);
            }

            public int Decode()
            {

                /*
                      0:      1:      2:      3:      4:
                     aaaa    ....    aaaa    aaaa    ....
                    b    c  .    c  .    c  .    c  b    c
                    b    c  .    c  .    c  .    c  b    c
                     ....    ....    dddd    dddd    dddd
                    e    f  .    f  e    .  .    f  .    f
                    e    f  .    f  e    .  .    f  .    f
                     gggg    ....    gggg    gggg    ....

                      5:      6:      7:      8:      9:
                     aaaa    aaaa    aaaa    aaaa    aaaa
                    b    .  b    .  .    c  b    c  b    c
                    b    .  b    .  .    c  b    c  b    c
                     dddd    dddd    ....    dddd    dddd
                    .    f  e    f  .    f  e    f  .    f
                    .    f  e    f  .    f  e    f  .    f
                     gggg    gggg    ....    gggg    gggg
                */

                var segments = test.GroupBy(v => v.Length).ToDictionary(kvp => kvp.Key, kvp => kvp.ToList());

                // 2 segs = 1
                // 3 segs = 7
                // 4 segs = 4
                // 5 segs = 2, 3, 5
                // 6 segs = 0, 6, 9
                // 7 segs = 8

                // 1, 4, 7, 8 are unique in their segment groups
                var one = segments[2].First();
                var four = segments[4].First();
                var seven = segments[3].First();
                var eight = segments[7].First();
                segments[2].Remove(one); 
                segments[4].Remove(four); 
                segments[3].Remove(seven); 
                segments[7].Remove(eight);

                // three is the only 5 segmenter with both segments from 1
                var three = segments[5].Where(v => v.Intersect(one).Count() == one.Length).First();
                segments[5].Remove(three);

                // six is the only 6 seg which doesn't contain both segs from 1
                var six = segments[6].Where(v => v.Intersect(one).Count() != one.Length).First();
                segments[6].Remove(six);

                // five shares 5 of 6's segs
                var five = segments[5].Where(v => v.Intersect(six).Count() == 5).First();
                segments[5].Remove(five);

                // two is the remaining 5 seg
                var two = segments[5].First();
                segments[5].Remove(two);

                // nine shares all of 5's segs
                var nine = segments[6].Where(v => v.Intersect(five).Count() == five.Length).First();
                segments[6].Remove(nine);

                // zero is the remaining 6 seg
                var zero = segments[6].First();
                segments[6].Remove(zero);

                var decoder = new Dictionary<string, char>
                {
                    { zero,  '0' },
                    { one,   '1' },
                    { two,   '2' },
                    { three, '3' },
                    { four,  '4' },
                    { five,  '5' },
                    { six,   '6' },
                    { seven, '7' },
                    { eight, '8' },
                    { nine,  '9' }
                };

                return int.Parse(output.Select(v => decoder[v]).ToArray().AsString());
            }

            public override string ToString()
            {
                return input;
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

            return data.Select(line => line.Decode()).Sum();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
 }