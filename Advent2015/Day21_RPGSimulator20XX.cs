using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day21 : IPuzzle
    {
        public string Name => "2015-21";

        class Item
        {
            public readonly int Id;
            public readonly string Type;
            public readonly string Name;
            public readonly int Damage;
            public readonly int Armour;
            public readonly int Cost;

            public Item(string line)
            {
                var items = Util.Split(line, ' ');
                Id = 1 << int.Parse(items[0]) - 1;
                Type = items[1];
                Name = items[2];
                Damage = int.Parse(items[4]);
                Armour = int.Parse(items[5]);
                Cost = int.Parse(items[3]);
            }

            public override string ToString() => Name;
        }

        static readonly string shopStr =
            //Weapons:        Cost  Damage  Armor
            " 1 W Dagger        8     4       0\n" +
            " 2 W Shortsword   10     5       0\n" +
            " 3 W Warhammer    25     6       0\n" +
            " 4 W Longsword    40     7       0\n" +
            " 5 W Greataxe     74     8       0\n" +

            //Armor:          Cost  Damage  Armor
            " 6 A Leather      13     0       1\n" +
            " 7 A Chainmail    31     0       2\n" +
            " 8 A Splintmail   53     0       3\n" +
            " 9 A Bandedmail   75     0       4\n" +
            "10 A Platemail   102     0       5\n" +

            //Rings:          Cost  Damage  Armor           
            "11 R Damage+1     25     1       0\n" +
            "12 R Damage+2     50     2       0\n" +
            "13 R Damage+3    100     3       0\n" +
            "14 R Defense+1    20     0       1\n" +
            "15 R Defense+2    40     0       2\n" +
            "16 R Defense+3    80     0       3\n";

        static readonly List<Item> shopItems = Util.Parse<Item>(shopStr);
        static IEnumerable<Item> Weapons => shopItems.Where(i => i.Type == "W").ToArray();
        static IEnumerable<Item> Armour => shopItems.Where(i => i.Type == "A").ToArray();
        static IEnumerable<Item> Rings => shopItems.Where(i => i.Type == "R").ToArray();

        class Entity
        {
            public string Name;
            int HP;
            readonly int BaseDamage;
            readonly int BaseArmour;

            int damage = 0;
            int armour = 0;

            Item[] items = Array.Empty<Item>();

            public Entity(string name, int hp, int dam, int armour)
            {
                Name = name;
                HP = hp;
                BaseDamage = dam;
                BaseArmour = armour;

                UpdateStats();
            }

            public void SetInventory(Item[] newItems)
            {
                items = newItems;
                UpdateStats();
            }

            void UpdateStats()
            {
                damage = BaseDamage + items.Sum(x => x.Damage);
                armour = BaseArmour + items.Sum(x => x.Armour);
            }

            public void Hit(int damage) => HP = Math.Max(0, HP - damage);

            public void Attack(Entity other) => other.Hit(Math.Max(1, damage - other.armour));

            public override string ToString() => Name;

            public bool Dead => HP == 0;
        }

        static IEnumerable<Item[]> GetInventoryCombinations(int gold)
        {
            foreach (var weapon in Weapons)
            {
                if (weapon.Cost > gold) continue;

                yield return new[] { weapon };

                if ((gold - weapon.Cost) > 0)
                {
                    foreach (var armour in Armour)
                    {
                        if (weapon.Cost + armour.Cost > gold) continue;
                        yield return new[] { weapon, armour };

                        foreach (var ring1 in Rings)
                        {
                            if (weapon.Cost + armour.Cost + ring1.Cost > gold) continue;
                            yield return new[] { weapon, armour, ring1 };

                            foreach (var ring2 in Rings.Where(r => r != ring1))
                            {
                                if (weapon.Cost + armour.Cost + ring1.Cost + ring2.Cost > gold) continue;

                                yield return new[] { weapon, armour, ring1, ring2 };
                            }
                        }
                    }
                }
            }
        }

        static Entity Fight(Entity e1, Entity e2)
        {
            while (true)
            {
                e1.Attack(e2);
                if (e2.Dead) return e1;
                (e1, e2) = (e2, e1);
            }
        }

        public static int Part1(string input)
        {
            var vals = Util.ExtractNumbers(input);

            int gold = Weapons.Select(w => w.Cost).Min();

            HashSet<int> triedCombos = new();

            while (true)
            {
                var itemCombos = GetInventoryCombinations(gold);

                foreach (var combo in itemCombos)
                {
                    var key = combo.Sum(i => i.Id);
                    if (triedCombos.Contains(key)) continue;
                    triedCombos.Add(key);

                    var enemy = new Entity("Enemy", vals[0], vals[1], vals[2]);
                    var player = new Entity("Player", 100, 0, 0);

                    player.SetInventory(combo);

                    if (Fight(player, enemy) == player) return gold;
                }
                gold++;
            }
        }

        public static int Part2(string input)
        {
            var vals = Util.ExtractNumbers(input);

            int maxgold = Weapons.Max(w => w.Cost) + Armour.Max(a => a.Cost) + (2 * Rings.Max(r => r.Cost));

            HashSet<int> triedCombos = new();

            var itemCombos = GetInventoryCombinations(maxgold).Select(combo => (items: combo, cost: combo.Sum(i => i.Cost))).OrderByDescending(tup => tup.cost);

            foreach (var (items, gold) in itemCombos)
            {
                var key = items.Sum(i => i.Id);
                if (triedCombos.Contains(key)) continue;
                triedCombos.Add(key);

                var enemy = new Entity("Enemy", vals[0], vals[1], vals[2]);
                var player = new Entity("Player", 100, 0, 0);

                player.SetInventory(items);

                if (Fight(player, enemy) == enemy) return gold;
            }

            return 0;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}