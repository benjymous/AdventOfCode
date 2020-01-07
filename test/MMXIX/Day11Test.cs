using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestClass]
    public class Day11Test
    {
        string input = Util.GetInput<Day11>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Painter_Part1_Regression()
        {
            Assert.AreEqual(2018, Day11.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Painter_Part2_Regression()
        {     
            const string expectedHash = "97A0FB4C860B850BEC2A81CDFB9595A2052DC6F1B397BD653A56975352913922";
            Assert.AreEqual(expectedHash, Day11.Part2(input, new ConsoleOut()));
        }

    }
}
