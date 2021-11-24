using AoC.Utils.Vectors;

namespace AoC.Advent2018
{
    public class Day11 : IPuzzle
    {
        public string Name { get { return "2018-11"; } }

        public static int Power(int serial, int x, int y)
        {
            int rackId = x + 10;
            int powerLevel = (rackId * y);
            powerLevel += serial;
            powerLevel *= rackId;
            int hundreds = (powerLevel / 100) % 10;
            var power = hundreds - 5;

            return power;
        }

        public static string Part1(string input)
        {
            int SERIAL = int.Parse(input);

            var grid = new int[301, 301];

            for (var y = 1; y <= 300; ++y)
            {
                for (var x = 1; x <= 300; ++x)
                {
                    grid[y, x] = Power(SERIAL, x, y);
                }
            }

            var max = 0;
            ManhattanVector2 pos = null;

            for (var y = 1; y < 298; ++y)
            {
                for (var x = 1; x < 298; ++x)
                {
                    var score = 0;

                    for (var ya = 0; ya < 3; ++ya)
                    {
                        for (var xa = 0; xa < 3; ++xa)
                        {
                            score += grid[y + ya, x + xa];
                        }
                    }

                    if (score > max)
                    {
                        max = score;
                        pos = new ManhattanVector2(x, y);
                    }
                }
            }

            return pos.ToString();
        }

        public static string Part2(string input)
        {
            int SERIAL = int.Parse(input);

            var grid = new int[301, 301];

            for (var y = 1; y <= 300; ++y)
            {
                for (var x = 1; x <= 300; ++x)
                {
                    grid[y, x] = Power(SERIAL, x, y);
                }
            }

            var max = 0;
            var lastBest = 0;
            ManhattanVector3 pos = null;

            for (var size = 1; size < 300; ++size)
            {
                int sizeBest = 0;
                //Console.WriteLine($"{size} {max}");
                for (var y = 1; y < 300 - size; ++y)
                {
                    for (var x = 1; x < 300 - size; ++x)
                    {
                        var score = 0;

                        for (var ya = 0; ya < size; ++ya)
                        {
                            for (var xa = 0; xa < size; ++xa)
                            {
                                score += grid[y + ya, x + xa];
                            }
                        }

                        if (score > max)
                        {
                            max = score;
                            pos = new ManhattanVector3(x, y, size);
                        }
                        if (score > sizeBest) sizeBest = score;
                    }
                }

                if (sizeBest + 10 < lastBest)
                {
                    return pos.ToString();
                }
                lastBest = sizeBest;

            }

            return pos.ToString();
        }

        public void Run(string input, ILogger logger)
        {



            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}