using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day01Test
    {

        readonly string input = Util.GetInput<Day01>();

        readonly string test = @"1000
2000
3000

4000

5000
6000

7000
8000
9000

10000";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Calories01Test()
        {
            Assert.AreEqual(24000, Advent2022.Day01.Part1(test.Replace("\r", "")));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Calories02Test()
        {
            Assert.AreEqual(45000, Advent2022.Day01.Part2(test.Replace("\r", "")));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Calories_Part1_Regression()
        {
            Assert.AreEqual(68775, Day01.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Calories_Part2_Regression()
        {
            Assert.AreEqual(202585, Day01.Part2(input));
        }
    }
}
