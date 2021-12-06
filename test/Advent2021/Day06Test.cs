using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day06Test
    {
        string input = Util.GetInput<Day06>();

        string test = "3,4,3,1,2";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Fishes01Test()
        {
            Assert.AreEqual((long)5934, Advent2021.Day06.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Fishes02Test()
        {
            Assert.AreEqual((long)26984457539, Advent2021.Day06.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Fishes_Part1_Regression()
        {
            Assert.AreEqual(386536, Day06.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Fishes_Part2_Regression()
        {
            Assert.AreEqual(1732821262171, Day06.Part2(input));
        }
    }
}
