using AoC.Utils;
using AoC.Utils.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2022
{
    public class Day22 : IPuzzle
    {
        public string Name => "2022-22";

        static IEnumerable<string> GetInstructions(string tape)
        {
            var current = "";
            foreach (char c in tape)
            {
                if (c.IsDigit()) current += c;
                else
                {
                    if (current != "") yield return current;
                    yield return c.ToString();
                    current = "";
                }
            }
            if (current != "") yield return current;
        }

        class Map
        {
            public Map(string input)
            {
                var bits = input.Split("\n\n");
                Data = Util.ParseSparseMatrix<char>(bits[0]).Where(kvp => kvp.Value != ' ').ToDictionary();
                Tape = GetInstructions(bits[1].Trim()).ToList();
                (maxX, maxY) = (Data.Max(kvp => kvp.Key.x), Data.Max(kvp => kvp.Key.y));
                for (int y = 0; y <= maxY; ++y) (rowMin[y], rowMax[y]) = (Data.Where(kvp => kvp.Key.y == y).Min(kvp => kvp.Key.x), Data.Where(kvp => kvp.Key.y == y).Max(kvp => kvp.Key.x));
                for (int x = 0; x <= maxX; ++x) (colMin[x], colMax[x]) = (Data.Where(kvp => kvp.Key.x == x).Min(kvp => kvp.Key.y), Data.Where(kvp => kvp.Key.x == x).Max(kvp => kvp.Key.y));
                FaceSize = (int)Math.Sqrt(Data.Count / 6);
            }

            public ManhattanVector2 Start => new(rowMin[0], 0);
            public readonly int FaceSize;

            public Face[,] faces = null;
            public Dictionary<char, Face> faceIndex = null;
            public Dictionary<char, (int x, int y)> faceLocationIndex = null;

            public readonly Dictionary<(int x, int y), char> Data;
            public readonly IEnumerable<string> Tape;

            public readonly Dictionary<int, int> rowMin = new(), rowMax = new(), colMin = new(), colMax = new();

            public readonly int maxX, maxY;

            public void OrientCubeFaces()
            {
                var (facesX, facesY) = ((maxX + 1) / FaceSize, (maxY + 1) / FaceSize);
                faces = new Face[facesX, facesY];
                bool first = true;
                Queue<(int x, int y)> unresolvedFaces = new();

                foreach (var k in Util.Range2DExclusive((0, facesX, 0, facesY)).Where(k => Data.ContainsKey((k.x * FaceSize, k.y * FaceSize))))
                {
                    if (first) faces[k.x, k.y] = new Face('f', Direction2.Up);
                    else
                    {
                        unresolvedFaces.Add(k);
                        faces[k.x, k.y] = new Face('?', Direction2.Null);
                    }
                    first = false;
                }

                unresolvedFaces.Operate(face =>
                {
                    if (face.x > 0 && faces[face.x - 1, face.y] != null && faces[face.x - 1, face.y].Name != '?') faces[face.x, face.y] = faces[face.x - 1, face.y].Neighbour(Direction2.Right);
                    else if (face.y > 0 && faces[face.x, face.y - 1] != null && faces[face.x, face.y - 1].Name != '?') faces[face.x, face.y] = faces[face.x, face.y - 1].Neighbour(Direction2.Down);
                    else if (face.x < facesX - 1 && faces[face.x + 1, face.y] != null && faces[face.x + 1, face.y].Name != '?') faces[face.x, face.y] = faces[face.x + 1, face.y].Neighbour(Direction2.Left);
                    else if (face.y < facesY - 1 && faces[face.x, face.y + 1] != null && faces[face.x, face.y + 1].Name != '?') faces[face.x, face.y] = faces[face.x, face.y + 1].Neighbour(Direction2.Up);
                    else unresolvedFaces.Enqueue(face);
                });

                faceIndex = faces.Values().Where(v => v != null).ToDictionary(v => v.Name);
                faceLocationIndex = faces.Entries().Where(v => v.value != null).ToDictionary(v => v.value.Name, v => (v.key.x * FaceSize, v.key.y * FaceSize));
            }
        }

        private static int FollowMap(Map map, QuestionPart part)
        {
            if (part.Two()) map.OrientCubeFaces();
            ManhattanVector2 pos = map.Start;
            Direction2 dir = new(1, 0);

            var wrapSize = map.FaceSize - 1;

            var neighbourMap = part.Two() ? MapNeighbours() : null;

            foreach (var instr in map.Tape)
            {
                if (instr == "L") dir.TurnLeft();
                else if (instr == "R") dir.TurnRight();
                else
                {
                    for (int i = 0; i < int.Parse(instr); ++i)
                    {
                        (var nextPos, var nextDir) = (pos + dir, new Direction2(dir));
                        if (!map.Data.ContainsKey(nextPos))
                        {
                            if (part.One())
                            {
                                if (dir.DX != 0)
                                {
                                    if (nextPos.X > map.rowMax[nextPos.Y]) nextPos.X = map.rowMin[nextPos.Y];
                                    else if (nextPos.X < map.rowMin[nextPos.Y]) nextPos.X = map.rowMax[nextPos.Y];
                                }
                                else
                                {
                                    if (nextPos.Y > map.colMax[nextPos.X]) nextPos.Y = map.colMin[nextPos.X];
                                    else if (nextPos.Y < map.colMin[nextPos.X]) nextPos.Y = map.colMax[nextPos.X];
                                }
                            }
                            else
                            {
                                var face = map.faces[pos.X / map.FaceSize, pos.Y / map.FaceSize];
                                var placedNeighbour = map.faceIndex[face.Neighbour(dir).Name];
                                var (localX, localY) = (pos.X % map.FaceSize, pos.Y % map.FaceSize);
                                (localX, localY, var rotate) = (dir.AsChar(), (neighbourMap[(placedNeighbour.Name, face.Name)] + placedNeighbour.Orientation).AsChar()) switch
                                {
                                    ('>', '^') => (wrapSize - localY, wrapSize - localX, 1),
                                    ('>', 'v') => (localY, localX, 3),
                                    ('>', '<') => (localX, wrapSize - localY, 2),
                                    ('<', '>') => (localX, wrapSize - localY, 2),
                                    ('<', '^') => (localY, localX, 3),
                                    ('v', 'v') => (wrapSize - localX, localY, 2),
                                    ('v', '>') => (localY, localX, 1),
                                    ('v', '<') => (localY, localX, 1),
                                    ('v', '^') => (localX, wrapSize - localY, 0),
                                    ('^', '>') => (localY, localX, 1),
                                    ('^', '^') => (localX, wrapSize - localY, 0),
                                    ('^', '<') => (localY, localX, 1),
                                    _ => throw new Exception("unhandled case")
                                };
                                nextDir.TurnRightBySteps(rotate);
                                nextPos = new ManhattanVector2(localX, localY) + map.faceLocationIndex[face.Neighbour(dir).Name];
                            }
                        }
                        if (map.Data[nextPos] == '#') break;
                        (pos, dir) = (nextPos, nextDir);
                    }
                }
            }

            return (1000 * (pos.Y + 1)) + ((pos.X + 1) * 4) + (Direction2.Right - dir);
        }

        private static Dictionary<(char from, char to), Direction2> MapNeighbours()
        {
            Dictionary<(char from, char to), Direction2> neighbourMap = new();
            foreach (char c in "fkrltb")
            {
                var face = new Face(c, Direction2.North);
                foreach (char d in "<>^v")
                {
                    var dir = Direction2.FromChar(d);
                    neighbourMap[(c, face.Neighbour(dir).Name)] = dir;
                }
            }
            return neighbourMap;
        }

        record Face(char Name, Direction2 Orientation)
        {
            public Face Neighbour(Direction2 delta) => (Name, (Orientation + delta).AsChar()) switch
            {
                ('f', '>') => new('r', Orientation),
                ('f', '<') => new('l', Orientation),
                ('f', '^') => new('t', Orientation),
                ('f', 'v') => new('b', Orientation),

                ('k', '>') => new('l', Orientation),
                ('k', '<') => new('r', Orientation),
                ('k', '^') => new('t', new Direction2(Orientation).Turn180()),
                ('k', 'v') => new('b', new Direction2(Orientation).Turn180()),

                ('r', '>') => new('k', Orientation),
                ('r', '<') => new('f', Orientation),
                ('r', '^') => new('t', new Direction2(Orientation).TurnRight()),
                ('r', 'v') => new('b', new Direction2(Orientation).TurnLeft()),

                ('l', '>') => new('f', Orientation),
                ('l', '<') => new('k', Orientation),
                ('l', '^') => new('t', new Direction2(Orientation).TurnLeft()),
                ('l', 'v') => new('b', new Direction2(Orientation).TurnRight()),

                ('t', '>') => new('r', new Direction2(Orientation).TurnLeft()),
                ('t', '<') => new('l', new Direction2(Orientation).TurnRight()),
                ('t', '^') => new('k', new Direction2(Orientation).Turn180()),
                ('t', 'v') => new('f', Orientation),

                ('b', '>') => new('r', new Direction2(Orientation).TurnRight()),
                ('b', '<') => new('l', new Direction2(Orientation).TurnLeft()),
                ('b', '^') => new('f', Orientation),
                ('b', 'v') => new('k', new Direction2(Orientation).Turn180()),

                _ => throw new Exception("unhandled case"),
            };
        }

        public static int Part1(string input)
        {
            return FollowMap(new Map(input), QuestionPart.Part1);
        }

        public static int Part2(string input)
        {
            return FollowMap(new Map(input), QuestionPart.Part2);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}