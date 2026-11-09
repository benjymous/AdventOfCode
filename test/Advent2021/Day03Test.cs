using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day03Test
    {
        readonly string input = Util.GetInput<Day03>();

        readonly string test = @"00100
11110
10110
10111
10101
01111
00111
11100
10000
11001
00010
01010";

        [TestCategory("Test")]
        [TestMethod]
        public void Diagnostic01Test()
        {
            Assert.AreEqual(198, Day03.Part1(test.Replace("\r", "")));
        }

        [TestCategory("Test")]
        [TestMethod]
        public void Diagnostic02Test()
        {
            Assert.AreEqual(230, Day03.Part2(test.Replace("\r", "")));
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void Diagnostic_Part1_Regression()
        {
            Assert.AreEqual(3912944, Day03.Part1(input));
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void Diagnostic_Part2_Regression()
        {
            Assert.AreEqual(4996233, Day03.Part2(input));
        }
    }
}
