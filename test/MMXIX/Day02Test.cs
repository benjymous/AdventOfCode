using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Advent.MMXIX.Test
{
    [TestClass]
    public class Day02Test
    {
        [DataRow("1,0,0,0,99", "2,0,0,0,99")]
        [DataRow("2,3,0,3,99", "2,3,0,6,99")]
        [DataRow("2,4,4,5,99,0", "2,4,4,5,99,9801")]
        [DataRow("1,1,1,4,99,5,6,0,99", "30,1,1,4,2,5,6,0,99")]
        [DataTestMethod]
        public void CPU01Test(string input, string expected)
        {
            var cpu = new Day02.IntCPU(input);
            cpu.Run();
            Assert.AreEqual(expected, cpu.ToString());
        }


    }
}
