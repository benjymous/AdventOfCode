using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXVIII.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day19Test
    {
        string input = Util.GetInput<Day19>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ReverseEngineer2_Part1_Regression()
        {
            Assert.AreEqual(1080, Day19.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void ReverseEngineer2_Part2_Regression()
        {
            Assert.AreEqual(11106760, Day19.Part2(input));
        }

    }
}
