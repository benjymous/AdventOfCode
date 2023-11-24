using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2022.Test
{
    [TestCategory("2022")]
    [TestClass]
    public class Day06Test
    {
        readonly string input = Util.GetInput<Day06>();

        [TestCategory("Test")]
        [DataRow("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
        [DataRow("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
        [DataRow("nppdvjthqldpwncqszvftbrmjlhg", 6)]
        [DataRow("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
        [DataRow("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
        [DataTestMethod]
        public void Tuning01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day06.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
        [DataRow("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
        [DataRow("nppdvjthqldpwncqszvftbrmjlhg", 23)]
        [DataRow("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
        [DataRow("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
        [DataTestMethod]
        public void Tuning02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day06.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Tuning_Part1_Regression()
        {
            Assert.AreEqual(1766, Day06.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Tuning_Part2_Regression()
        {
            Assert.AreEqual(2383, Day06.Part2(input));
        }
    }
}
