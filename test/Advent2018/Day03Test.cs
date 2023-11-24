using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day03Test
    {
        readonly string input = Util.GetInput<Day03>();

        [TestCategory("Test")]
        [DataRow("#1 @ 1,3: 4x4\n#2 @ 3,1: 4x4\n#3 @ 5,5: 2x2\n", 4)]
        [DataTestMethod]
        public void Overlaps01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day03.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("#1 @ 1,3: 4x4\n#2 @ 3,1: 4x4\n#3 @ 5,5: 2x2\n", "3")]
        [DataTestMethod]
        public void Overlapsy02Test(string input, string expected)
        {
            Assert.AreEqual(expected, Day03.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Overlaps_Part1_Regression()
        {
            Assert.AreEqual(121259, Day03.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Overlaps_Part2_Regression()
        {
            Assert.AreEqual("239", Day03.Part2(input));
        }
    }
}
