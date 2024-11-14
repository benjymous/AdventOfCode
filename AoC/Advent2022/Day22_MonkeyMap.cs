namespace AoC.Advent2022;
public class Day22 : IPuzzle
{
    class Map
    {
        public Map(string input)
        {
            var (mapData, tapeData) = input.DecomposeSections();
            Data = Util.ParseSparseMatrix<char>(mapData, new Util.Convertomatic.SkipChars(' ')).ToFrozenDictionary();
            Tape = Util.SplitNumbersAndWords(tapeData);
            (maxX, maxY) = (Data.Max(kvp => kvp.Key.x), Data.Max(kvp => kvp.Key.y));
            FaceSize = (int)Math.Sqrt(Data.Count / 6);
        }

        public readonly int FaceSize;

        public Dictionary<(int x, int y), Face> faces = [];
        public Dictionary<char, (Face face, (int x, int y) pos)> faceIndex = null;

        public FrozenDictionary<(int x, int y), char> Data;
        public readonly IEnumerable<(string word, int number)> Tape;

        public readonly int maxX, maxY;

        public void OrientCubeFaces()
        {
            var (facesX, facesY) = ((maxX + 1) / FaceSize, (maxY + 1) / FaceSize);

            var unresolvedFaces = Util.Range2DInclusive((0, facesX, 0, facesY)).Where(k => Data.ContainsKey((k.x * FaceSize, k.y * FaceSize))).ToQueue();
            faces[(unresolvedFaces.Peek().x, unresolvedFaces.Peek().y)] = new Face('f', Direction2.Up);

            unresolvedFaces.Operate(face =>
            {
                if (faces.TryGetValue((face.x - 1, face.y), out var nextTo)) faces[(face.x, face.y)] = CreateNeighbour(nextTo, Direction2.Right);
                else if (faces.TryGetValue((face.x, face.y - 1), out nextTo)) faces[(face.x, face.y)] = CreateNeighbour(nextTo, Direction2.Down); // is up and down transposed here?
                else if (faces.TryGetValue((face.x + 1, face.y), out nextTo)) faces[(face.x, face.y)] = CreateNeighbour(nextTo, Direction2.Left);
                else if (faces.TryGetValue((face.x, face.y + 1), out nextTo)) faces[(face.x, face.y)] = CreateNeighbour(nextTo, Direction2.Up);
                else unresolvedFaces.Enqueue(face);
            });

            faceIndex = faces.Where(v => v.Value != null).ToDictionary(v => v.Value.Name, v => (v.Value, (v.Key.x * FaceSize, v.Key.y * FaceSize)));
        }
    }

    private static int FollowMap(Map map, QuestionPart part)
    {
        if (part.Two()) map.OrientCubeFaces();
        var rowMinMax = Enumerable.Range(0, map.maxY).Select(y => map.Data.Where(kvp => kvp.Key.y == y).MinMax(kvp => kvp.Key.x)).ToArray();
        var colMinMax = Enumerable.Range(0, map.maxY).Select(x => map.Data.Where(kvp => kvp.Key.x == x).MinMax(kvp => kvp.Key.y)).ToArray();

        var (pos, dir) = ((x: rowMinMax[0].min, y: 0), new Direction2(1, 0));

        var wrapSize = map.FaceSize - 1;

        var neighbourMap = part.Two() ? MapNeighbours() : null;

        foreach (var (tapeInstruction, tapeSteps) in map.Tape)
        {
            if (tapeInstruction == "L") dir.TurnLeft();
            else if (tapeInstruction == "R") dir.TurnRight();
            else
            {
                for (int i = 0; i < tapeSteps; ++i)
                {
                    (var nextPos, var nextDir) = (pos.OffsetBy(dir), dir);
                    if (!map.Data.ContainsKey(nextPos))
                    {
                        if (part.One())
                        {
                            if (dir.DX != 0) nextPos.x = dir.DX > 0 ? rowMinMax[nextPos.y].min : rowMinMax[nextPos.y].max;
                            else nextPos.y = dir.DY > 0 ? colMinMax[nextPos.x].min : colMinMax[nextPos.x].max;
                        }
                        else
                        {
                            var face = map.faces[(pos.x / map.FaceSize, pos.y / map.FaceSize)];
                            var placedNeighbour = map.faceIndex[Neighbour(face, dir).name];
                            var (localX, localY) = (pos.x % map.FaceSize, pos.y % map.FaceSize);

                            (localX, localY, nextDir) = (dir.AsChar(), (neighbourMap[(placedNeighbour.face.Name, face.Name)] + placedNeighbour.face.Orientation).AsChar()) switch
                            {
                                ('>', '^') => (wrapSize - localY, wrapSize - localX, nextDir + 1),
                                ('>', 'v') or ('<', '^') => (localY, localX, nextDir - 1),
                                ('>', '<') or ('<', '>') => (localX, wrapSize - localY, nextDir + 2),
                                ('v', 'v') => (wrapSize - localX, localY, dir + 2),
                                ('v', '>') or ('v', '<') or ('^', '>') or ('^', '<') => (localY, localX, nextDir + 1),
                                ('v', '^') or ('^', '^') => (localX, wrapSize - localY, nextDir),
                                _ => throw new Exception("unhandled case")
                            };

                            nextPos = (localX, localY).OffsetBy(placedNeighbour.pos);
                        }
                    }
                    if (map.Data[nextPos] == '#') break;

                    (pos, dir) = (nextPos, nextDir);
                }
            }
        }

        return (1000 * (pos.y + 1)) + ((pos.x + 1) * 4) + (Direction2.Right - dir);
    }

    private static Dictionary<(char from, char to), Direction2> MapNeighbours() => "fkrltb".Select(c => new Face(c, Direction2.North)).SelectMany(face => "<>^v".Select(d => (face, new Direction2(d)))).ToDictionary(r => (r.face.Name, Neighbour(r.face, r.Item2).name), r => r.Item2);

    record Face(char Name, Direction2 Orientation);

    static Face CreateNeighbour(Face current, Direction2 delta)
    {
        var (name, turnRightBy) = Neighbour(current, delta);
        return new Face(name, new Direction2(current.Orientation).TurnRightBySteps(turnRightBy));
    }

    static (char name, int turnRightBy) Neighbour(Face current, Direction2 delta) => (current.Name, (current.Orientation + delta).AsChar()) switch
    {
        ('f', '>') => ('r', 0),
        ('f', '<') => ('l', 0),
        ('f', '^') => ('t', 0),
        ('f', 'v') => ('b', 0),

        ('k', '>') => ('l', 0),
        ('k', '<') => ('r', 0),
        ('k', '^') => ('t', 2),
        ('k', 'v') => ('b', 2),

        ('r', '>') => ('k', 0),
        ('r', '<') => ('f', 0),
        ('r', '^') => ('t', 1),
        ('r', 'v') => ('b', -1),

        ('l', '>') => ('f', 0),
        ('l', '<') => ('k', 0),
        ('l', '^') => ('t', -1),
        ('l', 'v') => ('b', 1),

        ('t', '>') => ('r', -1),
        ('t', '<') => ('l', 1),
        ('t', '^') => ('k', 2),
        ('t', 'v') => ('f', 0),

        ('b', '>') => ('r', 1),
        ('b', '<') => ('l', -1),
        ('b', '^') => ('f', 0),
        ('b', 'v') => ('k', 2),

        _ => throw new Exception("unhandled case"),
    };

    public static int Part1(string input) => FollowMap(new Map(input), QuestionPart.Part1);

    public static int Part2(string input) => FollowMap(new Map(input), QuestionPart.Part2);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}