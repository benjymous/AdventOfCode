namespace AoC.Advent2017;
public class Day25 : IPuzzle
{
    static int ToKey(char c) => c - 'A';

    class Program
    {
        public Program(string input) => Parser.Factory(input.SplitSections(), this);

        int Start = 0, Diagnostic = 0, TapePosition = 0;
        readonly List<(Action action, int move, int next)[]> States = [];
        readonly HashSet<int> Tape = [];

        [Regex(@"Begin in state (.).\nPerform a diagnostic checksum after (\d+) steps.")]
        public void Initialiser(char start, int diagnostic) => (Start, Diagnostic) = (ToKey(start), diagnostic);

        [Regex(@"In state .:\n.+If the current value is 0:\n.+- Write the value (.).\n.+- Move one slot to the (.).+\.\n.+- Continue with state (.).\n.+If the current value is 1:\n.+- Write the value (.).\n.+- Move one slot to the (.).+\.\n    - Continue with state (.).")]
        public void State(int write0, char move0, char continue0, int write1, char move1, char continue1)
            => States.Add([
                   (write0 == 0 ? () => { } : () => Tape.Add(TapePosition), move0 == 'r' ? 1 : -1, ToKey(continue0)),
                   (write1 == 1 ? () => { } : () => Tape.Remove(TapePosition), move1 == 'r' ? 1 : -1, ToKey(continue1))
               ]);

        public int Run()
        {
            var state = States[Start];
            for (int cycle = 0; cycle < Diagnostic; ++cycle)
            {
                var (action, move, next) = state[Tape.Contains(TapePosition) ? 1 : 0];

                action();
                TapePosition += move;
                state = States[next];

            }
            return Tape.Count;
        }
    }

    public static int Part1(string input) => new Program(input).Run();

    public void Run(string input, ILogger logger) => logger.WriteLine("- Pt1 - " + Part1(input));
}