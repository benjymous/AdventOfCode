using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestCategory("HexVector")]
    [TestClass]
    public class Day11Test
    {
        readonly string input = Util.GetInput<Day11>();

        [TestCategory("Test")]
        [DataRow("ne,ne,ne", 3)]
        [DataRow("ne,ne,sw,sw", 0)]
        [DataRow("ne,ne,s,s", 2)]
        [DataRow("se,sw,se,sw,sw", 3)]
        [DataTestMethod]
        public void HexEd01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day11.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void HexEd_Part1_Regression()
        {
            Assert.AreEqual(747, Day11.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void HexEd_Part2_Regression()
        {
            Assert.AreEqual(1544, Day11.Part2(input));
        }
    }
}
