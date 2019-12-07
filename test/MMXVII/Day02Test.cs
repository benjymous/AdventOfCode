using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVII.Test
{
    [TestClass]
    public class Day02Test
    {
        [DataRow("5\t1\t9\t5\n7\t5\t3\n2\t4\t6\t8\n", 18)]
        [DataTestMethod]
        public void Checksum01Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVII.Day02.Part1(input));
        }

        [DataRow("5\t9\t2\t8\n9\t4\t7\t3\n3\t8\t6\t5\n", 9)]
        [DataTestMethod]
        public void Checksum02Test(string input, int expected)
        {
            Assert.AreEqual(expected, MMXVII.Day02.Part2(input));
        }

        [DataTestMethod]
        public void Checksum_Part1_Regression()
        {
            var d = new MMXVII.Day02();
            var input = Util.GetInput(d);
            Assert.AreEqual(46402, MMXVII.Day02.Part1(input));
        }

        [DataTestMethod]
        public void Checksum_Part2_Regression()
        {
            var d = new MMXVII.Day02();
            var input = Util.GetInput(d);
            Assert.AreEqual(265, MMXVII.Day02.Part2(input));
        }

    }
}
