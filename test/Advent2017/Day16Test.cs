using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day16Test
    {
        readonly string input = Util.GetInput<Day16>();

        [DataRow("s1,x3/4,pe/b", "abcde", "baedc")]
        [DataRow("s1,x3/4,pe/b", "baedc", "ceadb")]
        [DataTestMethod]
        public void Dance01Test(string input, string start, string expected)
        {
            Assert.AreEqual(expected, Advent2017.Day16.DoDance(input, start));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Promenade_Part1_Regression()
        {
            Assert.AreEqual("lgpkniodmjacfbeh", Advent2017.Day16.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Promenade_Part2_Regression()
        {
            Assert.AreEqual("hklecbpnjigoafmd", Advent2017.Day16.Part2(input));
        }

    }
}
