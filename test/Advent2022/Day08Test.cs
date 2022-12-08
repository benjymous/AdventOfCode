using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day08Test
    {
        readonly string input = Util.GetInput<Day08>();
        readonly string test = @"30373
25512
65332
33549
35390".Replace("\r", "");

        [TestCategory("Test")]
        [DataTestMethod]
        public void Treehouse01Test()
        {
            Assert.AreEqual(21, Day08.Part1(test));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Treehouse02Test()
        {
            Assert.AreEqual(8, Day08.Part2(test));
        }        

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Treehouse_Part1_Regression()
        {
            Assert.AreEqual(1538, Day08.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Treehouse_Part2_Regression()
        {
            Assert.AreEqual(496125, Day08.Part2(input));
        }
    }
}
