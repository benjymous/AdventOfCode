using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXVIII.Test
{
    [TestCategory("2018")]
    [TestClass]
    public class Day12Test
    {
        string input = Util.GetInput<Day12>();


        [TestCategory("Regression")]
        [DataTestMethod]
        public void MarbleGame_Part1_Regression()
        {
            Assert.AreEqual(1715, Day12.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void MarbleGame_Part2_Regression()
        {
            Assert.AreEqual(1700000000011, Day12.Part2(input));
        }

    }
}
