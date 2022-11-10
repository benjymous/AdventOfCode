using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2021.Test
{
    [TestCategory("2021")]
    [TestCategory("FloodFill")]
    [TestClass]
    public class Day09Test
    {
        readonly string input = Util.GetInput<Day09>();

        readonly string test = "2199943210\n3987894921\n9856789892\n8767896789\n9899965678";

        [TestCategory("Test")]
        [DataTestMethod]
        public void Basins01Test()
        {
            Assert.AreEqual(15, Day09.Part1(test.Replace("\r", "")));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Basins02Test()
        {
            Assert.AreEqual(1134, Day09.Part2(test.Replace("\r", "")));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Basins_Part1_Regression()
        {
            Assert.AreEqual(423, Day09.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Basins_Part2_Regression()
        {
            Assert.AreEqual(1198704, Day09.Part2(input));
        }
    }
}
