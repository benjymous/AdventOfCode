using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace intcomp
{
    class Program
    {
        static void Main(string[] args)
        {

            //var file = File.ReadAllText(@"C:\Users\Rich\code\Advent\intcomp\examples\add.intcode").Split("\n");

            var file = File.ReadAllText(@"examples\add.intcode").Split('\n');

            Console.WriteLine(file);

            Dictionary<string, int> variables = new Dictionary<string, int>();
            Dictionary<string, int> keywords = new Dictionary<string, int>();

            keywords["ADD"] = 1;
            keywords["MUL"] = 2;

            keywords["GET"] = 3;
            keywords["OUT"] = 4;

            keywords["HALT"] = 99;

            var outMem = new List<string>();

            // first pass, tokenize
            foreach (var line in file)
            {
                Console.WriteLine(line);
                if (line.Trim().StartsWith(";"))
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
                if (variables.ContainsKey(outMem[i]))
                {
                    outMem[i] = variables[outMem[i]].ToString();
                }
                else if (keywords.ContainsKey(outMem[i]))
                {
                    outMem[i] = keywords[outMem[i]].ToString();
                }
            }


            Dictionary<int, string> backmap = new Dictionary<int, string>();
            foreach (var kvp in variables)
            {
                backmap[kvp.Value] = kvp.Key;
            }

            Console.WriteLine("----");
            int position = 0;
            foreach (var token in outMem)
            {
                string comment = "";
                if (backmap.ContainsKey(position))
                {
                    comment = $" ; {backmap[position]}";
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
