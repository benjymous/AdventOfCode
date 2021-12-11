using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC.Advent2017
{
    public class Day25 : IPuzzle
    {
        public string Name => "2017-25";

        struct Conditional
        {
            public int Write { get; set; }
            public int Move { get; set; }
            public char Next { get; set; }
        }


        class State
        {
            public char ID { get; private set; }
            public State(string[] initialLine)
            {
                //In state A:
                ID = initialLine[2][0];
                Parsing = 0;
                Conditions[0] = new Conditional();
                Conditions[1] = new Conditional();
            }

            public int Parsing = -1;

            public Conditional[] Conditions = new Conditional[2];

            public void Feed(string[] nextLine)
            {
                /*
                - Write the value 1.
                - Move one slot to the right.
                - Continue with state B.
                */

                switch (nextLine[1])
                {
                    case "Write":
                        Conditions[Parsing].Write = int.Parse(nextLine[4].Replace(".", ""));
                        return;
                    case "Move":
                        Conditions[Parsing].Move = nextLine[6][0] == 'r' ? 1 : -1;
                        return;
                    case "Continue":
                        Conditions[Parsing].Next = nextLine[4][0];
                        Parsing++;
                        return;
                    default:
                        Console.WriteLine("Parse fail!");
                        break;
                }
            }
        }

        public class Program
        {
            public Program(string input)
            {
                input = input.Replace("\r", "") + "\n";
                State state = null;
                foreach (var line in input.Split("\n"))
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        if (state != null)
                        {
                            States[state.ID] = state;
                        }
                        state = null;
                        continue;
                    }

                    var tokens = line.Trim().Split(" ");
                    switch (tokens[0])
                    {
                        case "Begin":
                            //Begin in state A.
                            Start = tokens[3][0];
                            break;

                        case "Perform":
                            //Perform a diagnostic checksum after 12399302 steps.
                            Diagnostic = int.Parse(tokens[5]);
                            break;

                        case "In":
                            state = new State(tokens);
                            break;

                        case "If":
                            break;

                        case "-":
                            state.Feed(tokens);
                            break;


                        default:
                            Console.WriteLine("Tokenization failed");
                            break;
                    }
                }

            }

            public void Run()
            {
                State = States[Start];
                for (int cycle = 0; cycle < Diagnostic; ++cycle)
                {
                    var condition = State.Conditions[ReadTape()];

                    WriteTape(condition.Write);
                    Position += condition.Move;
                    State = States[condition.Next];
                }
            }

            public int ReadTape()
            {
                if (Tape.TryGetValue(Position, out var value))
                {
                    return value;
                }
                return 0;
            }

            public void WriteTape(int val) => Tape[Position] = val;

            public int Checksum()
            {
                return Tape.Values.Sum();
            }

            Dictionary<int, int> Tape = new Dictionary<int, int>();

            State State;
            char Start = '?';
            int Position = 0;
            int Diagnostic = 0;

            Dictionary<char, State> States = new Dictionary<char, State>();
        }

        public static int Part1(string input)
        {
            var program = new Program(input);
            program.Run();
            return program.Checksum();
        }

        public void Run(string input, ILogger logger)
        {
            logger.WriteLine("- Pt1 - " + Part1(input));
        }
    }
}