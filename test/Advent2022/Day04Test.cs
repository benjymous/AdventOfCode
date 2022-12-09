using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day04Test
    {

        readonly string input = Util.GetInput<Day04>();

        readonly string test = @"2-4,6-8
    2-3,4-5
    5-7,7-9
    2-8,3-7
    6-6,4-6
    2-6,4-8".Replace("\r", "");


        [TestCategory("Test")]
        [DataTestMethod]
        public void Cleanup01Test()
        {
            Assert.AreEqual(2, Day04.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Cleanup02Test()
        {
            Assert.AreEqual(4, Day04.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Cleanup_Part1_Regression()
        {
            Assert.AreEqual(580, Day04.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Cleanup_Part2_Regression()
        {
            Assert.AreEqual(895, Day04.Part2(input));
        }
    }
}
