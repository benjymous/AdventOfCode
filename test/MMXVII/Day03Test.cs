using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVII.Test
{
    [TestClass]
    public class Day03Test
    {
        [DataRow(1, 0)]
        [DataRow(12, 3)]
        [DataRow(23, 2)]
        [DataRow(1024, 31)]
        [DataTestMethod]
        public void Spiral01Test(int input, int expected)
        {
            Assert.AreEqual(expected, MMXVII.Day03.Part1(input));
        }

        [DataRow(1, 2)]
        [DataRow(4, 5)]
        [DataRow(23, 25)]
        [DataRow(750, 806)]
        [DataTestMethod]
        public void Spiral02Test(int input, int expected)
        {
            Assert.AreEqual(expected, MMXVII.Day03.Part2(input));
        }

        [DataTestMethod]
        public void Spiral_Part1_Regression()
        {
            var d = new MMXVII.Day03();
            var input = Util.GetInput(d);
            Assert.AreEqual(326, MMXVII.Day03.Part1(input));
        }

        [DataTestMethod]
        public void Checksum_Part2_Regression()
        {
            var d = new MMXVII.Day03();
            var input = Util.GetInput(d);
            Assert.AreEqual(363010, MMXVII.Day03.Part2(input));
        }

    }
}
