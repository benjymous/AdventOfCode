namespace AoC.Advent2017;
public class Day25 : IPuzzle
{
    static int ToKey(char c) => c - 'A';

    enum Action
    {
        Set,
        Clear,
        Skip,
    }
    class State
    {
        public readonly (Action action, int move, int next)[] Conditions = new (Action, int, int)[2];

        [Regex(@"In state .:\n.+If the current value is 0:\n.+- Write the value (.).\n.+- Move one slot to the (.).+\.\n.+- Continue with state (.).\n.+If the current value is 1:\n.+- Write the value (.).\n.+- Move one slot to the (.).+\.\n    - Continue with state (.).")]
        public State(int write0, char move0, char continue0, int write1, char move1, char continue1)
        {
            Conditions[0] = (write0 == 0 ? Action.Skip : Action.Set, move0 == 'r' ? 1 : -1, ToKey(continue0));
            Conditions[1] = (write1 == 1 ? Action.Skip : Action.Clear, move1 == 'r' ? 1 : -1, ToKey(continue1));
        }
    }

    class Program
    {

        public Program(string input)
        {
            var parts = input.SplitSections();
            Start = ToKey(parts[0][15]);
            Diagnostic = Util.ExtractNumbers(parts[0])[0];
            States = Util.RegexParse<State>(parts.Skip(1)).ToArray();
        }

        public int Run()
        {
            HashSet<int> tape = [];
            State state = States[Start];
            int tapePosition = 0;
            for (int cycle = 0; cycle < Diagnostic; ++cycle)
            {
                var (action, move, next) = state.Conditions[tape.Contains(tapePosition) ? 1 : 0];

                state = States[next];

                switch (action)
                {
                    case Action.Set:
                        tape.Add(tapePosition);
                        break;
                    case Action.Clear:
                        tape.Remove(tapePosition);
                        break;
                }
                tapePosition += move;

            }
            return tape.Count;
        }

        readonly int Start = 0;
        readonly int Diagnostic = 0;
        readonly State[] States;
    }

    public static int Part1(string input) => new Program(input).Run();

    public void Run(string input, ILogger logger) => logger.WriteLine("- Pt1 - " + Part1(input));
}