using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXIX.Test
{
    [TestCategory("2019")]
    [TestCategory("IntCPU")]
    [TestClass]
    public class Day02Test
    {
        string input = Util.GetInput<Day02>();

        [TestCategory("Test")]
        [DataRow("1,0,0,0,99", "2,0,0,0,99")]
        [DataRow("2,3,0,3,99", "2,3,0,6,99")]
        [DataRow("2,4,4,5,99,0", "2,4,4,5,99,9801")]
        [DataRow("1,1,1,4,99,5,6,0,99", "30,1,1,4,2,5,6,0,99")]
        [DataTestMethod]
        public void CPUTest(string input, string expected)
        {
            var cpu = new NPSA.IntCPU(input);
            cpu.Run();
            Assert.AreEqual(expected, cpu.ToString());
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CPU_Part1_Regression()
        {
            Assert.AreEqual(3716250, Day02.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void CPU_Part2_Regression()
        {
            Assert.AreEqual(6472, Day02.Part2(input));
        }
    }
}
