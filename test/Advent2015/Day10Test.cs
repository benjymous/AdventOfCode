using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2015.Test
{
    [TestCategory("2015")]
    [TestClass]
    public class Day10Test
    {
        readonly string input = Util.GetInput<Day10>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void SayIt_Part1_Regression()
        {
            Assert.AreEqual(492982, Day10.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void SayIt_Part2_Regression()
        {
            Assert.AreEqual(6989950, Day10.Part2(input));
        }
    }
}
