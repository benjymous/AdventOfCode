using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestCategory("PackedVect")]
    [TestClass]
    public class Day24Test
    {
        readonly string input = Util.GetInput<Day24>();
        readonly string test = "#.######\n#>>.<^<#\n#.<..<<#\n#>v.><>#\n#<^v^^>#\n######.#";

        [TestCategory("Test")]
        [TestMethod]
        public void ElfSnacks01Test()
        {
            Assert.AreEqual(18, Day24.Part1(test));
        }

        [TestCategory("Test")]
        [TestMethod]
        public void ElfSnacks02Test()
        {
            Assert.AreEqual(54, Day24.Part2(test));
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void ElfSnacks_Part1_Regression()
        {
            Assert.AreEqual(373, Day24.Part1(input));
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void ElfSnacks_Part2_Regression()
        {
            Assert.AreEqual(997, Day24.Part2(input));
        }
    }
}
