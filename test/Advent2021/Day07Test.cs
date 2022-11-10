using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day07Test
    {
        readonly string input = Util.GetInput<Day07>();

        readonly string test = "16,1,2,0,4,2,7,1,2,14";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Crabs01Test()
        {
            Assert.AreEqual(37, Advent2021.Day07.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Crabs02Test()
        {
            Assert.AreEqual(168, Advent2021.Day07.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Crabs_Part1_Regression()
        {
            Assert.AreEqual(355150, Day07.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Crabs_Part2_Regression()
        {
            Assert.AreEqual(98368490, Day07.Part2(input));
        }
    }
}
