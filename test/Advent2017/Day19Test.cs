using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestCategory("ManhattanVector")]
    [TestCategory("Direction")]
    [TestClass]
    public class Day19Test
    {
        readonly string input = Util.GetInput<Day19>();

        readonly string test =
            "     |          \n" +
            "     |  +--+    \n" +
            "     A  |  C    \n" +
            " F---|--|-E---+ \n" +
            "     |  |  |  D \n" +
            "     +B-+  +--+ \n" +
            "                \n";

        [DataTestMethod]
        public void Tubes01Test()
        {
            Assert.AreEqual("ABCDEF", Day19.Part1(test));
        }

        [DataTestMethod]
        public void Tubes02Test()
        {
            Assert.AreEqual(38, Day19.Part2(test));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Tubes_Part1_Regression()
        {
            Assert.AreEqual("PBAZYFMHT", Day19.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Tubes_Part2_Regression()
        {
            Assert.AreEqual(16072, Day19.Part2(input));
        }

    }
}
