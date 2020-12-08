using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXX.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day05Test
    {
        string input = Util.GetInput<Day05>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Boarding_Part1_Regression()
        {
            Assert.AreEqual(938, Day05.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Boarding_Part2_Regression()
        {
            Assert.AreEqual(696, Day05.Part2(input));
        }
    }
}
