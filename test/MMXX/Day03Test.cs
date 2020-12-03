using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Advent.MMXX.Test
{
    [TestCategory("2020")]
    [TestClass]
    public class Day03Test
    {
        string input = Util.GetInput<Day03>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Toboggans_Part1_Regression()
        {
            Assert.AreEqual(184, Day03.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Toboggans_Part2_Regression()
        {
            Assert.AreEqual(2431272960, Day03.Part2(input));
        }

    }
}
