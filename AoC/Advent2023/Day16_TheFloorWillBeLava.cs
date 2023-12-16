namespace AoC.Advent2023;
public class Day16 : IPuzzle
{
    static void Step((int x, int y) pos, Direction2 dir, char[,] map, int[,] beams) => DoBeam(pos.OffsetBy(dir), dir, map, beams);

    static void DoBeam((int x, int y) pos, Direction2 dir, char[,] map, int[,] beams)
    {
        if (pos.x < 0 || pos.x >= map.Width() || pos.y < 0 || pos.y >= map.Height()) return;

        int beamBit = 1 << dir.RotationSteps();
        if ((beams[pos.x, pos.y] & beamBit) == 0)
        {
            beams[pos.x, pos.y] |= beamBit;

            switch (map[pos.x, pos.y])
            {
                case '\\':
                    dir.SetDirection(dir.DY, dir.DX);
                    break;

                case '/':
                    dir.SetDirection(-dir.DY, -dir.DX);
                    break;

                case '|':
                    if (dir.DY == 0)
                    {
                        Step(pos, dir - 1, map, beams);
                        Step(pos, dir + 1, map, beams);

                        return;
                    }
                    break;

                case '-':
                    if (dir.DX == 0)
                    {
                        Step(pos, dir - 1, map, beams);
                        Step(pos, dir + 1, map, beams);

                        return;
                    }
                    break;
            }
            Step(pos, dir, map, beams);
        }
    }

    private static int RunBeamSimulation((int x, int y) start, Direction2 dir, char[,] data)
    {
        var beams = new int[data.Width(), data.Height()];

        DoBeam(start, dir, data, beams);

        return beams.Values().Where(v => v > 0).Count();
    }

    private static IEnumerable<int> SolvePart2(char[,] data)
    {
        int size = data.Width() - 1;
        for (int i = 0; i <= size; ++i)
        {
            yield return RunBeamSimulation((0, i), Direction2.FromChar('>'), data);
            yield return RunBeamSimulation((size, i), Direction2.FromChar('<'), data);
            yield return RunBeamSimulation((i, 0), Direction2.FromChar('v'), data);
            yield return RunBeamSimulation((i, size), Direction2.FromChar('^'), data);
        }
    }

    public static int Part1(string input) => RunBeamSimulation((0, 0), Direction2.FromChar('>'), Util.ParseMatrix<char>(input));

    public static int Part2(string input) => SolvePart2(Util.ParseMatrix<char>(input)).Max();

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input)); 
    }
}