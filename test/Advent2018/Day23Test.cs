using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2018.Test
{
    [TestCategory("2018")]
    [TestCategory("RegexParse")]
    [TestClass]
    public class Day23Test
    {
        readonly string input = Util.GetInput<Day23>();

        [TestCategory("Test")]
        [DataRow("pos=<0,0,0>, r=4\npos=<1,0,0>, r=1\npos=<4,0,0>, r=3\npos=<0,2,0>, r=1\npos=<0,5,0>, r=3\npos=<0,0,3>, r=1\npos=<1,1,1>, r=1\npos=<1,1,2>, r=1\npos=<1,3,1>, r=1", 7)]
        [DataTestMethod]
        public void Teleportation01Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day23.Part1(input));
        }

        [TestCategory("Test")]
        [DataRow("pos=<10,12,12>, r=2\npos=<12,14,12>, r=2\npos=<16,12,12>, r=4\npos=<14,14,14>, r=6\npos=<50,50,50>, r=200\npos=<10,10,10>, r=5", 36)]
        [DataTestMethod]
        public void Teleportation02Test(string input, int expected)
        {
            Assert.AreEqual(expected, Day23.Part2(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Teleportation_Part1_Regression()
        {
            Assert.AreEqual(232, Day23.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Teleportation_Part2_Regression()
        {
            Assert.AreEqual(82010396, Day23.Part2(input));
        }
    }
}
