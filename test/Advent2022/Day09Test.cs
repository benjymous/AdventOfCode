using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day09Test
    {
        readonly string input = Util.GetInput<Day09>();

        [TestCategory("Test")]
        [DataRow("R 4\nU 4\nL 3\nD 1\nR 4\nD 1\nL 5\nR 2", 13)]
        [DataTestMethod]
        public void Ropes01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day09.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("R 4\nU 4\nL 3\nD 1\nR 4\nD 1\nL 5\nR 2", 1)]
        [DataRow("R 5\nU 8\nL 8\nD 3\nR 17\nD 10\nL 25\nU 20", 36)]
        [DataTestMethod]
        public void Ropes02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day09.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Ropes_Part1_Regression()
        {
            Assert.AreEqual(6406, Day09.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Ropes_Part2_Regression()
        {
            Assert.AreEqual(2643, Day09.Part2(input));
        }
    }
}
