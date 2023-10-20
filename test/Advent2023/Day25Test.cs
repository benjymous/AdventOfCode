using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2023.Test
{
    [TestCategory("2023")]
    [TestClass]
    public class Day25Test
    {
        string input = ""; // Util.GetInput<Day25>();

        [TestCategory("Test")]
        [DataRow("???", 0)]
        [DataTestMethod]
        public void _01Test(string input, int expected)
        {
            Assert.IsTrue(Day25.Part1(input) == expected);
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void _Part1_Regression()
        {
            Assert.AreEqual(0, Day25.Part1(input));
        }
    }
}
