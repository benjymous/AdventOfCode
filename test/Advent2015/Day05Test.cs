using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day05Test
    {
        readonly string input = Util.GetInput<Day05>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void NaughtyStrings_Part1_Regression()
        {
            Assert.AreEqual(258, Day05.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void NaughtyStrings_Part2_Regression()
        {
            Assert.AreEqual(53, Day05.Part2(input));
        }
    }
}
