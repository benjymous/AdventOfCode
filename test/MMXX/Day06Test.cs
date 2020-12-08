using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXX.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day06Test
    {
        string input = Util.GetInput<Day06>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Customs_Part1_Regression()
        {
            Assert.AreEqual(7110, Day06.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Customs_Part2_Regression()
        {
            Assert.AreEqual(3628, Day06.Part2(input));
        }
    }
}
