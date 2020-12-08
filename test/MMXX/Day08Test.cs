using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXX.Test
{
    [TestCategory("2020")]
    [TestCategory("Elf80")]
    [TestClass]
    public class Day08Test
    {
        string input = Util.GetInput<Day08>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Halting_Part1_Regression()
        {
            Assert.AreEqual(1553, Day08.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Halting_Part2_Regression()
        {
            Assert.AreEqual(1877, Day08.Part2(input));
        }
    }
}
