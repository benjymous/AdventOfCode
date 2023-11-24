namespace AoC.Advent2018;
public class Day24 : IPuzzle
{
    public class Group
    {
        [Regex(@"(\d+) units each with (\d+) hit points with an attack that does (\d+) (.).+ damage at initiative (\d+)")]
        public Group(uint unitCount, uint hitPoints, uint attackDamage, char damageType, uint initiative) => (UnitCount, HP, AttackDamage, AttackType, Initiative) = (unitCount, hitPoints, attackDamage, damageType, initiative);

        [Regex(@"(\d+) units each with (\d+) hit points \(?(.+)?\)? with an attack that does (\d+) (.).+ damage at initiative (\d+)")]
        public Group(uint unitCount, uint hitPoints, string attributes, uint attackDamage, char damageType, uint initiative)
            : this(unitCount, hitPoints, attackDamage, damageType, initiative)
        {
            var bits = attributes.Replace(",", "").Split(';', StringSplitOptions.TrimEntries);
            foreach (var bit in bits)
            {
                var bits2 = bit.Split(' ');
                ((bits2[0] == "weak") ? Weak : Immune).UnionWith(bits2.Skip(2).Select(attr => attr[0]).ToHashSet());
            }
        }

        public bool ImmuneSystem = true;
        public int Id = 0;
        public Group target = null;

        public uint UnitCount, HP, AttackDamage, Initiative;
        public char AttackType;
        public uint EffectivePower => UnitCount * AttackDamage;

        readonly HashSet<char> Weak = [], Immune = [];

        public uint EstimateDamage(Group attacker) => Immune.Contains(attacker.AttackType) ? 0 : (attacker.EffectivePower * (Weak.Contains(attacker.AttackType) ? 2u : 1u));

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
            int targetted = 0;
            foreach (var g in groups.OrderByDescending(g => g.EffectivePower))
            {
                g.target = groups.Where(g2 => ((targetted & g2.Id) == 0) && g2.ImmuneSystem != g.ImmuneSystem).Select(g2 => (group: g2, damage: g2.EstimateDamage(g))).Where(res => res.damage > 0).OrderByDescending(res => (res.damage, res.group.EffectivePower, res.group.Initiative)).Select(t => t.group).FirstOrDefault();
                if (g.target != null) targetted |= g.target.Id;
            }

            if (groups.Sum(g => g.DoAttack()) == 0) break;

            groups = groups.Where(g => g.UnitCount > 0).ToArray();
        }

        return (groups.All(g => g.ImmuneSystem), groups.Sum(g => g.UnitCount));
    }

    private static IEnumerable<Group> Parse(string input, uint boost)
    {
        var (immune, infection) = input.Split("\n\n").Select(data => Util.RegexParse<Group>(data.Split("\n", StringSplitOptions.TrimEntries).Skip(1)).ToArray()).Decompose2();

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