using System;
using System.Collections.Generic;

namespace AoC.Advent2017
{
    public class Day25 : IPuzzle
    {
        public string Name => "2017-25";

        static int ToKey(char c) => c - 'A';

        enum Action
        {
            Set,
            Clear,
            Skip,
        }
        class State
        {
            int Parsing = 0;

            public readonly (Action action, int move, int next)[] Conditions = new (Action, int, int)[2];

            public void Feed(string nextLine)
            {
                /*
                - Write the value 1.
                - Move one slot to the right.
                - Continue with state B.
                */

                switch (nextLine[6])
                {
                    case 'W': // Write
                        int val = nextLine[22] == '0' ? 0 : 1;
                        Conditions[Parsing].action = val == Parsing ? Action.Skip : val == 1 ? Action.Set : Action.Clear;
                        return;
                    case 'M': // Move
                        Conditions[Parsing].move = nextLine[27] == 'r' ? 1 : -1;
                        return;
                    case 'C': // Continue
                        Conditions[Parsing].next = ToKey(nextLine[26]);
                        Parsing++;
                        return;
                    default:
                        Console.WriteLine("Parse fail!");
                        break;
                }
            }
        }

        class Program
        {
            public Program(string input)
            {
                List<State> states = new();
                State state = null;
                foreach (var line in input.Split("\n"))
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        if (state != null) states.Add(state);
                        state = null;
                        continue;
                    }

                    switch (line[0])
                    {
                        case 'B': //Begin in state A.
                            Start = ToKey(line[15]);
                            break;

                        case 'P': //Perform a diagnostic checksum after 12399302 steps.
                            Diagnostic = Util.ExtractNumbers(line)[0];
                            break;

                        case 'I':
                            state = new State();
                            break;

                        case ' ':
                            if (line[4] == '-') state.Feed(line);
                            break;

                        default:
                            Console.WriteLine("Tokenization failed");
                            break;
                    }
                }

                States = states.ToArray();
            }

            public int Run()
            {
                HashSet<int> tape = new();
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

        public static int Part1(string input)
        {
            var program = new Program(input);
            return program.Run();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}