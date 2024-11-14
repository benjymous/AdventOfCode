namespace AoC.Advent2023;
public class Day16 : IPuzzle
{
    static void Step((int x, int y) pos, Direction2 dir, char[,] map, int[,] beams)
        => DoBeam(pos.OffsetBy(dir), dir, map, beams);

    static void DoBeam((int x, int y) pos, Direction2 dir, char[,] map, int[,] beams)
    {
        if (!map.Contains(pos)) return;

        int beamBit = 1 << dir.RotationSteps();
        if ((beams[pos.x, pos.y] & beamBit) == 0)
        {
            beams[pos.x, pos.y] |= beamBit;
            var c = map[pos.x, pos.y];

            if ((c == '|' && dir.DY == 0) || (c == '-' && dir.DX == 0))
            {
                Step(pos, dir - 1, map, beams);
                Step(pos, dir + 1, map, beams);
            }
            else
            {
                if (c is '\\' or '/') dir.Mirror(c);

                Step(pos, dir, map, beams);
            }
        }
    }

    private static int RunBeamSimulation((int x, int y) start, Direction2 dir, char[,] data)
    {
        var beams = new int[data.Width(), data.Height()];

        DoBeam(start, dir, data, beams);

        return beams.Values().Count(v => v > 0);
    }

    private static IEnumerable<int> SolvePart2(char[,] data)
    {
        int size = data.Width() - 1;
        for (int i = 0; i <= size; ++i)
        {
            yield return RunBeamSimulation((0, i), '>', data);
            yield return RunBeamSimulation((size, i), '<', data);
            yield return RunBeamSimulation((i, 0), 'v', data);
            yield return RunBeamSimulation((i, size), '^', data);
        }
    }

    public static int Part1(string input) => RunBeamSimulation((0, 0), '>', Util.ParseMatrix<char>(input));

    public static int Part2(string input) => SolvePart2(Util.ParseMatrix<char>(input)).Max();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}