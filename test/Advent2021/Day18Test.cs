using AoC.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static AoC.Advent2021.Day18;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day18Test
    {
        string input = Util.GetInput<Day18>();

        string AsString(Val v) => v.IsPair ? $"[{AsString(v.first)},{AsString(v.second)}]" : v.Value.ToString();
        public static Val Parse(string data) => Day18.Val.Parse(data.ToQueue());

        [TestCategory("Test")]


        [DataRow("[[[[[9,8],1],2],3],4]", "[[[[0,9],2],3],4]")]
        [DataRow("[7,[6,[5,[4,[3,2]]]]]", "[7,[6,[5,[7,0]]]]")]
        [DataRow("[[6,[5,[4,[3,2]]]],1]", "[[6,[5,[7,0]]],3]")]
        [DataRow("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]")]
        [DataRow("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", "[[3,[2,[8,0]]],[9,[5,[7,0]]]]")]
        [DataTestMethod]
        public void TestSingleExplode(string data, string expected)
        {
            var number = Parse(data);
            Assert.AreEqual(data, AsString(number));
            number.TryExplode();
            var result = AsString(number);
            Assert.AreEqual(expected, result);
        }
        [DataRow(9, "9")]
        [DataRow(10, "[5,5]")]
        [DataRow(11, "[5,6]")]
        [DataRow(12, "[6,6]")]
        [DataTestMethod]
        public void TestSingleSplit(int val, string expected)
        {
            var number = new Val { Value = val };
            number.TrySplit();
            var result = AsString(number);
            Assert.AreEqual(expected, result);
        }

        [DataRow("[[[[[4,3],4],4],[7,[[8,4],9]]],[1,1]]", "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
        [DataTestMethod]
        public void TestReduce(string data, string expected)
        {
            var number = Parse(data);
            Assert.AreEqual(data, AsString(number));
            number.Reduce();
            var result = AsString(number);
            Assert.AreEqual(expected, result);
        }

        [DataRow(new string[] { "[[[[4,3],4],4],[7,[[8,4],9]]]", "[1,1]" }, "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]")]
        [DataRow(new string[] { "[1,1]", "[2,2]", "[3,3]", "[4,4]" }, "[[[[1,1],[2,2]],[3,3]],[4,4]]")]
        [DataRow(new string[] { "[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]" }, "[[[[3,0],[5,3]],[4,4]],[5,5]]")]
        [DataRow(new string[] { "[1,1]", "[2,2]", "[3,3]", "[4,4]", "[5,5]", "[6,6]" }, "[[[[5,0],[7,4]],[5,5]],[6,6]]")]

        [DataRow(new string[]{"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]"
        , "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]"
        }, "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]")]

        [DataRow(new string[]{"[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]"
        , "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]"
        }, "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]")]

        [DataRow(new string[]{"[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]"
        , "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]"
        }, "[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]")]

        [DataRow(new string[]{"[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]"
        , "[7,[5,[[3,8],[1,4]]]]"
        }, "[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]")]

        [DataRow(new string[]{"[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]"
        , "[[2,[2,2]],[8,[8,1]]]"
        }, "[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]")]

        [DataRow(new string[]{"[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]"
        , "[2,9]"
        }, "[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]")]

        [DataRow(new string[]{"[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]"
        , "[1,[[[9,3],9],[[9,0],[0,7]]]]"
        }, "[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]")]

        [DataRow(new string[]{"[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]"
        , "[[[5,[7,4]],7],1]"
        }, "[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]")]

        [DataRow(new string[]{"[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]"
        , "[[[[4,2],2],6],[8,7]]"
        }, "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]")]


        [DataRow(new string[]{"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]",
                            "[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]",
                            "[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]",
                            "[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]",
                            "[7,[5,[[3,8],[1,4]]]]",
                            "[[2,[2,2]],[8,[8,1]]]",
                            "[2,9]",
                            "[1,[[[9,3],9],[[9,0],[0,7]]]]",
                            "[[[5,[7,4]],7],1]",
                            "[[[[4,2],2],6],[8,7]]"},
            "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]")]
        [DataTestMethod]
        public void TestAdd(string[] values, string expected)
        {
            var first = values.First();
            var number = Parse(first);
            Assert.AreEqual(first, AsString(number));       

            foreach (var next in values.Skip(1))
            {
                Console.WriteLine($"  {number}");
                var v = Parse(next);
                Console.WriteLine($"+ {v}");
                Assert.AreEqual(next, AsString(v));

                number = Add(number, v);

                Console.WriteLine($"= {number}");
                Console.WriteLine();
            }

            Console.WriteLine($"? {expected}");

            var result = AsString(number);
            Assert.AreEqual(expected, result);
        }

    
        [DataRow("[[1,2],[[3,4],5]]", 143)]
        [DataRow("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]", 1384)]
        [DataRow("[[[[1,1],[2,2]],[3,3]],[4,4]]", 445)]
        [DataRow("[[[[3,0],[5,3]],[4,4]],[5,5]]", 791)]
        [DataRow("[[[[5,0],[7,4]],[5,5]],[6,6]]", 1137)]
        [DataRow("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]", 3488)]
        [DataTestMethod]
        public void TestMagnitude(string data, int expected)
        {
            var number = Parse(data);
            var result = number.Magnitude;
            Assert.AreEqual(expected, result);
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Magnitude_Part1_Regression()
        {
            Assert.AreEqual(4072, Day18.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Magnitude_Part2_Regression()
        {
            Assert.AreEqual(4483, Day18.Part2(input));
        }
    }
}
