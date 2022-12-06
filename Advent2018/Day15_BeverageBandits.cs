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

        class Map : IMap<(int x, int y)>, IComparer<(int x, int y)>
        {
            public Map(string input, int elfAP)
            {
                var data = Util.ParseSparseMatrix<char>(input);

                Walls = data.Where(kvp => kvp.Value == '#').Select(kvp => kvp.Key).ToHashSet();
                var gobins = data.Where(kvp => kvp.Value == 'G').ToDictionary(kvp => kvp.Key, _ => new Creature { type = CreatureType.Goblin });
                var elves = data.Where(kvp => kvp.Value == 'E').ToDictionary(kvp => kvp.Key, _ => new Creature { type = CreatureType.Elf, AP = elfAP });

                Creatures = gobins.Union(elves).ToSortedDictionary(this);
            }

            readonly HashSet<(int x, int y)> Walls;
            readonly SortedDictionary<(int x, int y), Creature> Creatures;

            public int ElfCount() => Creatures.Values.Count(c => c.type == CreatureType.Elf);

            bool IsClear((int x, int y) pos)
            {
                return !Walls.Contains(pos) && !Creatures.ContainsKey(pos);
            }

            (bool hasTarget, (int x, int y) newPos) MoveUnit(KeyValuePair<(int x, int y), Creature> creature)
            {
                var targets = Creatures.Where(c => c.Value.type != creature.Value.type).ToDictionary();

                if (!targets.Any()) return (false, (0, 0));

                var target = FindTarget(creature, targets);

                if (target != creature.Key)
                {
                    Creatures.Remove(creature.Key);
                    Creatures.Add(target, creature.Value);
                }
                return (true, target);
            }

            (int x, int y) FindTarget(KeyValuePair<(int x, int y), Creature> creature, Dictionary<(int x, int y), Creature> targets)
            {
                foreach (var (dx, dy) in InRange)
                {
                    var pos = (x: creature.Key.x + dx, y: creature.Key.y + dy);
                    if (targets.TryGetValue(pos, out var potential))
                    {
                        return (creature.Key);
                    }
                }

                var potentialTargets = targets.SelectMany(target => InRange.Select(offset => (x: target.Key.x + offset.dx, y: target.Key.y + offset.dy))).Where(pos => IsClear(pos) || pos == creature.Key).Distinct();

                var reachable = potentialTargets.Select(pos => (pos, path: AStar<(int x, int y)>.FindPath(this, creature.Key, pos)))
                                                 .Where(entry => entry.path.Any());                                                                                                  

                if (!reachable.Any()) return creature.Key;

                var targetPos = reachable.OrderBy(v => (v.path.Count(), v.pos.y, v.pos.x))
                                         .First();

                //return targetPos.path.First();

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
                var moveOrder = Creatures.ToArray();

                foreach (var creature in moveOrder)
                {
                    if (creature.Value.HP <= 0) continue;

                    var (hasTarget, (x,y)) = MoveUnit(creature);
                    if (!hasTarget) return false;

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
                        var target = targets.OrderBy(t => (t.creature.HP, t.pos.y, t.pos.x)).First();

                        target.creature.HP -= creature.Value.AP;

                        if (target.creature.HP <= 0)
                        {
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

            public int Score() { return Creatures.Sum(c => c.Value.HP); }

            public int Compare((int x, int y) a, (int x, int y) b)
            {
                int result = a.y.CompareTo(b.y);
                if (result == 0)
                    result = a.x.CompareTo(b.x);
                return result;
            }
        }


        private static int ElfBattle(string input, int elfAP, bool endOnElfLoss)
        {
            var map = new Map(input, elfAP);
            int initialElves = map.ElfCount();

            int turns = 0;

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
                var score = ElfBattle(input, elfAP, true);
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
