namespace AoC.Advent2018;
public class Day15 : IPuzzle
{
    enum CreatureType { Elf, Goblin };

    class Creature(CreatureType type, int ap = 3)
    {
        public CreatureType Type = type;
        public int AP = ap, HP = 200;
    }

    static readonly PackedPos32[] InRange = [(0, -1), (-1, 0), (1, 0), (0, 1)];

    class Map : IMap<PackedPos32>
    {
        public Map(string input, int elfAP)
        {
            var data = Util.ParseSparseMatrix<PackedPos32, char>(input, new Util.Convertomatic.SkipChars('#'));

            Walkable = [.. data.Keys];
            Creatures = data.Where(kvp => kvp.Value == 'G').ToDictionary(kvp => kvp.Key, _ => new Creature(CreatureType.Goblin)).Union(data.Where(kvp => kvp.Value == 'E').ToDictionary(kvp => kvp.Key, _ => new Creature(CreatureType.Elf, elfAP))).ToSortedDictionary();
        }

        readonly HashSet<PackedPos32> Walkable;
        readonly SortedDictionary<PackedPos32, Creature> Creatures;

        public int ElfCount() => Creatures.Values.Count(c => c.Type == CreatureType.Elf);

        bool IsClear(PackedPos32 pos) => !Creatures.ContainsKey(pos) && Walkable.Contains(pos);

        bool MoveUnit(KeyValuePair<PackedPos32, Creature> creature)
        {
            var enemies = Creatures.Where(c => c.Value.Type != creature.Value.Type).ToDictionary();
            if (enemies.Count == 0) return false;
            var newPos = FindTarget(creature, enemies);
            Creatures.Move(creature.Key, newPos);

            var neighbours = InRange.Select(offset => newPos + offset).ToHashSet();
            var target = Creatures.Where(other => neighbours.Contains(other.Key) && other.Value.Type != creature.Value.Type).Select(kvp => (creature: kvp.Value, pos: kvp.Key)).OrderBy(t => (t.creature.HP, t.pos)).FirstOrDefault();
            if (target != default && (target.creature.HP -= creature.Value.AP) <= 0) Creatures.Remove(target.pos);
            return true;
        }

        PackedPos32 FindTarget(KeyValuePair<PackedPos32, Creature> creature, Dictionary<PackedPos32, Creature> enemies)
        {
            if (InRange.Select(offset => creature.Key + offset).Any(enemies.ContainsKey)) return creature.Key;

            var reachable = enemies.SelectMany(target => InRange.Select(offset => target.Key + offset)).Where(pos => IsClear(pos) || pos == creature.Key).Distinct().Select(pos => (pos, path: this.FindPath(creature.Key, pos))).Where(entry => entry.path.Length != 0).OrderBy(v => (v.path.Length, v.pos)).FirstOrDefault();

            return reachable == default ? creature.Key : ReadingOrderMove(creature.Key, reachable.pos, reachable.path[0]);
        }

        PackedPos32 ReadingOrderMove(PackedPos32 pos, PackedPos32 dest, PackedPos32 next)
        {
            var v = pos.Distance(dest) == 1 ? dest : InRange.Select(offset => pos + offset).Where(IsClear).Select(newPos => (pos: newPos, dist: this.FindPath(newPos, dest).Length)).Where(v => v.dist > 0).OrderBy(v => (v.dist, v.pos)).First().pos;

            if (v != dest && v != next)
            {
                Console.WriteLine("oh!");
            }

            return v;
        }

        public bool PerformTurn() => Creatures.ToArray().Where(c => c.Value.HP > 0).All(MoveUnit);

        public IEnumerable<PackedPos32> GetNeighbours(PackedPos32 centre) => InRange.Select(offset => centre + offset).Where(IsClear);

        public int Heuristic(PackedPos32 location1, PackedPos32 location2) => location1.Distance(location2);

        public int Score() => Creatures.Sum(c => c.Value.HP);
    }

    private static int ElfBattle(string input, int elfAP, bool endOnElfLoss)
    {
        var map = new Map(input, elfAP);
        int initialElves = map.ElfCount();

        for (int turns = 0; ; ++turns)
        {
            if (!map.PerformTurn()) return map.Score() * turns;
            if (endOnElfLoss && map.ElfCount() != initialElves) return -1;
        }
    }

    public static int Part1(string input) => ElfBattle(input, 3, false);

    public static int Part2(string input)
    {
        return Util.BinarySearch(4, elfAP =>
        {
            var score = ElfBattle(input, elfAP, true);
            return (score > 0, score);
        }).result;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}