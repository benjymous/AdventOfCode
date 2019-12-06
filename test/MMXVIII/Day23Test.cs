using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestClass]
    public class Day23Test
    {
        [DataRow("pos=<0,0,0>, r=4\npos=<1,0,0>, r=1\npos=<4,0,0>, r=3\npos=<0,2,0>, r=1\npos=<0,5,0>, r=3\npos=<0,0,3>, r=1\npos=<1,1,1>, r=1\npos=<1,1,2>, r=1\npos=<1,3,1>, r=1", 7)]
        [DataTestMethod]
        public void Teleportation01Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day23.Part1(input));
        }

        [DataRow("pos=<10,12,12>, r=2\npos=<12,14,12>, r=2\npos=<16,12,12>, r=4\npos=<14,14,14>, r=6\npos=<50,50,50>, r=200\npos=<10,10,10>, r=5", 36)]
        [DataTestMethod]
        public void Teleportation02Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVIII.Day23.Part2(input));
        }

        [DataTestMethod]
        public void Teleportation_Part1_Regression()
        {
            var d = new Day23();
            var input = Util.GetInput(d);
            Assert.AreEqual(232, MMXVIII.Day23.Part1(input));
        }

        [DataTestMethod]
        public void Teleportation_Part2_Regression()
        {
            var d = new Day23();
            var input = Util.GetInput(d);
            Assert.AreEqual(82010396, MMXVIII.Day23.Part2(input));
        }

    }
}
