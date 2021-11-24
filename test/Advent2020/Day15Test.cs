using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day15Test
    {
        string input = Util.GetInput<Day15>();

        [TestCategory("Test")]
        [DataRow("0,3,6", 436)]
        [DataRow("1,3,2", 1)]
        [DataRow("2,1,3", 10)]
        [DataRow("1,2,3", 27)]
        [DataRow("2,3,1", 78)]
        [DataRow("3,2,1", 438)]
        [DataRow("3,1,2", 1836)]
        [DataTestMethod]
        public void Sequence1Test(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2020.Day15.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("0,3,6", 175594)]
        [DataRow("1,3,2", 2578)]
        [DataRow("2,1,3", 3544142)]
        [DataRow("1,2,3", 261214)]
        [DataRow("2,3,1", 6895259)]
        [DataRow("3,2,1", 18)]
        [DataRow("3,1,2", 362)]
        [DataTestMethod]
        public void Sequence2Test(string input, int expected)
        {
            Assert.AreEqual(expected, Advent2020.Day15.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Sequence_Part1_Regression()
        {
            Assert.AreEqual(758, Day15.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Sequence_Part2_Regression()
        {
            Assert.AreEqual(814, Day15.Part2(input));
        }
    }
}
