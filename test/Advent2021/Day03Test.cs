using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day03Test
    {
        string input = Util.GetInput<Day03>();

        string test = @"00100
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
        [DataTestMethod]
        public void Diagnostic01Test()
        {
            Assert.IsTrue(Advent2021.Day03.Part1(test.Replace("\r", "")) == 198);
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Diagnostic02Test()
        {
            Assert.IsTrue(Advent2021.Day03.Part2(test.Replace("\r", "")) == 230);
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Diagnostic_Part1_Regression()
        {
            Assert.AreEqual(3912944, Day03.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Diagnostic_Part2_Regression()
        {
            Assert.AreEqual(4996233, Day03.Part2(input));
        }
    }
}
