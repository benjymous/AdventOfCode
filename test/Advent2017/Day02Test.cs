using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestClass]
    public class Day02Test
    {
        readonly string input = Util.GetInput<Day02>();

        [TestCategory("Test")]
        [DataRow("5\t1\t9\t5\n7\t5\t3\n2\t4\t6\t8\n", 18)]
        [DataTestMethod]
        public void Checksum01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2017.Day02.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("5\t9\t2\t8\n9\t4\t7\t3\n3\t8\t6\t5\n", 9)]
        [DataTestMethod]
        public void Checksum02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2017.Day02.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Checksum_Part1_Regression()
        {
            Assert.AreEqual(46402, Advent2017.Day02.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Checksum_Part2_Regression()
        {
            Assert.AreEqual(265, Advent2017.Day02.Part2(input));
        }

    }
}
