using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestClass]
    public class Day15Test
    {
        string input = Util.GetInput<Day15>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Oxygen_Part1_Regression()
        {
            Assert.AreEqual(380, Day15.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Oxygen_Part2_Regression()
        {     
            Assert.AreEqual(410, Day15.Part2(input));
        }

    }
}
