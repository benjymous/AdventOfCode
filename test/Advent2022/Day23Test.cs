using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestCategory("PackedVect")]
    [TestClass]
    public class Day23Test
    {
        readonly string input = Util.GetInput<Day23>();
        readonly string test = @"..............
        ..............
        .......#......
        .....###.#....
        ...#...#.#....
        ....#...##....
        ...#.###......
        ...##.#.##....
        ....#..#......
        ..............
        ..............
        ..............".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void Diffusion01Test()
        {
            Assert.AreEqual(110, Day23.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Diffusion02Test()
        {
            Assert.AreEqual(20, Day23.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Diffusion_Part1_Regression()
        {
            Assert.AreEqual(3762, Day23.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Diffusion_Part2_Regression()
        {
            Assert.AreEqual(997, Day23.Part2(input));
        }
    }
}
