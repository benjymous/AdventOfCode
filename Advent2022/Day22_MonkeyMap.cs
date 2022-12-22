using AoC.Utils;
using AoC.Utils.Pathfinding;
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
            foreach (var c in tape)
            {
                if (c >= '0' && c <='9')
                {
                    current += c;
                }
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

                maxX = Data.Max(kvp => kvp.Key.x);
                maxY = Data.Max(kvp => kvp.Key.y);

                for (int y = 0; y <= maxY; ++y)
                {
                    rowMin[y] = Data.Where(kvp => kvp.Key.y == y).Min(kvp => kvp.Key.x);
                    rowMax[y] = Data.Where(kvp => kvp.Key.y == y).Max(kvp => kvp.Key.x);
                }
                for (int x = 0; x <= maxX; ++x)
                {
                    colMin[x] = Data.Where(kvp => kvp.Key.x == x).Min(kvp => kvp.Key.y);
                    colMax[x] = Data.Where(kvp => kvp.Key.x == x).Max(kvp => kvp.Key.y);
                }
            }

            public ManhattanVector2 Start => new(rowMin[0], 0);
            public int FaceSize => (int)Math.Sqrt(Data.Count / 6);

            public char this[(int x, int y) key] => Data[key];
            public bool ContainsKey((int x, int y) key) => Data.ContainsKey(key);

            public Face[,] faces = null;
            public Dictionary<char, Face> faceIndex = null;
            public Dictionary<char, (int x, int y)> faceLocationIndex = null;

            public readonly Dictionary<(int x, int y), char> Data;
            public readonly IEnumerable<string> Tape;

            public readonly Dictionary<int, int> rowMin = new(), rowMax = new(), colMin = new(), colMax = new();

            public readonly int maxX, maxY;

            public void OrientCubeFaces()
            {
                int faceSize = FaceSize;

                var facesX = (maxX + 1) / faceSize;
                var facesY = (maxY + 1) / faceSize;

                faces = new Face[facesX, facesY];

                Console.WriteLine($"{faceSize} : {facesX}, {facesY}");

                bool first = true;

                Queue<(int x, int y)> unresolvedFaces = new();

                for (int y = 0; y < facesY; ++y)
                {
                    for (int x = 0; x < facesX; ++x)
                    {
                        if (Data.ContainsKey((x * faceSize, y * faceSize)))
                        {
                            Console.Write("[]");
                            if (first)
                            {
                                faces[x, y] = new Face('f', Direction2.Up);
                            }
                            else
                            {
                                unresolvedFaces.Add((x, y));
                                faces[x, y] = new Face('?', Direction2.Null);
                            }

                            first = false;
                        }
                        else
                        {
                            Console.Write("  ");
                            faces[x, y] = null;
                        }
                    }
                    Console.WriteLine();
                }

                unresolvedFaces.Operate(face =>
                {
                    if (face.x > 0 && faces[face.x - 1, face.y] != null && faces[face.x - 1, face.y].Name != '?') faces[face.x, face.y] = NeighbourFace(faces[face.x - 1, face.y], Direction2.Right);
                    else if (face.y > 0 && faces[face.x, face.y - 1] != null && faces[face.x, face.y - 1].Name != '?') faces[face.x, face.y] = NeighbourFace(faces[face.x, face.y - 1], Direction2.Down);
                    else if (face.x < facesX - 1 && faces[face.x + 1, face.y] != null && faces[face.x + 1, face.y].Name != '?') faces[face.x, face.y] = NeighbourFace(faces[face.x + 1, face.y], Direction2.Left);
                    else if (face.y < facesY - 1 && faces[face.x, face.y + 1] != null && faces[face.x, face.y + 1].Name != '?') faces[face.x, face.y] = NeighbourFace(faces[face.x, face.y + 1], Direction2.Up);
                    else unresolvedFaces.Enqueue(face);
                });

                faceIndex = faces.Values().Where(v => v != null).ToDictionary(v => v.Name);
                faceLocationIndex = faces.Entries().Where(v => v.value != null).ToDictionary(v => v.value.Name, v => (v.key.x * faceSize, v.key.y * faceSize));

                Console.WriteLine();
                for (int y = 0; y < facesY; ++y)
                {
                    for (int x = 0; x < facesX; ++x)
                    {
                        if (faces[x, y] != null)
                        {
                            var f = faces[x, y];
                            Console.Write($"{f.Name}{f.Orientation.AsChar()}");
                        }
                        else
                        {
                            Console.Write("  ");
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        static (int x, int y) RotatePosition(int x, int y, int size, int rightTurns)
        {
            return rightTurns switch
            {
                0 => (x, y),
                1 => (size - y, x),
                2 => (size - x, size - y),
                3 => (y, size - x),
                _ => throw new Exception ("ye cannae rotate that much!")
            };
        }

        private static int FollowMap(Map map, QuestionPart part)
        {        
            if (part.Two()) map.OrientCubeFaces();
            ManhattanVector2 pos = map.Start;
            Direction2 dir = new(1, 0);

            var faceSize = map.FaceSize;
            var wrapSize = faceSize - 1;

            var debug = new Dictionary<(int x, int y), char>();

            foreach (var instr in map.Tape)
            {
                //debug[pos] = dir.AsChar();
                if (instr == "L")
                {
                    dir.TurnLeft();
                }
                else if (instr == "R")
                {
                    dir.TurnRight();
                }
                else
                {
                    
                    var steps = int.Parse(instr);

                    for (int i = 0; i < steps; ++i)
                    {
                        debug[pos] = dir.AsChar();
                        var nextPos = pos + dir;
                        var nextDir = new Direction2(dir);


                        if (!map.ContainsKey(nextPos))
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
                                int gridX = pos.X / faceSize;
                                int gridY = pos.Y / faceSize;
                                var face = map.faces[gridX, gridY];
                                var neighbourFace = NeighbourFace(face, dir);
                                var placedNeighbour = map.faceIndex[neighbourFace.Name];

                                int localX = pos.X % faceSize;
                                int localY = pos.Y % faceSize;

                                Console.WriteLine($"wrap {dir} : {face.Name} => {neighbourFace.Name}");

                                if (face.Orientation != neighbourFace.Orientation || face.Orientation != placedNeighbour.Orientation)
                                {
                                    var currentToNatural = face.Orientation.AsChar() switch
                                    {
                                        '^' => 0,
                                        '<' => 1,
                                        'v' => 2,
                                        '>' => 3,
                                        _ => throw new Exception("bad direction!")
                                    };

                                    (localX, localY) = RotatePosition(localX, localY, wrapSize, currentToNatural);

                                    (int faceToNeighbour, int faceToGrid) = ((face.Orientation - neighbourFace.Orientation), (face.Orientation - placedNeighbour.Orientation));
                                    
                                    //(localX, localY, int realRot) = (rotateBy, flipBy) switch
                                    //{
                                    //    (0,1) => (localY, localX, 1),   
                                    //    (0,2) => (localX, wrapSize-localX, 2), // * f->l, l->f?
                                    //    (0,3) =>  // *t->f
                                    //    (1,0) => (localY, localX, 3), //* b->r
                                    //    (1,2) => (wrapSize - localY, wrapSize - localX, 1),
                                    //    (2,0) => (wrapSize - localX, localY, 2),

                                    //    (3,0) => (localY, localX, 1), // * r->b
                                    //    (3,2) => (wrapSize - localY, localX, 2), // * b->l
                                    //    //3 => throw new Exception("!!"),
                                    //    _ => throw new Exception("She cannae rotate any more cap'n!")
                                    //};;  
                                    //nextDir.TurnRightBySteps(realRot);
                                }

                                var offset = map.faceLocationIndex[neighbourFace.Name];

                                nextPos.X = localX + offset.x;
                                nextPos.Y = localY + offset.y;
                            }
                        }
                  
                        var c = map[nextPos];
                        if (c == '#')
                            break;

                        pos = nextPos;
                        dir = nextDir;
                    }
                }
            }
            debug[pos] = '*';

            Console.WriteLine();
            for (int y=0; y<=map.maxY; ++y)
            {
                for (int x = 0; x <= map.maxX; ++x)
                {
                    if (debug.ContainsKey((x,y)))
                    {
                        Console.Write(debug[(x, y)]);
                    }
                    else if (map.Data.ContainsKey((x,y)))
                    {
                        Console.Write(map.Data[(x, y)]);
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }

            int row = pos.Y + 1;
            int col = pos.X + 1;

            //0 for right (>), 1 for down (v), 2 for left (<), and 3 for up (^)
            int dirScore = dir.AsChar() switch
            {
                '>' => 0,
                'v' => 1,
                '<' => 2,
                '^' => 3,
                _ => throw new Exception("unexpected dir")
            };

            var sum = (1000 * row) + (col * 4) + dirScore;

            return sum;
        }

        record Face(char Name, Direction2 Orientation) { };

        static Face NeighbourFace(Face face, Direction2 delta)
        {
            var dir = (face.Orientation + delta);
            return (face.Name, dir.AsChar()) switch
            {
                ('f', '>') => new('r', face.Orientation),
                ('f', '<') => new('l', face.Orientation),
                ('f', '^') => new('t', face.Orientation),
                ('f', 'v') => new('b', face.Orientation),
                ('k', '>') => new('l', face.Orientation),
                ('k', '<') => new('r', face.Orientation),
                ('k', '^') => new('t', new Direction2(face.Orientation).Turn180()),
                ('k', 'v') => new('b', new Direction2(face.Orientation).Turn180()),
                ('r', '>') => new('k', face.Orientation),
                ('r', '<') => new('f', face.Orientation),
                ('r', '^') => new('t', new Direction2(face.Orientation).TurnRight()),
                ('r', 'v') => new('b', new Direction2(face.Orientation).TurnLeft()),
                ('l', '>') => new('f', face.Orientation),
                ('l', '<') => new('k', face.Orientation),
                ('l', '^') => new('t', new Direction2(face.Orientation).TurnLeft()),
                ('l', 'v') => new('b', new Direction2(face.Orientation).TurnRight()),
                ('t', '>') => new('r', new Direction2(face.Orientation).TurnLeft()),
                ('t', '<') => new('l', new Direction2(face.Orientation).TurnRight()),
                ('t', '^') => new('k', new Direction2(face.Orientation).Turn180()),
                ('t', 'v') => new('f', face.Orientation),
                ('b', '>') => new('r', new Direction2(face.Orientation).TurnRight()),
                ('b', '<') => new('l', new Direction2(face.Orientation).TurnLeft()),
                ('b', '^') => new('f', face.Orientation),
                ('b', 'v') => new('k', new Direction2(face.Orientation).Turn180()),
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
            //logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
