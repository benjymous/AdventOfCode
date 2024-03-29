using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestCategory("RegexFactory")]
    [TestClass]
    public class Day09Test
    {
        readonly string input = Util.GetInput<Day09>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Routes_Part1_Regression()
        {
            Assert.AreEqual(117, Day09.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Routes_Part2_Regression()
        {
            Assert.AreEqual(909, Day09.Part2(input));
        }
    }
}
