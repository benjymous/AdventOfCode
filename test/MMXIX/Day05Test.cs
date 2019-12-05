using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestClass]
    public class Day05Test
    {
        [DataRow("1002,4,3,4,33", "1002,4,3,4,99")]
        [DataTestMethod]
        public void CPU2Test1(string input, string expected)
        {
            var cpu = new IntCPU(input);
            cpu.Run();
            Assert.AreEqual(expected, cpu.ToString());
        }

        [DataTestMethod]
        public void CPU2_Part1_Regression()
        {
            var d = new Day05();
            var input = Util.GetInput(d);
            Assert.AreEqual("0,0,0,0,0,0,0,0,0,16574641", Day05.Part1(input));
        }

        [DataTestMethod]
        public void CPU2_Part2_Regression()
        {
            var d = new Day05();
            var input = Util.GetInput(d);
            Assert.AreEqual(0, Day05.Part2(input));
        }
    }
}
