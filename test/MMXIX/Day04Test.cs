using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestClass]
    public class Day04Test
    {
        string input = Util.GetInput<Day04>();

        [TestCategory("Test")]
        [DataRow("111111", true)]
        [DataRow("223450", false)]
        [DataRow("123789", false)]
        [DataTestMethod]
        public void SecureTest01(string input, bool expected)
        {
            Assert.AreEqual(expected, Day04.CheckCriteria(input, false));
        }

        [TestCategory("Test")]
        [DataRow("112233", true)]
        [DataRow("123444", false)]
        [DataRow("111122", true)]
        [DataTestMethod]
        public void SecureTest02(string input, bool expected)
        {
            Assert.AreEqual(expected, Day04.CheckCriteria(input, true));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Secure_Part1_Regression()
        {
            Assert.AreEqual(1246, Day04.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Secure_Part2_Regression()
        {
            Assert.AreEqual(814, Day04.Part2(input));
        }

    }
}
