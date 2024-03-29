using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AoC.Advent2017.Test
{
    [TestCategory("2017")]
    [TestCategory("Circle")]
    [TestClass]
    public class Day17Test
    {
        readonly string input = Util.GetInput<Day17>();

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Spinlock_Part1_Regression()
        {
            Assert.AreEqual(640, Day17.Part1(input));
        }

        [TestCategory("Regression")]
        [DataTestMethod]
        public void Spinlock_Part2_Regression()
        {
            Assert.AreEqual(47949463, Day17.Part2(input));
        }
    }
}
