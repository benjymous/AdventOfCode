using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day09Test
    {
        readonly string input = Util.GetInput<Day09>();

        [TestCategory("Test")]
        [DataRow("35,20,15,25,47,40,62,55,65,95,102,117,150,182,127,219,299,277,309,576", 5, 127)]
        [TestMethod]
        public void Encoding01Test(string input, int preamble, int expected)
        {
            Assert.AreEqual(expected, (int)Day09.Part1(input, preamble));
        }

        [TestCategory("Test")]
        [DataRow("35,20,15,25,47,40,62,55,65,95,102,117,150,182,127,219,299,277,309,576", 5, 62)]
        [TestMethod]
        public void Encoding02Test(string input, int preamble, int expected)
        {
            Assert.AreEqual(expected, (int)Day09.Part2(input, preamble));
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void Encoding_Part1_Regression()
        {
            Assert.AreEqual(1124361034, Day09.Part1(input));
        }

        [TestCategory("Regression")]
        [TestMethod]
        public void Encoding_Part2_Regression()
        {
            Assert.AreEqual(129444555, Day09.Part2(input));
        }
    }
}
