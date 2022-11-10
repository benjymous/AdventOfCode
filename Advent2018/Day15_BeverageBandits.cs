using AoC.Utils;
using AoC.Utils.Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2018
{
    public class Day15 : IPuzzle
    {
        public string Name => "2018-15";

        enum CreatureType
        {
            Elf,
            Goblin
        };

        class Creature
        {
            public CreatureType type;
            public int HP = 200;
            public int AP = 3;
        }

        static readonly (int dx, int dy)[] InRange = { (0, -1), (-1, 0), (1, 0), (0, 1) };

        class Map : IMap<(int x, int y)>
        {
            public Map(string input, int elfAP)
            {
                var data = Util.ParseSparseMatrix<char>(input);

                Walls = data.Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();
                var gobins = data.Where(kvp => kvp.Value == 'G').ToDictionary(kvp => kvp.Key, _ => new Creature { type = CreatureType.Goblin });
                var elves = data.Where(kvp => kvp.Value == 'E').ToDictionary(kvp => kvp.Key, _ => new Creature { type = CreatureType.Elf, AP = elfAP });

                Creatures = gobins.Union(elves).ToDictionary();
            }

            readonly HashSet<(int x, int y)> Walls;
            readonly Dictionary<(int x, int y), Creature> Creatures;

            public int ElfCount() => Creatures.Values.Where(c => c.type == CreatureType.Elf).Count();

            bool IsClear((int x, int y) pos)
            {
                return !Walls.Contains(pos) && !Creatures.ContainsKey(pos);
            }

            (bool hasTarget, (int x, int y) newPos) MoveUnit(KeyValuePair<(int x, int y), Creature> creature)
            {
                var targets = Creatures.Where(c => c.Value.type != creature.Value.type).ToDictionary();

                if (!targets.Any()) return (false, (0,0));

                var target = FindTarget(creature, targets);

                if (target != creature.Key)
                {
                    //Console.WriteLine($"{creature.Value.type} moves to {target}");
                    Creatures.Remove(creature.Key);
                    Creatures.Add(target, creature.Value);
                }
                return (true, target);
            }

            (int x, int y) FindTarget(KeyValuePair<(int x, int y), Creature> creature, Dictionary<(int x, int y), Creature> targets)
            {
                foreach (var (dx, dy) in InRange)
                {
                    var pos = (x:creature.Key.x + dx, y:creature.Key.y + dy);
                    if (targets.TryGetValue(pos, out var potential))
                    {
                        return (creature.Key);
                    }
                }

                var potentialTargets = targets.SelectMany(target => InRange.Select(offset => (x:target.Key.x + offset.dx, y: target.Key.y + offset.dy))).Where(pos => IsClear(pos) || pos == creature.Key).ToHashSet();

                var reachable = potentialTargets.Select(pos => (pos, path: AStar<(int x, int y)>.FindPath(this, creature.Key, pos)))
                                                 .Where(entry => entry.path.Any())
                                                 .Select(entry => (entry.pos, dist:entry.path.Count()));

                if (!reachable.Any()) return creature.Key;

                var targetPos = reachable.OrderBy(v => (v.dist, v.pos.y, v.pos.x)).First();

                //Console.WriteLine($"Heads towards {targetPos.pos}");

                return ReadingOrderMove(creature.Key, targetPos.pos);
            }

            public (int x, int y) ReadingOrderMove((int x, int y) pos, (int x, int y) dest)
            {
                List<((int x, int y) pos, int dist)> readingOrder = new();
                foreach (var (dx, dy) in InRange)
                {
                    var newPos = (pos.x + dx, pos.y + dy);
                    if (newPos == dest) return newPos;
                    if (IsClear(newPos))
                    {
                        var path = AStar<(int x, int y)>.FindPath(this, newPos, dest);
                        if (path.Any()) readingOrder.Add((newPos, path.Count()));
                    }
                }

                return readingOrder.OrderBy(v => (v.dist, v.pos.y, v.pos.x)).First().pos;
            }

            public bool PerformTurn()
            {
                var moveOrder = Creatures.OrderBy(kvp => (kvp.Key.y, kvp.Key.x)).ToArray();

                foreach (var creature in moveOrder)
                {
                    //Console.WriteLine($"{creature.Value.type} {creature.Key} considers turn");

                    if (creature.Value.HP <= 0) continue;

                    var (hasTarget, newPos) = MoveUnit(creature);
                    if (!hasTarget) return false;

                    var (x, y) = newPos;

                    var targets = new List<((int x, int y) pos, Creature creature)>();
                    foreach (var (dx, dy) in InRange)
                    {
                        var pos = (x + dx, y + dy);
                        if (Creatures.TryGetValue(pos, out var potential))
                        {
                            if (potential.type != creature.Value.type)
                            {
                                targets.Add((pos, potential));
                            }
                        }
                    }

                    if (targets.Any())
                    {
                        var target= targets.OrderBy(t => (t.creature.HP, t.pos.y, t.pos.x)).First();

                        //Console.WriteLine($"{creature.Value.type} {creature.Key} attacks {target.creature.type} {target.pos}");

                        target.creature.HP -= creature.Value.AP;

                        if (target.creature.HP <= 0)
                        {
                            //Console.WriteLine($"{target.creature.type} dies");
                            Creatures.Remove(target.pos);
                        }
                    }
                }
                return true;
            }

            public IEnumerable<(int x, int y)> GetNeighbours((int x, int y) center) => from offset in InRange
                                                                                       let pos = (center.x + offset.dx, center.y + offset.dy)
                                                                                       where IsClear(pos)
                                                                                       select pos;

            public int Heuristic((int x, int y) location1, (int x, int y) location2) => Math.Abs(location1.x - location2.x) + Math.Abs(location1.y - location2.y);
            public int GScore((int x, int y) location) => 1;

            //public void Display()
            //{
            //    int maxX = Walls.Max(p => p.x);
            //    int maxY = Walls.Max(p => p.x);
            //    for (int y = 0; y <= maxY; ++y)
            //    {
            //        var lineStats = "   ";
            //        for (int x = 0; x <= maxX; ++x)
            //        {                
            //            var c = ' ';
            //            if (Walls.Contains((x, y))) c = '#';
            //            else if (Creatures.TryGetValue((x,y), out var creature))
            //            {                       
            //                c = creature.type == CreatureType.Elf ? 'E' : 'G';

            //                lineStats += $"{c}({creature.HP}) ";
            //            }
            //            Console.Write(c);
            //        }
            //        Console.Write(lineStats);
            //        Console.WriteLine();
            //    }

            //    Console.WriteLine();
            //}

            public int Score() { return Creatures.Sum(c => c.Value.HP); }
        }


        private static int ElfBattle(string input, int elfAP, bool endOnElfLoss)
        {
            var map = new Map(input, elfAP);
            int initialElves = map.ElfCount();

            int turns = 0;

            //map.Display();

            while (true)
            {

                if (!map.PerformTurn()) break;

                if (endOnElfLoss)
                {
                    if (map.ElfCount() != initialElves)
                    {
                        return -1;
                    }
                }
                turns++;
                //Console.WriteLine();
                //Console.WriteLine($"After {turns} rounds");
                //map.Display();
            }

            return map.Score() * turns;
        }


        public static int Part1(string input)
        {
            return ElfBattle(input, 3, false);
        }

        public static int Part2(string input)
        {
            return Util.BinarySearch(4, elfAP =>
            {
                var score = ElfBattle(input, (int)elfAP, true);
                bool win = score > 0;
                Console.WriteLine($"{elfAP} => {win}");
                return (win, score);
            }).result;

        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}
