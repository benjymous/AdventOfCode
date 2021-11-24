using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day06Test
    {
        string input = Util.GetInput<Day06>();

        [TestCategory("Test")]
        [DataRow("0\t2\t7\t0", 5)]
        [DataTestMethod]
        public void Redistribution01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2017.Day06.Part1(input));
        }

        //[TestCategory("Test")]
        //[DataRow("0,3,0,1,-3", 10)]
        //[DataTestMethod]
        //public void Twisty02Test(string input, int expected)
        //{
        //    Assert.AreEqual(expected, MMXVII.Day05.Part2(input));
        //}

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Redistribution_Part1_Regression()
        {
            Assert.AreEqual(7864, Advent2017.Day06.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Redistribution_Part2_Regression()
        {
            Assert.AreEqual(1695, Advent2017.Day06.Part2(input));
        }

    }
}
