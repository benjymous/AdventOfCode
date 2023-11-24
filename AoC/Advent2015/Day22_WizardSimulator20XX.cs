namespace AoC.Advent2015;
public class Day22 : IPuzzle
{
    static readonly List<(int mana, int duration, Action<State> cast, Action<State, int> tick)> Spells = new()
    { 
        /* Magic Missile */ { ( 53,0, (State s) => s.BossHP -= 4,                       (State s, int duration) => { }) }, 
        /* Drain         */ { ( 73,0, (State s) => { s.BossHP -= 2; s.PlayerHP += 2; }, (State s, int duration) => { }) }, 
        /* Shield        */ { (113,6, (State s) => s.PlayerArmour += 7,                 (State s, int duration) => { if (duration == 0) s.PlayerArmour -= 7;}) }, 
        /* Poison        */ { (173,6, (State s) => { },                                 (State s, int duration) => s.BossHP -= 3) }, 
        /* Recharge      */ { (229,5, (State s) => { },                                 (State s, int duration) => s.PlayerMana += 101) }
    };

    public class State
    {
        [Regex(@"Hit Points: (\d+)\nDamage: (\d+)")]
        public State(int bossHP, int bossDamage) => (BossHP, BossDamage) = (bossHP, bossDamage);

        State(State state, (int mana, int duration, Action<State> cast, Action<State, int> tick) spell)
        {
            (PlayerHP, PlayerArmour, PlayerMana, BossHP, BossDamage, ManaSpend, PlayerTurn, ActiveEffects) = (state.PlayerHP, state.PlayerArmour, state.PlayerMana - spell.mana, state.BossHP, state.BossDamage, state.ManaSpend + spell.mana, !state.PlayerTurn, new Dictionary<int, (Action<State, int> effect, int remaining)>(state.ActiveEffects).Plus(spell.mana, (spell.tick, spell.duration)));
            spell.cast(this);
        }

        public int PlayerHP = 50, PlayerArmour = 0, PlayerMana = 500, ManaSpend = 0, BossHP, BossDamage;

        bool PlayerTurn = true;

        public Dictionary<int, (Action<State, int> effect, int remaining)> ActiveEffects = [];

        public int Priority => BossHP + ManaSpend;

        public IEnumerable<(State, int)> Tick(bool hardMode)
        {
            foreach (var (key, effect, remaining) in ActiveEffects.Select(kvp => (kvp.Key, kvp.Value.effect, kvp.Value.remaining - 1)))
            {
                effect(this, remaining);

                if (remaining > 0) ActiveEffects[key] = (effect, remaining);
                else ActiveEffects.Remove(key);
            }

            if (PlayerTurn)
            {
                if (hardMode) PlayerHP--;
                if (PlayerHP > 0)
                    foreach (var newState in Spells.Where(spell => spell.mana <= PlayerMana && !ActiveEffects.ContainsKey(spell.mana)).Select(s => new State(this, s)))
                        yield return (newState, newState.Priority);
            }
            else if (BossHP > 0)
            {
                PlayerHP -= Math.Max(0, BossDamage - PlayerArmour);
                PlayerTurn = !PlayerTurn;
                yield return (this, Priority);
            }
        }
    }

    public static int Run(State initialState, bool hardMode = false)
    {
        return Solver<State, int>.Solve(initialState, (state, solver) =>
        {
            if (solver.IsBetterThanSeen((state.PlayerMana, state.BossHP), state.ManaSpend))
            {
                var nextStates = state.Tick(hardMode).ToArray();

                if (state.BossHP <= 0) return state.ManaSpend;
                else if (state.PlayerHP > 0) solver.EnqueueRange(nextStates);
            }

            return default;
        }, Math.Min);
    }

    public static int Part1(string input) => Run(Util.FromString<State>(input));

    public static int Part2(string input) => Run(Util.FromString<State>(input), true);

    public void Run(string input, ILogger logger)
    {
        logger.WriteLine("- Pt1 - " + Part1(input));
        logger.WriteLine("- Pt2 - " + Part2(input));
    }
}