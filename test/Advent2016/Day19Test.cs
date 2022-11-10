using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2016.Test
{
    [TestCategory("2016")]
    [TestCategory("Circle")]
    [TestClass]
    public class Day19Test
    {
        readonly string input = Util.GetInput<Day19>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PresentTheft_Part1_Regression()
        {
            Assert.AreEqual(1815603, Day19.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void PresentTheft_Part2_Regression()
        {
            Assert.AreEqual(1410630, Day19.Part2(input));
        }
    }
}
