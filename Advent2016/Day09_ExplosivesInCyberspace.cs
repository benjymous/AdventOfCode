using System;

namespace AoC.Advent2016
{
    public class Day09 : IPuzzle
    {
        public string Name => "2016-09";

        static (int numChars, int numRepeats) Parse32(string cmd)
        {
            var bits = Util.ParseNumbers<int>(cmd, 'x');
            return (bits[0], bits[1]);
        }

        static Int64 Decompress(string input, bool recurse)
        {
            Int64 length = 0;
            int i = 0;
            while (i < input.Length)
            {
                var c = input[i];

                if (c == '(')
                {
                    int start = i;
                    while (input[i++] != ')') ;

                    var (numChars, numRepeats) = Parse32(input.Substring(start + 1, i - start - 2));

                    if (recurse)
                    {
                        length += Decompress(input.Substring(i, numChars), true) * numRepeats;
                    }
                    else
                    {
                        length += numChars * numRepeats;
                    }
                    i += numChars;

                }
                else
                {
                    i++;
                    length++;
                }
            }
            return length;
        }

        public static Int64 Part1(string input)
        {
            return Decompress(input.Trim(), false);
        }

        public static Int64 Part2(string input)
        {
            return Decompress(input.Trim(), true);
        }

        public void Run(string input, ILogger logger)
        {
            //Util.Test(Part1("ADVENT"), 6UL);
            //Util.Test(Part1("A(1x5)BC"), 7UL);
            //Util.Test(Part1("(3x3)XYZ"), 9UL);

            //Util.Test(Part2("ADVENT"), 6UL);
            //Util.Test(Part2("A(1x5)BC"), 7UL);
            // Util.Test(Part2("(3x3)XYZ"), 9UL);
            // Util.Test(Part2("X(8x2)(3x3)ABCY"), 20UL);
            // Util.Test(Part2("(27x12)(20x12)(13x14)(7x10)(1x12)A"), 241920UL);
            // Util.Test(Part2("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN"), 445UL);

            logger.WriteLine("- Pt1 - " + Part1(input));  // 107035
            logger.WriteLine("- Pt2 - " + Part2(input));  // 11451628995
        }
    }
}