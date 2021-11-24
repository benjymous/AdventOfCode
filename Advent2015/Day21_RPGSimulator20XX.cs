using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day21 : IPuzzle
    {
        public string Name { get { return "2015-21"; } }

        class Item
        {
            public string Type;
            public string Name;
            public int Dam;
            public int Armour;
            public int Cost;

            public Item(string line)
            {
                var items = Util.Split(line, ' ');
                Type = items[0];
                Name = items[1];
                Dam = int.Parse(items[3]);
                Armour = int.Parse(items[4]);
                Cost = int.Parse(items[2]);
            }

            public override string ToString() => Name;
        }

        static string shopStr =
            //Weapons:    Cost  Damage  Armor
            "W Dagger        8     4       0\n" +
            "W Shortsword   10     5       0\n" +
            "W Warhammer    25     6       0\n" +
            "W Longsword    40     7       0\n" +
            "W Greataxe     74     8       0\n" +

            //Armor:      Cost  Damage  Armor
            "A None          0     0       0\n" +
            "A Leather      13     0       1\n" +
            "A Chainmail    31     0       2\n" +
            "A Splintmail   53     0       3\n" +
            "A Bandedmail   75     0       4\n" +
            "A Platemail   102     0       5\n" +

            //Rings:      Cost  Damage  Armor
            "R None          0     0       0\n" +
            "R None          0     0       0\n" +
            "R Damage+1     25     1       0\n" +
            "R Damage+2     50     2       0\n" +
            "R Damage+3    100     3       0\n" +
            "R Defense+1    20     0       1\n" +
            "R Defense+2    40     0       2\n" +
            "R Defense+3    80     0       3\n";

        static List<Item> shopItems = Util.Parse<Item>(shopStr);
        static IEnumerable<Item> Weapons() => shopItems.Where(i => i.Type == "W");
        static IEnumerable<Item> Armour() => shopItems.Where(i => i.Type == "A");
        static IEnumerable<Item> Rings() => shopItems.Where(i => i.Type == "R");

        class Entity
        {
            public string Name;
            int HP;
            int BaseDamage;
            int BaseArmour;

            int damage = 0;
            int armour = 0;

            List<Item> items = new List<Item>();

            public Entity(string name, int hp, int dam, int armour)
            {
                Name = name;
                HP = hp;
                BaseDamage = dam;
                BaseArmour = armour;

                UpdateStats();
            }

            public void SetInventory(List<Item> newItems)
            {
                items = newItems;
                UpdateStats();
            }


            void UpdateStats()
            {
                damage = BaseDamage + items.Select(x => x.Dam).Sum();
                armour = BaseArmour + items.Select(x => x.Armour).Sum();
            }

            public void Hit(int damage)
            {
                HP = Math.Max(0, HP - damage);
            }

            public void Attack(Entity other)
            {
                int attack = Math.Max(1, damage - other.armour);
                other.Hit(attack);

                //Console.WriteLine($"{Name} deals {attack} to {other}.  {other} has {other.HP} HP");
            }

            public override string ToString() => Name;

            public bool Dead { get { return HP == 0; } }
        }

        static IEnumerable<List<Item>> GetInventoryCombinations(int gold)
        {
            foreach (var weapon in Weapons())
            {
                if (weapon.Cost > gold) continue;

                List<Item> justWeapon = new List<Item> { weapon };

                yield return justWeapon;

                if ((gold - weapon.Cost) > 0)
                {
                    foreach (var armour in Armour())
                    {

                        if (weapon.Cost + armour.Cost > gold) continue;
                        List<Item> weaponAndArmour = new List<Item> { weapon, armour };
                        yield return weaponAndArmour;

                        foreach (var ring1 in Rings())
                        {
                            foreach (var ring2 in Rings().Where(r => r != ring1))
                            {
                                if (weapon.Cost + armour.Cost + ring1.Cost + ring2.Cost > gold) continue;

                                List<Item> weaponAndArmourAndRings = new List<Item> { weapon, armour, ring1, ring2 };
                                yield return weaponAndArmourAndRings;
                            }
                        }
                    }
                }
            }
        }



        static Entity Fight(Entity e1, Entity e2)
        {
            Queue<Entity> pattern = new Queue<Entity>();
            pattern.Enqueue(e1);
            pattern.Enqueue(e2);

            while (true)
            {
                var x = pattern.Dequeue();
                var y = pattern.Dequeue();

                x.Attack(y);
                if (y.Dead)
                {
                    //Console.WriteLine($"{y} dies");
                    return x;
                }

                pattern.Enqueue(y);
                pattern.Enqueue(x);
            }
        }

        public static int Part1(string input)
        {
            var vals = Util.ExtractNumbers(input);

            int gold = Weapons().Select(w => w.Cost).Min();

            HashSet<string> triedCombos = new HashSet<string>();

            while (true)
            {
                var itemCombos = GetInventoryCombinations(gold);

                foreach (var combo in itemCombos)
                {
                    var key = string.Join(", ", combo.OrderBy(i => i.Name).Where(i => i.Name != "None"));
                    if (triedCombos.Contains(key)) continue;
                    triedCombos.Add(key);

                    var enemy = new Entity("Enemy", vals[0], vals[1], vals[2]);
                    var player = new Entity("Player", 100, 0, 0);

                    //Console.WriteLine($"Bought {key} with {gold} gold");

                    player.SetInventory(combo);

                    if (Fight(player, enemy) == player) return gold;

                    //Console.WriteLine();
                }
                gold++;
            }
        }

        public static int Part2(string input)
        {
            var vals = Util.ExtractNumbers(input);

            int maxgold = Weapons().Select(w => w.Cost).Max() + Armour().Select(a => a.Cost).Max() + (2 * Rings().Select(r => r.Cost).Max());

            HashSet<string> triedCombos = new HashSet<string>();

            var itemCombos = GetInventoryCombinations(maxgold).Select(combo => (combo, combo.Select(i => i.Cost).Sum())).OrderByDescending(tup => tup.Item2);

            foreach (var combo in itemCombos)
            {

                var key = string.Join(", ", combo.Item1.OrderBy(i => i.Name).Where(i => i.Name != "None"));
                var gold = combo.Item2;
                if (triedCombos.Contains(key)) continue;
                triedCombos.Add(key);

                var enemy = new Entity("Enemy", vals[0], vals[1], vals[2]);
                var player = new Entity("Player", 100, 0, 0);

                //Console.WriteLine($"Bought {key} with {gold} gold");

                player.SetInventory(combo.Item1);

                if (Fight(player, enemy) == enemy) return gold;

                //Console.WriteLine();
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