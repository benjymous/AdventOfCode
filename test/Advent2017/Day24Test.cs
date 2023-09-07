using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day24Test
    {
        readonly string input = Util.GetInput<Day24>();

        [TestCategory("Test")]
        [DataRow("0/2\n2/2\n2/3\n3/4\n3/5\n0/1\n10/1\n9/10", 31)]
        [DataTestMethod]
        public void Chains01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day24.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("0/2\n2/2\n2/3\n3/4\n3/5\n0/1\n10/1\n9/10", 19)]
        [DataTestMethod]
        public void Chains02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day24.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Chains_Part1_Regression()
        {
            Assert.AreEqual(1656, Day24.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Chains_Part2_Regression()
        {
            Assert.AreEqual(1642, Day24.Part2(input));
        }

    }
}
