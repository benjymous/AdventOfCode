using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advent.MMXVI
{
    public class Day09 : IPuzzle
    {
        public string Name { get { return "2016-09";} }
 
        static string Decompress(string input)
        {
            var sb = new StringBuilder();
            int i=0;
            while (i<input.Length)
            {
                var c = input[i];
                if (c=='(')
                {
                    int start = i;
                    while (input[i++]!=')');

                    var cmd = input.Substring(start+1, i-start-2);

                    //Console.WriteLine(cmd);

                    var bits = cmd.Split("x").Select(i => int.Parse(i)).ToArray();

                   
                    var sb2 = new StringBuilder();
                    int j=0;
                    while (j++ < bits[0])
                    {
                        sb2.Append(input[i++]);
                    }

                    for (var k=0; k<bits[1]; ++k)
                    {
                        sb.Append(sb2.ToString());
                    }

                }
                else
                {
                    sb.Append(c);
                    i++;
                }

                
            }
            return sb.ToString();
        }

        public static int Part1(string input)
        {
            var stripped = input.Trim();
            var decompressed = Decompress(stripped);
            return decompressed.Length;
        }

        public static int Part2(string input)
        {
            input = input.Trim();
            
            var count = 0;

            while(input.Contains("("))
            {
                input = Decompress(input);
                Console.WriteLine($"{count++}, {input.Length}, {input.Where(c => c=='(').Count()}");
            }

            return 0;
        }

        public void Run(string input, System.IO.TextWriter console)
        {
            Util.Test(Decompress("A(1x5)BC"), "ABBBBBC");

            console.WriteLine("- Pt1 - "+Part1(input));
            //console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}