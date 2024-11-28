using AoC.Utils.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day13Test
    {
        readonly string input = Util.GetInput<Day13>();
        readonly string test = @"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        [DataRow("[1,1,3,1,1]", "[1,1,5,1,1]", true)]
        [DataRow("[[1],[2,3,4]]", "[[1],4]", true)]
        [DataRow("[9]", "[[8,7,6]]", false)]
        [DataRow("[[4,4],4,4]", "[[4,4],4,4,4]", true)]
        [DataRow("[7,7,7,7]", "[7,7,7]", false)]
        [DataRow("[]", "[3]", true)]
        [DataRow("[[[]]]", "[[]]", false)]
        [DataRow("[1,[2,[3,[4,[5,6,7]]]],8,9]", "[1,[2,[3,[4,[5,6,0]]]],8,9]", false)]
        public void CompareTest(string el1str, string el2str, bool rightOrder)
        {
            var el1 = Parser.FromString<Day13.Element>(el1str);
            var el2 = Parser.FromString<Day13.Element>(el2str);
            Assert.AreEqual(rightOrder, el1.CompareTo(el2) < 0);
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Distress01Test()
        {
            Assert.AreEqual(13, Day13.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Distress02Test()
        {
            Assert.AreEqual(140, Day13.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Distress_Part1_Regression()
        {
            Assert.AreEqual(6086, Day13.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Distress_Part2_Regression()
        {
            Assert.AreEqual(27930, Day13.Part2(input));
        }
    }
}
