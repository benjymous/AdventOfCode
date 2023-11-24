using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace intcomp
{
    class Program
    {
        static void Main(string[] _)
        {

            //var file = File.ReadAllText(@"C:\Users\Rich\code\Advent\intcomp\examples\add.intcode").Split("\n");

            var file = File.ReadAllText(@"examples\add.intcode").Split('\n');

            Console.WriteLine(file);

            Dictionary<string, int> variables = [];
            Dictionary<string, int> keywords = new()
            {
                ["ADD"] = 1,
                ["MUL"] = 2,

                ["GET"] = 3,
                ["OUT"] = 4,

                ["HALT"] = 99
            };

            var outMem = new List<string>();

            // first pass, tokenize
            foreach (var line in file)
            {
                Console.WriteLine(line);
                if (line.Trim().StartsWith(';'))
                    continue; // comment

                var instr = line.Trim().Split(" ").Where(i => i.Length > 0);

                //Console.WriteLine(string.Join(",", instr));

                foreach (var bit in instr)
                {

                    if (bit.StartsWith(':'))
                    {
                        string var = bit.Replace(":", "");
                        variables[var] = outMem.Count;
                    }
                    else
                    {
                        outMem.Add(bit);
                    }
                }
            }

            // second pass, replace variables
            for (var i = 0; i < outMem.Count; ++i)
            {
                if (variables.TryGetValue(outMem[i], out int value))
                {
                    outMem[i] = value.ToString();
                }
                else if (keywords.TryGetValue(outMem[i], out int value2))
                {
                    outMem[i] = value2.ToString();
                }
            }

            Dictionary<int, string> backmap = [];
            foreach (var kvp in variables)
            {
                backmap[kvp.Value] = kvp.Key;
            }

            Console.WriteLine("----");
            int position = 0;
            foreach (var token in outMem)
            {
                string comment = "";
                if (backmap.TryGetValue(position, out string value))
                {
                    comment = $" ; {value}";
                }
                string lineOut = $"{position} '{token}'{comment}";
                Console.WriteLine(lineOut);
                position++;
            }
            Console.WriteLine("----");
            Console.WriteLine(string.Join(", ", outMem));
        }
    }
}
