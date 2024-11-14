namespace AoC.Advent2015;
public class Day21 : IPuzzle
{
    [Regex(@"^\s?(\d+) .+\s+(\d+)\s+(\d)\s+(\d+)$")]
    record class Item(int Id, int Cost, int Damage, int Armour);

    static readonly Item[] Weapons = Util.RegexParse<Item>("0 Dagger 8 4 0\n 1 Shortsword 10 5 0\n 2 Warhammer 25 6 0\n 3 Longsword 40 7 0\n 4 Greataxe 74 8 0").ToArray();
    static readonly Item[] Armour = Util.RegexParse<Item>("5 Leather 13 0 1\n 6 Chainmail 31 0 2\n 7 Splintmail 53 0 3\n 8 Bandedmail 75 0 4\n 9 Platemail 102 0 5\n").ToArray();
    static readonly Item[] Rings = Util.RegexParse<Item>("10 Damage+1 25 1 0\n 11 Damage+2 50 2 0\n 12 Damage+3 100 3 0\n 13 Defense+1 20 0 1\n 14 Defense+2 40 0 2\n 15 Defense+3 80 0 3\n").ToArray();

    static int MakeId(params Item[] items) => items.Sum(item => 1 << item.Id);

    class Entity(bool isPlayer, int hp, int dmg, int armour)
    {
        public readonly bool IsPlayer = isPlayer;
        public int HP = hp;

        public readonly int Damage = dmg, Armour = armour;
    }

    static IEnumerable<(int id, int damage, int armour, int cost)> GetInventoryCombinations(int gold)
    {
        foreach (var weapon in Weapons.Where(w => w.Cost <= gold))
        {
            foreach (var armour in Armour.Where(a => weapon.Cost + a.Cost <= gold))
            {
                foreach (var ring1 in Rings.Where(r => weapon.Cost + armour.Cost + r.Cost <= gold))
                {
                    foreach (var ring2 in Rings.Where(r => r != ring1 && weapon.Cost + armour.Cost + ring1.Cost + r.Cost <= gold))
                    {
                        yield return (MakeId(weapon, armour, ring1, ring2), weapon.Damage + ring1.Damage + ring2.Damage, armour.Armour + ring1.Armour + ring2.Armour, weapon.Cost + armour.Cost + ring1.Cost + ring2.Cost);
                    }

                    yield return (MakeId(weapon, armour, ring1), weapon.Damage + ring1.Damage, armour.Armour + ring1.Armour, weapon.Cost + armour.Cost + ring1.Cost);
                }
                yield return (MakeId(weapon, armour), weapon.Damage, armour.Armour, weapon.Cost + armour.Cost);
            }

            yield return (MakeId(weapon), weapon.Damage, 0, weapon.Cost);
        }
    }

    static bool DoFight(int enemyHp, int enemyDmg, int enemyArmour, int itemDmg, int itemArmour)
    {
        var (attacker, defender) = (new Entity(true, 100, itemDmg, itemArmour), new Entity(false, enemyHp, enemyDmg, enemyArmour));

        while (true)
        {
            defender.HP -= Math.Max(1, attacker.Damage - defender.Armour);
            if (defender.HP <= 0) return attacker.IsPlayer;
            (attacker, defender) = (defender, attacker);
        }
    }

    public static int Part1(string input)
    {
        var (enemyHp, enemyDmg, enemyArmour) = Util.ExtractNumbers(input).Decompose3();

        int gold = Weapons.Select(w => w.Cost).Min();

        HashSet<int> triedCombos = [];

        while (true)
        {
            foreach (var (id, damage, armour, cost) in GetInventoryCombinations(gold).Where(combo => !triedCombos.Contains(combo.id)))
            {
                if (DoFight(enemyHp, enemyDmg, enemyArmour, damage, armour)) return gold;
                triedCombos.Add(id);
            }
            gold++;
        }
    }

    public static int Part2(string input)
    {
        var (enemyHp, enemyDmg, enemyArmour) = Util.ExtractNumbers(input).Decompose3();

        int maxgold = Weapons.Max(w => w.Cost) + Armour.Max(a => a.Cost) + (2 * Rings.Max(r => r.Cost));
        var itemCombos = GetInventoryCombinations(maxgold).OrderByDescending(tup => tup.cost);
        return itemCombos.First(combo => DoFight(enemyHp, enemyDmg, enemyArmour, combo.damage, combo.armour) == false).cost;
    }

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}