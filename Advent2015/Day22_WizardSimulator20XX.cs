using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2015
{
    public class Day22 : IPuzzle
    {
        public string Name => "2015-22";

        public enum Spell
        {
            MagicMissile,
            Drain,
            Shield,
            Poison,
            Recharge,
        }

        static readonly Dictionary<Spell, (int mana, int duration)> Spells = new()
        {
            { Spell.MagicMissile,  (53,0) },
            { Spell.Drain,  (73,0) },
            { Spell.Shield,  (113,6) },
            { Spell.Poison,  (173,6) },
            { Spell.Recharge, (229,5) }
        };

        public class State
        {
            [Regex(@"Hit Points: (\d+)\nDamage: (\d+)")]
            public State(int bossHP, int bossDamage)
            {
                (BossHP, BossDamage) = (bossHP, bossDamage);
                PlayerHP = 50;
                PlayerArmour = 0;
                PlayerMana = 500;
            }

            public State(State state)
            {
                PlayerHP = state.PlayerHP;
                PlayerArmour = state.PlayerArmour;
                PlayerMana = state.PlayerMana;
                BossHP = state.BossHP;
                BossDamage = state.BossDamage;
                ManaSpend = state.ManaSpend;
                PlayerTurn = !state.PlayerTurn;
                HardMode = state.HardMode;

                ActiveEffects = new(state.ActiveEffects);
            }

            public int PlayerHP;
            public int PlayerArmour;
            public int PlayerMana;

            public int BossHP;
            public int BossDamage;

            public int ManaSpend = 0;

            public bool PlayerTurn = true;
            public bool HardMode = false;

            public List<(Spell effect, int duration)> ActiveEffects = new();

            public IEnumerable<State> Tick()
            {
                foreach (var (effect, duration) in ActiveEffects)
                {
                    switch (effect)
                    {
                        case Spell.Poison:
                            BossHP -= 3;
                            break;
                        case Spell.Shield:
                            if (duration == 1)
                            {
                                PlayerArmour -= 7;
                            }
                            break;
                        case Spell.Recharge:
                            PlayerMana += 101;
                            break;
                    }
                }
                ActiveEffects = ActiveEffects.Select(kvp => (kvp.effect, kvp.duration - 1)).Where(kvp => kvp.Item2 > 0).ToList();

                if (PlayerTurn)
                {
                    if (HardMode) PlayerHP--;

                    if (PlayerHP > 0)
                    {
                        foreach (var spell in Spells)
                        {
                            if (spell.Value.mana > PlayerMana) continue;
                            if (ActiveEffects.Any(e => e.effect == spell.Key)) continue;

                            var newState = new State(this);

                            switch (spell.Key)
                            {
                                case Spell.MagicMissile:
                                    newState.BossHP -= 4;
                                    break;

                                case Spell.Drain:
                                    newState.BossHP -= 2;
                                    newState.PlayerHP += 2;
                                    break;

                                case Spell.Shield:
                                    newState.PlayerArmour += 7;
                                    break;
                            }

                            if (spell.Value.duration > 0)
                            {
                                newState.ActiveEffects.Add((spell.Key, spell.Value.duration));
                            }
                            newState.PlayerMana -= spell.Value.mana;
                            newState.ManaSpend += spell.Value.mana;

                            yield return newState;
                        }
                    }
                }
                else
                {
                    if (BossHP > 0)
                    {
                        int attack = Math.Max(0, BossDamage - PlayerArmour);
                        PlayerHP -= attack;
                        PlayerTurn = !PlayerTurn;
                        yield return this;
                    }
                }             
            }
        }

        public static int Run(State initialState)
        {
            PriorityQueue<State, int> queue = new();
            queue.Enqueue(initialState, 0);

            int bestScore = int.MaxValue;

            Dictionary<(int PlayerHP, int PlayerArmour, int PlayerMana, int BossHP, int BossDamage), int> cache = new();

            while (queue.TryDequeue(out var state, out var _))
            {
                if (state.ManaSpend >= bestScore) continue;
                if (cache.TryGetValue((state.PlayerHP, state.PlayerArmour, state.PlayerMana, state.BossHP, state.BossDamage), out int prev) && prev <= state.ManaSpend) continue;
                cache[(state.PlayerHP, state.PlayerArmour, state.PlayerMana, state.BossHP, state.BossDamage)] = state.ManaSpend;

                var nextStates = state.Tick().ToArray();

                if (state.BossHP <= 0)
                {
                    bestScore = Math.Min(bestScore, state.ManaSpend);
                    continue;
                }

                if (state.PlayerHP > 0)
                {
                    foreach (var nextState in nextStates)
                    {
                        queue.Enqueue(nextState, nextState.BossHP + nextState.ManaSpend);
                    }
                }
            }

            return bestScore;
        }

        public static int Part1(string input)
        {
            var state = Util.RegexCreate<State>(input);

            return Run(state);
        }

        public static int Part2(string input)
        {
            var state = Util.RegexCreate<State>(input);
            state.HardMode = true;

            return Run(state);
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}