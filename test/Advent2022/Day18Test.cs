using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestCategory("FloodFill")]
    [TestClass]
    public class Day18Test
    {
        readonly string input = Util.GetInput<Day18>();
        readonly string test = @"2,2,2
1,2,2
3,2,2
2,1,2
2,3,2
2,2,1
2,2,3
2,2,4
2,2,6
1,2,5
3,2,5
2,1,5
2,3,5".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void Obsidian01Test()
        {
            Assert.AreEqual(64, Day18.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Obsidian02Test()
        {
            Assert.AreEqual(58, Day18.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Obsidian_Part1_Regression()
        {
            Assert.AreEqual(3576, Day18.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Obsidian_Part2_Regression()
        {
            Assert.AreEqual(2066, Day18.Part2(input));
        }
    }
}
