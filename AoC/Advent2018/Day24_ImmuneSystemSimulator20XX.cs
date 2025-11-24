namespace AoC.Advent2018;
public class Day24 : IPuzzle
{
    public enum AttrType
    {
        weak,
        immune
    }
    [Regex(@"(weak|immune) to (.+)")]
    public class Attribute(AttrType type, [Split(", ")] string[] elements)
    {
        public AttrType Type = type;
        public int Elements = elements.Sum(e => 1 << e[0]-'a');
    }
    [Regex(@"(\d+) units each with (\d+) hit points ?\(?(.+)?\)? with an attack that does (\d+) (.).+ damage at initiative (\d+)")]
    public class Group(uint unitCount, uint hitPoints, [Split("; ")] List<Attribute> attributes, uint attackDamage, char damageType, uint initiative)
    {
        public bool ImmuneSystem = true;
        public int Id = 0;
        public Group target = null;

        public uint UnitCount = unitCount, HP = hitPoints, AttackDamage = attackDamage, Initiative = initiative;
        public int AttackType = 1 << damageType-'a';
        public uint EffectivePower => UnitCount * AttackDamage;

        private readonly Dictionary<AttrType, int> Attributes = attributes.ToDictionary(a => a.Type, a => a.Elements);
        public bool CheckAffinity(AttrType type, int attr) => Attributes.TryGetValue(type, out var hash) && (hash & attr)!=0;

        public uint EstimateDamage(Group attacker) => CheckAffinity(AttrType.immune, attacker.AttackType) ? 0 : (attacker.EffectivePower * (CheckAffinity(AttrType.weak, attacker.AttackType) ? 2u : 1u));

        public uint DoAttack()
        {
            if (target == null) return 0;
            uint killed = Math.Min(target.EstimateDamage(this) / target.HP, target.UnitCount);
            target.UnitCount -= killed;
            return killed;
        }

        public override int GetHashCode() => Id;
    }

    private static (bool win, long res) Run(string input, uint boost = 0)
    {
        var groups = Parse(input, boost).OrderByDescending(g => g.Initiative).ToArray();

        while (true)
        {
            int targeted = 0;
            foreach (var g in groups.OrderByDescending(g => g.EffectivePower))
            {
                g.target = groups.Where(g2 => ((targeted & g2.Id) == 0) && g2.ImmuneSystem != g.ImmuneSystem).Select(g2 => (group: g2, damage: g2.EstimateDamage(g))).Where(res => res.damage > 0).OrderByDescending(res => (res.damage, res.group.EffectivePower, res.group.Initiative)).Select(t => t.group).FirstOrDefault();
                if (g.target != null) targeted |= g.target.Id;
            }

            if (groups.Sum(g => g.DoAttack()) == 0) break;

            groups = [.. groups.Where(g => g.UnitCount > 0)];
        }

        return (groups.All(g => g.ImmuneSystem), groups.Sum(g => g.UnitCount));
    }

    private static IEnumerable<Group> Parse(string input, uint boost)
    {
        var (immune, infection) = input.SplitSections().Select(data => Parser.Parse<Group>(data.Split("\n", StringSplitOptions.TrimEntries).Skip(1)).ToArray()).Decompose2();

        immune.ForEach(g => g.AttackDamage += boost);
        infection.ForEach(g => g.ImmuneSystem = false);

        return immune.Concat(infection).Select((g, id) => { g.Id = 1 << id; return g; });
    }

    public static long Part1(string input) => Run(input).res;

    public static long Part2(string input) => Util.BinarySearch<uint, long>(1, boost => Run(input, boost)).result;

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}