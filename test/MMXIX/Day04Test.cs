using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestClass]
    public class Day04Test
    {
        [DataRow("111111", true)]
        [DataRow("223450", false)]
        [DataRow("123789", false)]
        [DataTestMethod]
        public void SecureTest01(string input, bool expected)
        {
            Assert.AreEqual(expected, Day04.CheckCriteria(input, false));
        }

        [DataRow("112233", true)]
        [DataRow("123444", false)]
        [DataRow("111122", true)]
        [DataTestMethod]
        public void SecureTest02(string input, bool expected)
        {
            Assert.AreEqual(expected, Day04.CheckCriteria(input, true));
        }

        [DataTestMethod]
        public void Secure_Part1_Regression()
        {
            var d = new Day04();
            var input = Util.GetInput(d);
            Assert.AreEqual(1246, Day04.Part1(input));
        }

        [DataTestMethod]
        public void Secure_Part2_Regression()
        {
            var d = new Day04();
            var input = Util.GetInput(d);
            Assert.AreEqual(814, Day04.Part2(input));
        }

    }
}
