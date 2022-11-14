using AoC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC.Advent2018
{
    public class Day24 : IPuzzle
    {
        public string Name => "2018-24";

        public enum AttackType
        {
            slashing,
            fire,
            radiation,
            bludgeoning,
            cold
        }

        public class Group
        {
            [Regex(@"(\d+) units each with (\d+) hit points \((.+)\) with an attack that does (\d+) (.+) damage at initiative (\d+)")]
            public Group(uint unitCount, uint hitPoints, string attributes, uint attackDamage, AttackType damageType, uint initiative)
            {
                UnitCount = unitCount;
                HP = hitPoints;
                AttackDamage = attackDamage;
                AttackType = damageType;
                Initiative = initiative;

                var bits = attributes.Replace(",","").Split(';', StringSplitOptions.TrimEntries);
                foreach (var bit in bits)
                {
                    var bits2 = bit.Split(' ');
                    var type = bits2[0];
                    foreach (var attr in bits2.Skip(2))
                    {
                        AttackType atype = (AttackType)Enum.Parse(typeof(AttackType), attr);

                        if (type == "weak")
                            Weak.Add(atype);
                        else
                            Immune.Add(atype);
                    }
                }
            }

            [Regex(@"(\d+) units each with (\d+) hit points with an attack that does (\d+) (.+) damage at initiative (\d+)")]
            public Group(uint unitCount, uint hitPoints, uint attackDamage, AttackType damageType, uint initiative)
            {
                UnitCount = unitCount;
                HP = hitPoints;
                AttackDamage = attackDamage;
                AttackType = damageType;
                Initiative = initiative;
            }

            public bool ImmuneSystem = true;
            public int Id = 0;

            public uint UnitCount;
            public uint HP;
            public uint AttackDamage;
            public AttackType AttackType;
            public uint Initiative;
            public uint EffectivePower => UnitCount * AttackDamage;

            readonly HashSet<AttackType> Weak = new();
            readonly HashSet<AttackType> Immune = new();

            public long EstimateDamage(Group attacker)
            {
                if (Immune.Contains(attacker.AttackType)) return 0;
                if (Weak.Contains(attacker.AttackType)) return attacker.EffectivePower * 2;
                return attacker.EffectivePower;             
            }

            public int DoAttack()
            {
                long damage = target.EstimateDamage(this);
                int killed = 0;

                while (damage >= target.HP && target.UnitCount > 0)
                {
                    target.UnitCount--;
                    damage -= target.HP;
                    killed++;
                }

                return killed;
            }

            public override string ToString()
            {
                return ImmuneSystem ? $"Immune group {Id}" : $"Infection group {Id}";
            }

            public Group target = null;
        }

        private static (bool win, long res) Run(string input, uint boost = 0)
        {
            IEnumerable<Group> groups = Parse(input, boost);

            bool haveAttacked = false;
            do
            {
                haveAttacked = false;

                HashSet<Group> targetable = groups.ToHashSet();

                var targetOrder = groups.OrderByDescending(g => (g.EffectivePower, g.Initiative));
                foreach (var g in targetOrder)
                {
                    g.target = null;
                    var enemies = targetable.Where(g2 => g2.ImmuneSystem != g.ImmuneSystem);

                    var targetDamage = enemies.Select(g2 => (g2, g2.EstimateDamage(g))).OrderByDescending(res => res.Item2);

                    if (targetDamage.Any())
                    {
                        long maxDamage = targetDamage.First().Item2;

                        if (maxDamage > 0)
                        {
                            var targets = targetDamage.Where(res => res.Item2 == maxDamage)
                                             .Select(res => res.g2)
                                             .OrderByDescending(g2 => (g2.EstimateDamage(g), g2.EffectivePower, g2.Initiative));


                            g.target = targets.FirstOrDefault();
                            targetable.Remove(g.target);
                        }
                    }
                }

                foreach (var g in groups.OrderByDescending(g => g.Initiative))
                {
                    if (g.target == null) continue;

                    if (g.DoAttack() > 0) haveAttacked = true;

                }

                groups = groups.Where(g => g.UnitCount > 0);

            } while (haveAttacked);

            return (groups.All(g => g.ImmuneSystem), groups.Sum(g => g.UnitCount));
        }

        private static IEnumerable<Group> Parse(string input, uint boost)
        {
            var parts = input.Split("\n\n");

            var immune = Util.RegexParse<Group>(parts[0].Split("\n", StringSplitOptions.TrimEntries).Skip(1));
            var infection = Util.RegexParse<Group>(parts[1].Split("\n", StringSplitOptions.TrimEntries).Skip(1));
            immune.WithIndex().ForEach(g => g.Value.Id = g.Index + 1);
            immune.ForEach(g => g.AttackDamage += boost);
            infection.WithIndex().ForEach(g => { g.Value.ImmuneSystem = false; g.Value.Id = g.Index + 1; });

            IEnumerable<Group> groups = immune.Concat(infection);
            return groups.ToArray();
        }

        public static long Part1(string input)
        {
            return Run(input).res;
        }

        public static long Part2(string input)
        {
            return Util.BinarySearch<uint, long>(1, boost =>
            {
                var res = Run(input, boost);
                Console.WriteLine($"{boost} => {res}");
                return res;
            }).result;
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));

            // 1009 too low
        }
     }
 }
