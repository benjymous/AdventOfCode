using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day17 : IPuzzle
    {
        public string Name => "2018-17";

        public class Wall
        {
            [Regex(@"(.)=(\d+), .=(\d+)..(\d+)")]
            public Wall(char xOrY, int v1, int v2, int v3)
            {
                for (int v = v2; v <= v3; ++v)
                {
                    Vals.Add(xOrY == 'x' ? (v1, v) : (v, v1));
                }
            }

            public HashSet<(int x, int y)> Vals = new();
        }

        static bool IsWall(int x, int y, Dictionary<(int x, int y), char> grid) => grid.TryGetValue((x, y), out var c) && c == '#';

        static bool IsFilled(int x, int y, Dictionary<(int x, int y), char> grid) => grid.TryGetValue((x, y), out var c) && c == '#' || c == '~';

        static void Set((int x, int y) pos, Dictionary<(int x, int y), char> grid, int maxY)
        {
            if (grid.ContainsKey(pos)) return;

            grid[pos] = '|';

            if (pos.y < maxY && !grid.ContainsKey((pos.x, pos.y + 1))) Set((pos.x, pos.y + 1), grid, maxY);

            if (IsFilled(pos.x, pos.y + 1, grid))
            {
                int x1 = pos.x;
                int x2 = pos.x;

                while (true)
                {
                    if (IsWall(x1 - 1, pos.y, grid) || !IsFilled(x1 - 1, pos.y + 1, grid)) break;
                    x1--;
                }
                bool openLeft = !IsWall(x1 - 1, pos.y, grid);

                while (true)
                {
                    if (IsWall(x2 + 1, pos.y, grid) || !IsFilled(x2 + 1, pos.y + 1, grid)) break;
                    x2++;
                }
                bool openRight = !IsWall(x2 + 1, pos.y, grid);
            
                if (!openLeft && !openRight)
                {
                    for (int x = x1; x <= x2; ++x)
                    {
                        grid[(x, pos.y)] = '~';
                    }
                }
                else
                {
                    if (openLeft || x1 < pos.x)
                    {
                        Set((pos.x - 1, pos.y), grid, maxY);
                    }

                    if (openRight || x2 > pos.x)
                    {
                        Set((pos.x + 1, pos.y), grid, maxY);
                    }
                }             
            }
        }

        private static Dictionary<(int x, int y), char> RunWater(string input)
        {
            var walls = Util.RegexParse<Wall>(input);
            var data = walls.SelectMany(w => w.Vals).ToHashSet().ToDictionary(p => p, p => '#');
            int maxY = data.Max(p => p.Key.y);
            int minY = data.Min(p => p.Key.y);

            Set((500, minY), data, maxY);

            return data;
        }

        private static void RenderPng(Dictionary<(int x, int y), char> data)
        {
            var minx = data.Min(p => p.Key.x) - 5;
            var maxx = data.Max(p => p.Key.x) + 5;
            var maxy = data.Max(p => p.Key.y) + 5;

            int width = maxx - minx + 1;
            int height = maxy;

            var bmp = new Bitmap(width, height+1);

            for (var y = 0; y < maxy; ++y)
            {
                for (var x = minx; x <= maxx ; ++x)
                {
                    Color col = Color.MidnightBlue;
                    if (!data.TryGetValue((x, y), out var c)) c = '.';
                   
                    if (c == '#') col = Color.Black;
                    if (c == '~') col = Color.DodgerBlue;
                    if (c == '|') col = Color.DeepSkyBlue;

                    bmp.SetPixel(x-minx, y, col);
                }
            }

            bmp.Save(@"clay.png");
        }

        public static int Part1(string input)
        {
            Dictionary<(int x, int y), char> data = RunWater(input);

            //RenderPng(data);
            return data.Values.Count(v => v == '|' || v == '~');
        }

        public static int Part2(string input)
        {
            Dictionary<(int x, int y), char> data = RunWater(input);

            return data.Values.Count(v => v == '~');
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
