using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace AoC.Advent2015
{
    public class Day07 : IPuzzle
    {
        public string Name => "2015-07";

        public class Component
        {
            [Regex(@"(\S+) (\S+) (\S+) -> (\S+)")]
            public Component(string in1, string op, string in2, string outName)
            {
                Input1 = in1;
                Operator = op;
                Input2 = in2;
                OutName = outName;
            }

            [Regex(@"(\S+) (\S+) -> (\S+)")]
            public Component(string op, string in1, string outName)
            {
                Input1 = in1;
                Operator = op;
                OutName = outName;
            }

            [Regex(@"(\S+) -> (\S+)")]
            public Component(string in1, string outName)
            {
                if (int.TryParse(in1, out int v))
                {
                    Value = v;
                    HasValue = true;
                }
                else
                {
                    Input1 = in1;
                }
                OutName = outName;
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
                var components = Util.RegexParse<Component>(input);

                index = components.ToDictionary(c => c.OutName, c => c);
            }

            public void Override(string wire, int value)
            {
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

                    comp.Value = comp.Operator switch
                    {
                        "AND" => Solve(comp.Input1) & Solve(comp.Input2),
                        "OR" => Solve(comp.Input1) | Solve(comp.Input2),
                        "LSHIFT" => Solve(comp.Input1) << Solve(comp.Input2),
                        "RSHIFT" => Solve(comp.Input1) >> Solve(comp.Input2),
                        "NOT" => 65535 - Solve(comp.Input1),
                        _ => throw new Exception("Unknown operator!"),
                    };
                    comp.HasValue = true;
                    return comp.Value;
                }

                return 0;
            }

            readonly Dictionary<string, Component> index;
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