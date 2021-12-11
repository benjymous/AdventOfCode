using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestClass]
    public class Day11Test
    {
        string input = Util.GetInput<Day11>();

        string test = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Octoflash01Test()
        {
            Assert.AreEqual(1656, Day11.Part1(test.Replace("\r", "")));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Octoflash02Test()
        {
            Assert.AreEqual(195, Day11.Part2(test.Replace("\r", "")));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Octoflash_Part1_Regression()
        {
            Assert.AreEqual(1705, Day11.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Octoflash_Part2_Regression()
        {
            Assert.AreEqual(265, Day11.Part2(input));
        }
    }
}
