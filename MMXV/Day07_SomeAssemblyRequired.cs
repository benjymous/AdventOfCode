using System;
using System.Collections.Generic;
using System.Linq;

namespace Advent.MMXV
{
    public class Day07 : IPuzzle
    {
        public string Name { get { return "2015-07"; } }

        public class Component
        {
            public Component(string line)
            {
                var split1 = line.Split("->");
                OutName = split1[1].Trim();

                var split2 = split1[0].Trim().Split(" ");

                switch (split2.Length)
                {
                    case 1:
                        {
                            if (int.TryParse(split2[0], out int v))
                            {
                                Value = int.Parse(split2[0]);
                                HasValue = true;
                            }
                            else
                            {
                                Input1 = split2[0];
                            }
                        }
                        break;

                    case 2:
                        {
                            Operator = split2[0];
                            Input1 = split2[1];
                        }
                        break;
                    case 3:
                        {

                            Input1 = split2[0];
                            Operator = split2[1];
                            Input2 = split2[2];
                        }
                        break;

                    default:
                        throw new Exception("malformed circuit!");
                }



            }

            public bool HasValue { get; set; } = false;
            public int Value { get; set; }

            public string OutName { get; private set; } = null;

            public string Input1 { get; private set; } = null;
            public string Input2 { get; private set; } = null;

            public string Operator { get; private set; } = null;
        }

        public class Circuit
        {

            public Circuit(string input)
            {
                var components = Util.Parse<Component>(input);

                index = components.ToDictionary(c => c.OutName, c => c);
            }

            public void Override(string wire, int value)
            {
                var x = index[wire];
                index[wire].Value = value;
                index[wire].HasValue = true;
            }

            public int Solve(string output)
            {
                if (int.TryParse(output, out int val))
                {
                    return val;
                }

                if (!index.ContainsKey(output)) throw new Exception("Unexpected wire");

                var comp = index[output];
                if (comp.HasValue)
                {
                    return comp.Value;
                }
                if (comp.Operator == null)
                {
                    // wire
                    if (comp.Input1 != null)
                    {
                        comp.Value = Solve(comp.Input1);
                        comp.HasValue = true;
                        return comp.Value;
                    }
                }
                else
                {

                    switch (comp.Operator)
                    {
                        case "AND":
                            comp.Value = Solve(comp.Input1) & Solve(comp.Input2);
                            break;

                        case "OR":
                            comp.Value = Solve(comp.Input1) | Solve(comp.Input2);
                            break;

                        case "LSHIFT":
                            comp.Value = Solve(comp.Input1) << Solve(comp.Input2);
                            break;


                        case "RSHIFT":
                            comp.Value = Solve(comp.Input1) >> Solve(comp.Input2);
                            break;

                        case "NOT":
                            comp.Value = 65535 - Solve(comp.Input1);
                            break;

                        default: throw new Exception("Unknown operator!");
                    }
                    comp.HasValue = true;
                    return comp.Value;
                }

                return 0;
            }

            Dictionary<string, Component> index;
        }


        public static int Part1(string input)
        {
            var circuit = new Circuit(input);

            return circuit.Solve("a");
        }

        public static int Part2(string input)
        {
            var circuit = new Circuit(input);

            circuit.Override("b", 16076);

            return circuit.Solve("a");
        }

        public void Run(string input, ILogger logger)
        {
            // var example = "123 -> x\n456 -> y\nx AND y -> d\nx OR y -> e\nx LSHIFT 2 -> f\ny RSHIFT 2 -> g\nNOT x -> h\nNOT y -> i";
            // var circuit = new Circuit(example);

            // Console.WriteLine(circuit.Solve("d"));
            // Console.WriteLine(circuit.Solve("e"));
            // Console.WriteLine(circuit.Solve("f"));
            // Console.WriteLine(circuit.Solve("g"));
            // Console.WriteLine(circuit.Solve("h"));
            // Console.WriteLine(circuit.Solve("i"));
            // Console.WriteLine(circuit.Solve("x"));
            // Console.WriteLine(circuit.Solve("y"));


            logger.WriteLine("- Pt1 - " + Part1(input));
            logger.WriteLine("- Pt2 - " + Part2(input));
        }
    }
}