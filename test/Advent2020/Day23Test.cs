using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2020.Test
{
    [TestCategory("2020")]
    [TestCategory("Circle")]
    [TestClass]
    public class Day23Test
    {
        readonly string input = Util.GetInput<Day23>();

        [TestCategory("Test")]
        [DataRow("389125467", 10, "92658374")]
        [DataRow("389125467", 100, "67384529")]
        [DataTestMethod]
        public void Circle1Test(string input, int rounds, string expected)
        {
            Assert.AreEqual(expected, Day23.Part1(input, rounds));
        }

        [TestCategory("Test")]
        [DataTestMethod]
        public void Circle2Test()
        {
            Assert.AreEqual(149245887792, Day23.Part2("389125467"));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Circle_Part1_Regression()
        {
            Assert.AreEqual("36542897", Day23.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Circle_Part2_Regression()
        {
            Assert.AreEqual(562136730660, Day23.Part2(input));
        }
    }
}
