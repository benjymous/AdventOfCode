using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day18Test
    {
        readonly string input = Util.GetInput<Day18>();

        [TestCategory("Test")]
        [DataRow("1 + 2 * 3 + 4 * 5 + 6", 71)]
        [DataRow("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [DataRow("2 * 3 + (4 * 5)", 26)]
        [DataRow("5 + (8 * 3 + 9 + 3 * 4 * 3)", 437)]
        [DataRow("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 12240)]
        [DataRow("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 13632)]
        [DataTestMethod]
        public void Solve1Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day18.Solve1(input));
        }

        [TestCategory("Test")]
        [DataRow("1 + 2 * 3 + 4 * 5 + 6", 231)]
        [DataRow("1 + (2 * 3) + (4 * (5 + 6))", 51)]
        [DataRow("2 * 3 + (4 * 5)", 46)]
        [DataRow("5 + (8 * 3 + 9 + 3 * 4 * 3)", 1445)]
        [DataRow("5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))", 669060)]
        [DataRow("((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2", 23340)]
        [DataTestMethod]
        public void Solve2Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day18.Solve2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Operations_Part1_Regression()
        {
            Assert.AreEqual(13976444272545, Day18.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Operations_Part2_Regression()
        {
            Assert.AreEqual(88500956630893, Day18.Part2(input));
        }
    }
}
