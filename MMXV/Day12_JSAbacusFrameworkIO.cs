using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Advent.MMXV
{
    public class Day12 : IPuzzle
    {
        public string Name { get { return "2015-12";} }

        public static IEnumerable<int> FindNumbers(string input)
        {
            StringBuilder current = new StringBuilder();
            for (int i=0; i<input.Length; ++i)
            {
                if (input[i]=='-' || (input[i]>='0' && input[i]<='9'))
                {
                    current.Append(input[i]);
                }
                else
                {
                    if (current.Length>0)
                    {
                        yield return int.Parse(current.ToString());
                        current = new StringBuilder();
                    }
                }
            }
            if (current.Length>0)
            {
                yield return int.Parse(current.ToString());
            }
        }

        public static int Part1(string input)
        {
            return FindNumbers(input).Sum();
        }

        static bool HasRed(dynamic jsonObj)
        {
            if (jsonObj.red != null) return true;

            foreach (var child in jsonObj)
            {
                if (child.Value.Type == JTokenType.String && child.Value == "red") 
                    return true;
            }

            return false;
        }

        static int GetSum(dynamic jsonObj)
        {
            if (jsonObj.Type == JTokenType.String) return 0;
            if (jsonObj.Type == JTokenType.Integer) return (int)jsonObj;

            if (jsonObj.Type == JTokenType.Array) 
            {
                int sum = 0;

                foreach (var child in jsonObj)
                {
                    sum += GetSum(child);
                }
                return sum;
            }


            if (HasRed(jsonObj))
            {
                return 0;
            }
            else
            {
                //Console.WriteLine(jsonObj);
                int sum = 0;
                foreach (var child in jsonObj)
                {
                    //Console.WriteLine($"{child.Name}");
                    sum += GetSum(child.Value);
                }
                return sum;
            }
        }

        public static int Part2(string input)
        {
            dynamic jsonObj = JsonConvert.DeserializeObject(input);
         
            return GetSum(jsonObj);
        }

        public void Run(string input, System.IO.TextWriter console)
        {

            // var data = FindNumbers(input);
            // foreach (var i in data)
            // {
            //     console.WriteLine(i);
            // }

            // console.WriteLine(Part2("[1,2,3]"));
            // console.WriteLine(Part2("[1,{\"c\":\"red\",\"b\":2},3"));
            // console.WriteLine(Part2("{\"d\":\"red\",\"e\":[1,2,3,4],\"f\":5}"));
            // console.WriteLine(Part2("[1,\"red\",5]"));

            console.WriteLine("- Pt1 - "+Part1(input));
            console.WriteLine("- Pt2 - "+Part2(input));
        }
    }
}